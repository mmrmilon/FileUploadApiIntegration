using ExactOnline.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Infrastructure.Repositories
{
    public interface IExactOnlineConnection : IDisposable
    {
        bool IsAuthenticated { get; }

        Uri GetAuthorizationUri();

        void Authenticate(Uri responseUri);

        Guid CreateDocument(string subject, string body);

        List<Document> GetDocuments();
    }
}
