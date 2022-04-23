using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class DecisionNode : ConstraintNode
    {
        // TODO: add the location within the code (i.e. method + offset)

        public DecisionNode(ConstraintNode parent, IModel inputModel, Expression constraint) : base(parent, inputModel)
        {
            _constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }

        private readonly Expression _constraint;

        public override Expression Constraint => _constraint;
    }
}
