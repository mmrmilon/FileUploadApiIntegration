using System;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure.Repositories
{
    public interface IDropboxUserToken : IDisposable
    {
        DropboxUser TryRetrieveTokenFromDatabase(Guid userId);

        void UpdateOrCreateToken(Guid userId, string dropboxToken = null, string exactToken = null, DateTime? exactTokenExpiration = null);
    }
}
