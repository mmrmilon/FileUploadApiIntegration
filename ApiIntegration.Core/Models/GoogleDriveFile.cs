using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Models
{
    public class GoogleDriveFile
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long? Size { get; set; }

        public long? Version { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
