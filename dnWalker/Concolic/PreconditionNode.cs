using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class PreconditionNode : ConstraintNode
    {
        private Constraint _precondition;

        public PreconditionNode(ConstraintTree tree) : this(tree, new Constraint())
        {
        }

        public PreconditionNode(ConstraintTree tree, Constraint precondtion) : base(tree, null)
        {
            _precondition = precondtion;
        }

        public override Constraint GetPrecondition()
        {
            return _precondition;
        }
    }
}
