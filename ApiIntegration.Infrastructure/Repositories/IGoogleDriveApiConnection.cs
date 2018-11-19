using System;
using System.Web;
using System.Collections.Generic;
using ApiIntegration.Core.Models;

namespace ApiIntegration.Infrastructure.Repositories
{
    public interface IGoogleDriveApiConnection : IDisposable
    {
        List<GoogleDriveFile> GetDriveFiles();

        void FileUpload(HttpPostedFileBase file);

        string DownloadGoogleFile(string fileId);

        void DeleteFile(GoogleDriveFile files);
    }
}
