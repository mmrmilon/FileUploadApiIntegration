using System.Data.Entity;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure
{
    public class DropboxDbContext : DbContext, IDropboxDbContext
    {
        public DbSet<DropboxUser> DropboxUser { get; set; }

        public DbSet<CopiedFileReference> CopiedFileReferences { get; set; }
    }
}
