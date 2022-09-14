using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
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

        private ConstraintNode _current;

        /// <summary>
        /// Initializes a new constraint tree explorer with a preconditions.
        /// </summary>
        public ConstraintTreeExplorer(IExplorationStrategy strategy, IEnumerable<ConstraintTree> trees)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));

            _trees = new List<ConstraintTree>(trees);

            foreach (ConstraintTree ct in _trees)
            {
                strategy.AddDiscoveredNode(ct.Root);
            }
        }


        public ConstraintNode Current
        {
            //get
            //{
            //    return _current;
            //}
            get
            {
                if (_current == null)
                {
                    _current = _trees[0].Root;
                }
                return _current;
            }
        }

        public IReadOnlyList<ConstraintTree> Trees
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
        public void MakeDecision(ExplicitActiveState cur, int decision, ControlFlowEdge[] choicePaths, Expression[] choiceExpressions)
        {
            if (_current == null) _current = _trees[0].Root;

            Debug.Assert(decision >= 0 && decision < choicePaths.Length, "Decision must be greater than or equal to 0 and less than choiceTargets.Length.");
            Debug.Assert(choicePaths.Length == choiceExpressions.Length, "choiceTargets must have same length as choiceExpressions.");

            // we are making a decision from the current node => we can mark it covered
            _strategy.AddExploredNode(_current);


            // case 1
            // the node is not yet expanded (node.Children.Count == 0)
            // create the decision nodes and select the correct one
            if (!_current.IsExpanded)
            {
                Constraint[] choiceConstraints = new Constraint[choiceExpressions.Length];
                for (int i = 0; i < choiceExpressions.Length; ++i)
                {
                    choiceConstraints[i] = new Constraint();
                    choiceConstraints[i].AddExpressionConstraint(choiceExpressions[i]);
                }

                _current.Expand(choicePaths, choiceConstraints);
                for (int i = 0; i < _current.Children.Count; ++i)
                {
                    if (i != decision)
                    {
                        _strategy.AddDiscoveredNode(_current.Children[i]);
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
                Debug.Assert(_current.Children.Count == choicePaths.Length);
                _current = _current.Children[decision];
            }
        }

        public bool TryGetNextInputModel(ISolver solver, [NotNullWhen(true)] out IModel inputModel)
        {
            // we have finished exploration of the last node
            if (_current != null)
            {
                _strategy.AddExploredNode(_current);
            }

            if (_strategy.TryGetNextSatisfiableNode(solver, out ConstraintNode node, out inputModel))
            {
                // a new iteration will be run => reset

                _current = node.Tree.Root;

                return true;
            }
            return false;
        }
    }
}
