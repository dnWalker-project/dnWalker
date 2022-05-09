using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;
using MMC.Util;

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
    /// Systematically explores constraint trees built from predicates.
    /// </summary>
    public class ConstraintTreeExplorer
    {
        private readonly IExplorationStrategy _strategy;

        private readonly List<ConstraintTree> _trees;

        private readonly Stack<ConstraintNode> _frontier;
        private ConstraintNode _current;

        /// <summary>
        /// Initializes a new constraint tree explorer without any preconditions, e.i. TRUE precondition.
        /// </summary>
        public ConstraintTreeExplorer(IExplorationStrategy strategy) : this(strategy, new Constraint[] { new Constraint() })
        {
        }

        /// <summary>
        /// Initializes a new constraint tree explorer with a preconditions.
        /// </summary>
        public ConstraintTreeExplorer(IExplorationStrategy strategy, IEnumerable<Constraint> preconditions)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));

            _trees = new List<ConstraintTree>(preconditions.Select(static pc => new ConstraintTree(pc)));
            _frontier = new Stack<ConstraintNode>(_trees.Select(static ct => ct.Root));
        }


        public ConstraintNode Current => _current;

        public IList<ConstraintTree> Trees
        {
            get { return _trees; }
        }

        /// <summary>
        /// Invoked by an instruction executor which enforces some constraint.
        /// </summary>
        /// <param name="decision">Specifies which choice is selected based on the concrete inputs</param>
        /// <param name="choices">Specifies all possible symbolic outcomes.</param>
        /// <example>
        /// BRTRUE:
        /// - the concrete value is TRUE
        /// - the symbolic value is (x \gt 0)
        /// - choices  := { (x \leq 0), (x \gt 0) }
        /// - decision := 1
        /// The order is important!!! The first choice represents decision which will result in FALLTHROUGH => no flow change
        ///
        /// More complex instructions, for example LDELEM will provide more choices:
        /// - choices :=
        ///     {
        ///         (arr != null && len(arr) \gt idx)   // "normal" execution, no errors... => retValue := NextRetVal
        ///         (arr == null),                      // null reference exc
        ///         (arr != null && idx \lt 0),          // index out of range exc, idx < 0
        ///         (arr != null && len(arr) \leq idx),  // index out of range exc, idx >= arr.Length
        ///     }
        /// </example>
        public void MakeDecision(ExplicitActiveState cur, int decision, params Expression[] choices)
        {
            Debug.Assert(decision >= 0);
            Debug.Assert(decision < choices.Length);

            // we are making a decision from the current node => we can mark it covered
            OnNodeExplored(_current);

            // case 1
            // the node is not yet expanded (node.Children.Count == 0)
            // create the decision nodes and select the correct one
            if (!_current.IsExpanded)
            {
                CILLocation location = cur.CurrentLocation;

                _current.Expand(location, choices);
                for (int i = 0; i < _current.Children.Count; ++i)
                {
                    if (i == decision)
                    {
                        // we are exploring this choice right now => no need to add into the frontier
                        continue;
                    }

                    if (ShouldBeExplored(_current.Children[i]))
                    {
                        _frontier.Push(_current.Children[i]);
                    }
                }

                _current = _current.Children[decision];
            }
            // case 2
            // current node is already expanded
            // => verify whether the expansion is correct??
            // set current node based on the decision
            else
            {
                Debug.Assert(_current.Children.Count == choices.Length);
                _current = _current.Children[decision];
            }
        }

        /// <summary>
        /// Finds the next constraint to solve in order to advance the exploration.
        /// Null if exploration is finished.
        /// </summary>
        /// <returns></returns>
        public ConstraintNode GetNextConstraintNode()
        {
            // this will be invoked by the concolic explorer before a new execution
            // should mark the last constraint node as covered
            if (_current != null) OnNodeExplored(_current);

            while (_frontier.TryPop(out ConstraintNode node))
            {
                if (ShouldBeExplored(node))
                {
                    _current = node.Tree.Root;
                    return node;
                }
            }

            return null;
        }

        public bool TryGetNextConstraintNode([NotNullWhen(true)] out ConstraintNode constraintNode)
        {
            constraintNode = GetNextConstraintNode();
            return constraintNode != null;
        }

        private bool ShouldBeExplored(ConstraintNode node)
        {
            return _strategy.ShouldBeExplored(node);
        }

        private void OnNodeExplored(ConstraintNode node)
        {
            _strategy.OnNodeExplored(node);
        }
    }
}
