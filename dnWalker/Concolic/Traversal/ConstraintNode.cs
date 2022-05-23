using dnWalker.Graphs.ControlFlow;
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
        private readonly ControlFlowNode _location;
        private readonly ConstraintTree _tree;
        private readonly List<int> _iterations = new List<int>();
        private IReadOnlyList<ConstraintNode> _children = Array.Empty<ConstraintNode>();

        private bool _unsatisfiable;
        private int _sourceNodeIteration;

        public ConstraintNode(ConstraintTree tree, ConstraintNode parent, ControlFlowNode location, Expression condition)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _parent = parent;
            _location = location;
            _condition = condition;
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

        public bool IsExplored => _iterations.Count > 0;
        public bool IsSatisfiable => !_unsatisfiable;
        public bool IsPreconditionSource => _sourceNodeIteration > 0;

        public ControlFlowNode Location => _location;

        public ConstraintTree Tree => _tree;

        public void MarkExplored(int iteration)
        {
            _iterations.Add(iteration);
        }
        public IReadOnlyList<int> Iterations => _iterations;

        public void MarkUnsatisfiable()
        {
            _unsatisfiable = true;
            // at this time, children should be empty, but just in case...
            foreach (ConstraintNode child in Children)
            {
                child.MarkUnsatisfiable();
            }
        }

        public void MarkPreconditionSource(int iteration) => _sourceNodeIteration = iteration;

        public IReadOnlyList<ConstraintNode> Expand(ControlFlowNode[] locations, Expression[] choices)
        {
            ConstraintNode[] children = new ConstraintNode[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                ConstraintNode child = new ConstraintNode(_tree, this, locations[i], choices[i]);
                children[i] = child;
            }
            _children = children;
            return children;
        }
    }
}
