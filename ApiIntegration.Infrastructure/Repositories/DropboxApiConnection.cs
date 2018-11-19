using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiIntegration.Core.Models;
using Dropbox.Api;
using Dropbox.Api.Files;
using DropboxRestAPI;
using DropboxRestAPI.Models.Core;

namespace ApiIntegration.Infrastructure.Repositories
{
    public class DropboxApiConnection : IDropboxApiConnection
    {
        private Client dropboxClient;

        private string accessToken;

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(accessToken); }
        }

        readonly IDropboxUserToken dropboxUserToken;

        public DropboxApiConnection(string clientId, string clientSecret, string redirectUri, IDropboxUserToken dropboxUserToken)
        {
            this.dropboxUserToken = dropboxUserToken;

            var token = dropboxUserToken.TryRetrieveTokenFromDatabase(HardcodedUser.Id);

            accessToken = token.DropboxAccessToken;
            var options = new Options
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri,
                UseSandbox = true,
                AccessToken = accessToken
            };

            dropboxClient = new Client(options);
        }

        public async Task Authenticate(string authCode)
        {
            var token = await dropboxClient.Core.OAuth2.TokenAsync(authCode);

            accessToken = token.access_token;

            dropboxUserToken.UpdateOrCreateToken(HardcodedUser.Id, dropboxToken: accessToken);
        }

        public Uri GetAuthorizationUri()
        {
            var authRequestUrl = dropboxClient.Core.OAuth2.Authorize("code");

            return authRequestUrl;
        }

        public async Task<string> DownloadFile(string path)
        {
            string selectedFile = string.Empty;
            /* " */
            //path = "/fileuploadexactonline/accessdenied.png"; //"/" + folder + "/" + file;

            using (var dbx = new DropboxClient(accessToken))
            {
                using (var response = await dbx.Files.DownloadAsync(path))
                {
                    selectedFile = response.Response.Name;
                }
            }
            return selectedFile;
        }

        public async Task<IEnumerable<TreeNode>> GetFiles()
        {
            var nodes = new List<TreeNode>();
            using (var dbx = new DropboxClient(accessToken))
            {
                var rootFolder = await dbx.Files.ListFolderAsync(string.Empty);
                var rootNode = new TreeNode
                {
                    Name = "root",
                    Path = "/"
                };

                nodes = (from f in rootFolder.Entries
                         select new TreeNode
                         {
                             Name = f.Name,
                             Path = f.PathLower,
                             Children = MapChildrenNode(dbx, f.PathDisplay, f.IsFolder)
                         }).ToList();
            }

            return nodes;
        }

        //To upload a file
        public async Task<string> Upload(string folder, string file)
        {
            using (var dbx = new DropboxClient(accessToken))
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(@"D:\MILON\Files\document1.docx")))
                {
                    var response = await dbx.Files.UploadAsync("/" + folder + "/" + file, WriteMode.Overwrite.Instance, body: stream);

                    return response.Rev;
                }
            }
        }

        public void Dispose()
        {
            dropboxUserToken.Dispose();
        }

        private List<TreeNode> MapChildrenNode(DropboxClient dbx, string fileDir, bool isFolder)
        {
            var result = new List<TreeNode>();

            if (isFolder)
            {
                var rootFolder = dbx.Files.ListFolderAsync(fileDir);

                result = (from f in rootFolder.Result.Entries
                          select new TreeNode
                          {
                              Name = f.Name,
                              Path = f.PathLower
                          }).ToList();
            }
            return result;
        }
    }
}
