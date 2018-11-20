using ApiIntegration.Infrastructure.Repositories;
using FileUploadWebApp.Models;
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
        //https://console.developers.google.com/apis/dashboard?project=aspdotnetcsharp-221915&duration=PT1H
        //public IGoogleDriveApiConnection googleDriveApiConnection;

        //public FileUploadController(IGoogleDriveApiConnection googleDriveApiConnection)
        //{
        //    this.googleDriveApiConnection = googleDriveApiConnection;
        //}

        // GET: FileUpload
        public ActionResult Upload()
        {
            List<GoogleDriveFiles> result = null;
            try
            {
                result = GoogleDriveFilesRepository.GetDriveFiles();
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("GoogleDriveConnectionError");
            }
            return View(result);
        }

        public ActionResult GoogleDriveConnectionError()
        {
            return View();
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