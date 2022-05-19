using dnWalker.Graphs.ControlFlow;
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

        public PreconditionNode(ConstraintTree tree, ControlFlowNode location) : this(tree, new Constraint(), location)
        {
        }

        public PreconditionNode(ConstraintTree tree, Constraint precondtion, ControlFlowNode location) : base(tree, null, location, null)
        {
            _precondition = precondtion;
        }

        public override Constraint GetPrecondition()
        {
            return _precondition;
        }
    }
}
