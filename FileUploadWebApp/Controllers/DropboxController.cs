using ApiIntegration.Infrastructure.Repositories;
using FileUploadWebApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FileUploadWebApp.Controllers
{
    public class DropboxController : Controller
    {
        private readonly IDropboxApiConnection dropboxApiConnection;

        public DropboxController(IDropboxApiConnection dropboxApiConnection)
        {
            this.dropboxApiConnection = dropboxApiConnection;
        }

        public ActionResult Authenticate()
        {
            var authUrl = dropboxApiConnection.GetAuthorizationUri();

            return Redirect(authUrl.AbsoluteUri);
        }

        [HttpGet]
        public async Task<ActionResult> DropboxFile()
        {
            var model = new ViewModel
            {
                UploadedFiles = await dropboxApiConnection.GetFiles()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DropboxFile(HttpPostedFileBase file)
        {
            var response = dropboxApiConnection.Upload("Apps", file.FileName);

            return RedirectToAction("DropboxFile");
        }

        public async Task<ActionResult> OAuth(string code)
        {
            await dropboxApiConnection.Authenticate(code);

            return RedirectToAction("Index", "DropboxExact");
        }
    }
}