using ApiIntegration.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadWebApp.Models
{
    public class ViewModel
    {
        public bool DropboxIsAuthenticated { get; set; }

        public bool ExactIsAuthenticated { get; set; }

        public IEnumerable<TreeNode> UploadedFiles { get; set; }

        [Required(ErrorMessage = "You must select a file from the tree")]
        public string SelectedFile { get; set; }

        [Required(ErrorMessage = "You must specify a title for the document")]
        public string Title { get; set; }

        public ViewModel()
        {
            UploadedFiles = Enumerable.Empty<TreeNode>();
        }
    }
}