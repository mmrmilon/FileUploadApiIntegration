using System;
using ApiIntegration.Core.Models;
using DotNetOpenAuth.OAuth2;
using ExactOnline.Client.Sdk.Controllers;
using System.IO;
using ExactOnline.Client.Models;
using System.Collections.Generic;

namespace ApiIntegration.Infrastructure.Repositories
{
    public class ExactOnlineConnection : IExactOnlineConnection
    {
        private string endPoint;

        private  UserAgentClient oAuthClient;

        private ExactOnlineClient exactClient;

        private IAuthorizationState authorization;

        private int miscellaneousDocumentType = 0;

        private Guid generalCategory = Guid.Empty; //0ef65ad5-3b67-4901-acd0-ecceea85ef64

        public bool IsAuthenticated
        {
            get { return IsAccessTokenValid(); }
        }

        private readonly IDropboxUserToken dropboxUserToken;

        public ExactOnlineConnection(string clientId, string clientSecret, string redirectUri, string endPoint, IDropboxUserToken dropboxUserToken)
        {
            this.dropboxUserToken = dropboxUserToken;

            miscellaneousDocumentType = 55;

            generalCategory = Guid.Parse("3b6d3833-b31b-423d-bc3c-39c62b8f2b12"); //Guid.Parse(clientId);

            this.endPoint = endPoint;

            authorization = new AuthorizationState
            {
                Callback = new Uri(redirectUri)
            };

            var token = dropboxUserToken.TryRetrieveTokenFromDatabase(HardcodedUser.Id);

            authorization.AccessToken = token.ExactAccessToken;

            authorization.AccessTokenExpirationUtc = token.ExactAccessTokenExpiration;

            exactClient = IsAccessTokenValid() ? new ExactOnlineClient(endPoint, GetAccessToken) : exactClient;

            var serverDescription = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(string.Format("{0}/api/oauth2/auth", endPoint)),
                TokenEndpoint = new Uri(string.Format("{0}/api/oauth2/token", endPoint))
            };

            oAuthClient = new UserAgentClient(serverDescription, clientId, clientSecret);
            oAuthClient.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(clientSecret);
        }

        public void Authenticate(Uri responseUri)
        {
            authorization = oAuthClient.ProcessUserAuthorization(responseUri, authorization);

            exactClient = new ExactOnlineClient(endPoint, GetAccessToken);

            dropboxUserToken.UpdateOrCreateToken(HardcodedUser.Id, exactToken: authorization.AccessToken, exactTokenExpiration: authorization.AccessTokenExpirationUtc);
        }

        public Guid CreateDocument(string subject, string body)
        {
            try
            {
                
                var document = new Document
                {
                    Subject = subject,
                    Body = body,
                    Category = generalCategory,
                    Type = miscellaneousDocumentType,
                    DocumentDate = DateTime.UtcNow.Date
                };

                if (exactClient.For<Document>().Insert(ref document))
                {
                    return document.ID;
                }
            }
            catch (Exception ex)
            {
                string errror = ex.GetBaseException().Message;
            }

            return Guid.Empty;
        }

        public List<Document> GetDocuments()
        {
            var fields = new[] { "ID, Account, Subject, CreatorFullName, TypeDescription, DocumentViewUrl, Body" };

            var documents = exactClient.For<Document>().Select(fields).Top(5).Get();

            return documents;
        }

        public Uri GetAuthorizationUri()
        {
            return oAuthClient.RequestUserAuthorization(authorization);
        }

        public void Dispose()
        {
            dropboxUserToken.Dispose();
        }

        private string GetAccessToken()
        {
            return authorization.AccessToken;
        }

        private bool IsAccessTokenValid()
        {
            return !string.IsNullOrWhiteSpace(authorization.AccessToken)
                && authorization.AccessTokenExpirationUtc.HasValue
                && authorization.AccessTokenExpirationUtc.Value > DateTime.UtcNow;
        }
    }
}
