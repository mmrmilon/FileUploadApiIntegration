using ApiIntegration.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploadWebApp.Controllers
{
    public class ExactController : Controller
    {
        private readonly IExactOnlineConnection exactOnlineConnection;

        public ExactController(IExactOnlineConnection exactOnlineConnection)
        {
            this.exactOnlineConnection = exactOnlineConnection;
        }

        public ActionResult Authenticate()
        {
            var authUrl = exactOnlineConnection.GetAuthorizationUri();

            return Redirect(authUrl.AbsoluteUri);
        }

        public ActionResult ExactFile()
        {
            return View(exactOnlineConnection.GetDocuments());
        }

        public ActionResult OAuth()
        {
            exactOnlineConnection.Authenticate(this.Request.Url);

            return RedirectToAction("Index", "DropboxExact");
        }
    }
}