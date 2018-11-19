using System;
using System.Data.Entity;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure
{
    public interface IDropboxDbContext : IDisposable
    {
        DbSet<DropboxUser> DropboxUser { get; set; }

        DbSet<CopiedFileReference> CopiedFileReferences { get; set; }

        int SaveChanges();
    }
}
