using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Models
{
    public class CopiedFileReference
    {
        public Guid Id { get; set; }

        public string ClientFilePath { get; set; }

        public Guid ExactId { get; set; }
    }
}
