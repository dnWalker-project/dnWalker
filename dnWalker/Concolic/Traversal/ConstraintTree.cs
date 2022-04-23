using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    /// <summary>
    /// A tree which tracks the decisions in a execution path.
    /// </summary>
    public class ConstraintTree
    {
        public ConstraintTree()
        {
            Root = new ConstraintNode(null);
        }

        public ConstraintNode Root { get; }
    }
}
