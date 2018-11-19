using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploadWebApp.Controllers
{
    public class GoogleDriveExactController : Controller
    {
        // GET: GoogleDriveExact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            //GoogleDriveFilesRepository.FileUpload(file);
            //googleDriveApiConnection.FileUpload(file);

            return RedirectToAction("Index");
        }
    }
}