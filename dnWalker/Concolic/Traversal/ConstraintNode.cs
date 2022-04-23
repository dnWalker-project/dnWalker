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
    public class ConstraintNode
    {
        private readonly IModel _inputModel;
        private readonly ConstraintNode _parent;
        private IReadOnlyList<ConstraintNode> _children = Array.Empty<ConstraintNode>();

        private List<Expression> _ancestorConstraints = null;
        private bool _explored;

        public ConstraintNode(ConstraintNode parent, IModel inputModel)
        {
            _parent = parent;
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }
        public ConstraintNode(ConstraintNode parent) : this(parent, new Model())
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
        public ConstraintNode Parent => _parent;
        public bool IsRoot => _parent == null;

        public IReadOnlyList<ConstraintNode> Children => _children;

        public bool IsExpanded => _children != null && _children.Count > 0;

        public bool IsExplored => _explored;

        public void MarkExplored() => _explored = true;

        public void Expand(params Expression[] choices)
        {
            DecisionNode[] children = new DecisionNode[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                DecisionNode child = new DecisionNode(this, _inputModel, choices[i]);
                children[i] = child;
            }
            _children = children;
        }

        /// <summary>
        /// Gets the precondition associated with this node
        /// </summary>
        /// <returns></returns>
        public IPrecondition GetPrecondition()
        {
            if (_ancestorConstraints == null)
            {
                List<Expression> _ancestorConstraints = new List<Expression>();
                ConstraintNode current = this;
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
