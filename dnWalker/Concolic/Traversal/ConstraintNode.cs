using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Util;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ConstraintNode _parent;
        private readonly Expression _condition;
        private readonly CILLocation _location;
        private readonly ConstraintTree _tree;
        
        private IReadOnlyList<ConstraintNode> _children = Array.Empty<ConstraintNode>();

        private bool _explored;
        private bool _unsatisfiable;

        protected ConstraintNode(ConstraintTree tree, ConstraintNode parent)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _parent = parent;
            _location = CILLocation.None;
            _condition = null;
        }
        public ConstraintNode(ConstraintTree tree, ConstraintNode parent, CILLocation location, Expression condition)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _parent = parent;
            _location = location;
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        /// <summary>
        /// Gets the constraint associated with this node.
        /// If null, the node is unconstrained, (the expression is TRUE)
        /// </summary>
        public virtual Constraint GetPrecondition()
        {
            ConstraintNode cn = this;
            List<Expression> conditions = new List<Expression>();
            while (!cn.IsRoot)
            {
                Expression conditionExpression = cn._condition;
                if (conditionExpression != null)
                {
                    conditions.Add(conditionExpression);
                }
                cn = cn.Parent;
            }

            Debug.Assert(!ReferenceEquals(this, cn));

            // cn is the root node, should be PreconditionNode
            Constraint precondition = cn.GetPrecondition().Clone();
            foreach (Expression condition in conditions)
            {
                precondition.AddExpressionConstraint(condition);
            }

            return precondition;
        }
        
        public Expression Condition
        {
            get { return _condition; }
        }

        public override string ToString()
        {
            return _condition?.ToString() ?? "True";
        }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public ConstraintNode Parent => _parent;
        public bool IsRoot => _parent == null;

        public IReadOnlyList<ConstraintNode> Children => _children;

        public bool IsExpanded => _children != null && _children.Count > 0;

        public bool IsExplored => _explored;
        public bool IsSatisfiable => !_unsatisfiable;

        public CILLocation Location => _location;

        public ConstraintTree Tree => _tree;

        public void MarkExplored() => _explored = true;
        public void MarkUnsatisfiable() => _unsatisfiable = true;

        public void Expand(CILLocation location, params Expression[] choices)
        {
            ConstraintNode[] children = new ConstraintNode[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                ConstraintNode child = new ConstraintNode(_tree, this, location, choices[i]);
                children[i] = child;
            }
            _children = children;
        }
    }
}
