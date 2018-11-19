using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Models
{
    public class TreeNode
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public IEnumerable<TreeNode> Children { get; set; }

        public bool HasChildren
        {
            get { return Children.Any(); }
        }

        public TreeNode()
        {
            Children = Enumerable.Empty<TreeNode>();
        }
    }
}
