using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ConstraintNode _root;

        public ConstraintTree(Constraint precondition)
        {
            _root = new PreconditionNode(this, precondition);
        }

        public ConstraintNode Root => _root;

    }
}
