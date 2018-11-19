using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure.Repositories
{
    public interface IDropboxApiConnection : IDisposable
    {
        bool IsAuthenticated { get; }
        
        Task<IEnumerable<TreeNode>> GetFiles();

        Task<string> DownloadFile(string path);

        Task<string> Upload(string folder, string file);

        Uri GetAuthorizationUri();

        Task Authenticate(string authCode);
    }
}
