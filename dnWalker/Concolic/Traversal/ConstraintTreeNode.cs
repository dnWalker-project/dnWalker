using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    /// <summary>
    /// A node within the constraint tree.
    /// </summary>
    public class ConstraintTreeNode
    {
        private readonly IModel _inputModel;
        private readonly ConstraintTreeNode _parent;
        private IReadOnlyList<ConstraintTreeNode> _children = Array.Empty<ConstraintTreeNode>();

        private List<Expression> _ancestorConstraints = null;

        public ConstraintTreeNode(ConstraintTreeNode parent, IModel inputModel)
        {
            _parent = parent;
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }
        public ConstraintTreeNode(ConstraintTreeNode parent) : this(parent, new Model())
        {
        }

        /// <summary>
        /// Gets the constraint associated with this node.
        /// If null, the node is unconstrained, (the expression is TRUE)
        /// </summary>
        public virtual Expression Constraint => null;
        
        /// <summary>
        /// Gets the least specified input model needed to reach this constraint node.
        /// </summary>
        public IModel InputModel => _inputModel;

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public ConstraintTreeNode Parent => _parent;
        public bool IsRoot => _parent == null;

        /// <summary>
        /// Gets the precondition associated with this node
        /// </summary>
        /// <returns></returns>
        public IPrecondition GetPrecondition()
        {
            if (_ancestorConstraints == null)
            {
                List<Expression> _ancestorConstraints = new List<Expression>();
                ConstraintTreeNode current = this;
                while (current != null)
                {
                    Expression expression = current.Constraint;
                    if (expression != null) _ancestorConstraints.Add(expression);
                    current = current.Parent;
                }
            }

            return new Precondition(_inputModel, _ancestorConstraints);
        }
    }
}
