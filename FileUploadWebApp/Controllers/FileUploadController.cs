using ApiIntegration.Infrastructure.Repositories;
using FileUploadWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploadWebApp.Controllers
{
    public class FileUploadController : Controller
    {
        //public IGoogleDriveApiConnection googleDriveApiConnection;

        //public FileUploadController(IGoogleDriveApiConnection googleDriveApiConnection)
        //{
        //    this.googleDriveApiConnection = googleDriveApiConnection;
        //}
        // GET: FileUpload
        public ActionResult Upload()
        {
            return View(GoogleDriveFilesRepository.GetDriveFiles());
            //return View(googleDriveApiConnection.GetDriveFiles());
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            //GoogleDriveFilesRepository.FileUpload(file);
            //googleDriveApiConnection.FileUpload(file);

            return RedirectToAction("Upload");
        }
    }
}