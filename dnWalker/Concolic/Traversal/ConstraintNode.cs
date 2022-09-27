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
        private readonly Constraint _constraint;
        private readonly ControlFlowNode _location;
        private readonly ConstraintTree _tree;
        private readonly List<int> _iterations = new List<int>();
        private IReadOnlyList<ConstraintNode> _children = Array.Empty<ConstraintNode>();

        private Constraint? _precondition = null;

        private bool _unsatisfiable;
        private int _sourceNodeIteration;

        public ConstraintNode(ConstraintTree tree, ConstraintNode parent, ControlFlowNode location, Constraint constraint)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _parent = parent;
            _location = location;
            _constraint = constraint;
        }

        /// <summary>
        /// Gets the constraint associated with this node.
        /// If null, the node is unconstrained, (the expression is TRUE)
        /// </summary>
        public virtual Constraint GetPrecondition()
        {
            if (_precondition == null)
            {

                List<Constraint> constraints = new List<Constraint>();

                ConstraintNode cn = this;
                while (cn != null)
                {
                    constraints.Add(cn._constraint);
                    cn = cn.Parent;
                }

                _precondition = Constraint.Merge(constraints);
            }
            return _precondition;
        }
        
        public Constraint Constraint
        {
            get { return _constraint; }
        }

        public override string ToString()
        {
            return _constraint?.ToString() ?? "True";
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

        public IReadOnlyList<ConstraintNode> Expand(ControlFlowEdge[] paths, Constraint[] choices)
        {
            ConstraintNode[] children = new ConstraintNode[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                ConstraintNode child = new ConstraintNode(_tree, this, paths[i].Target, choices[i]);
                children[i] = child;
            }
            _children = children;
            return children;
        }

        public void AddConstraint(Expression constraintExpression)
        {
            if (IsExplored)
            {
                // already explored => do not change, the constraint should already be there...
                return;
            }
            Constraint.AddExpressionConstraint(constraintExpression);
        }

        public void AddConstraints(IEnumerable<Expression> constraintExpressions)
        {
            if (IsExplored)
            {
                // already explored => do not change, the constraint should already be there...
                return;
            }
            Constraint.AddExpressionConstraints(constraintExpressions);
        }
    }
}
