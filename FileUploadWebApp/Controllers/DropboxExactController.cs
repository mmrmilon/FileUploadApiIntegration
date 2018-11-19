using ApiIntegration.Core.Models;
using ApiIntegration.Infrastructure.Repositories;
using FileUploadWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FileUploadWebApp.Controllers
{
    public class DropboxExactController : Controller
    {
        private readonly IDropboxApiConnection dropboxApiConnection;

        private readonly IExactOnlineConnection exactOnlineConnection;

        //private readonly ICopiedFileReferenceRepository _repository;

        public DropboxExactController(IDropboxApiConnection dropboxApiConnection, IExactOnlineConnection exactOnlineConnection)
        {
            this.dropboxApiConnection = dropboxApiConnection;

            this.exactOnlineConnection = exactOnlineConnection;
        }

        public async Task<ActionResult> Index()
        {
            var model = await GetOrCreateModel();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ViewModel model)
        {
            if (ModelState.IsValid)
            {
                var body = await dropboxApiConnection.DownloadFile(model.SelectedFile);
                var id = exactOnlineConnection.CreateDocument(model.Title, body);

                if (id == Guid.Empty)
                    return RedirectToAction("Result", new { success = false });

                //using (_repository)
                //{
                //    _repository.CreateReference(model.SelectedFile, id);
                //}

                return RedirectToAction("Result", new { success = true });
            }

            model = await GetOrCreateModel(model);

            return View(model);
        }

        public ActionResult Result(bool success)
        {
            var model = new ResultViewModel
            {
                Success = success,
                ResultText = success ? "File upload succesfully" : "Upload file failed",
                ButtonText = success ? "Upload another file" : "Try again"
            };

            return View(model);
        }

        private async Task<ViewModel> GetOrCreateModel(ViewModel model = null)
        {
            if (model == null)
                model = new ViewModel();

            model.DropboxIsAuthenticated = dropboxApiConnection.IsAuthenticated;

            model.ExactIsAuthenticated = exactOnlineConnection.IsAuthenticated;

            model.UploadedFiles = dropboxApiConnection.IsAuthenticated && exactOnlineConnection.IsAuthenticated ? await dropboxApiConnection.GetFiles() : Enumerable.Empty<TreeNode>();

            return model;
        }
    }
}