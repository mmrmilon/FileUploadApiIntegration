using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure.Repositories
{
    public class DropboxUserToken : IDropboxUserToken
    {
        private readonly IDropboxDbContext context;

        public DropboxUserToken(IDropboxDbContext context)
        {
            this.context = context;
        }

        public DropboxUser TryRetrieveTokenFromDatabase(Guid userId)
        {
            var userToken = context.DropboxUser.SingleOrDefault(x => x.UserId == userId);

            if (userToken != null)
            {
                return userToken;
            }

            return DropboxUser.Empty;
        }

        public void UpdateOrCreateToken(Guid userId, string dropboxToken = null, string exactToken = null, DateTime? exactTokenExpiration = null)
        {
            var userToken = context.DropboxUser.SingleOrDefault(x => x.UserId == userId);

            if (userToken == null)
            {
                userToken = new DropboxUser
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                context.DropboxUser.Add(userToken);
            }

            userToken.DropboxAccessToken = dropboxToken ?? userToken.DropboxAccessToken;
            userToken.ExactAccessToken = exactToken ?? userToken.ExactAccessToken;
            userToken.ExactAccessTokenExpiration = exactTokenExpiration ?? userToken.ExactAccessTokenExpiration;

            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
