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
        private readonly Dictionary<CILLocation, Dictionary<Expression, bool>> _conditionCoverage = new Dictionary<CILLocation, Dictionary<Expression, bool>>();
        private readonly List<ConstraintTree> _trees;

        private readonly Stack<ConstraintNode> _frontier;
        private ConstraintNode _current;

        /// <summary>
        /// Initializes a new constraint tree explorer without any preconditions, e.i. TRUE precondition.
        /// </summary>
        public ConstraintTreeExplorer() : this(new Constraint[] { new Constraint() })
        {
        }

        /// <summary>
        /// Initializes a new constraint tree explorer with a preconditions.
        /// </summary>
        public ConstraintTreeExplorer(IEnumerable<Constraint> preconditions)
        {
            _trees = new List<ConstraintTree>(preconditions.Select(static pc => new ConstraintTree(pc)));
            _frontier = new Stack<ConstraintNode>(_trees.Select(static ct => ct.Root));
        }


        public ConstraintNode Current => _current;

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
            //uint offset = _cur.CurrentMethod.ProgramCounter.Offset;
            //MethodDef method = _cur.CurrentMethod.Definition;
            ConstraintNode current = _current;

            // we are making a decision from the current node => we can mark it covered
            MarkCovered(current);

            // case 1
            // the node is not yet expanded (node.Children.Count == 0)
            // create the decision nodes and select the correct one
            if (!current.IsExpanded)
            {
                CILLocation location = cur.CurrentLocation;

                current.Expand(location, choices);
                for (int i = 0; i < current.Children.Count; ++i)
                {
                    if (i == decision)
                    {
                        // we are exploring this choice right now => no need to add into the frontier
                        continue;
                    }

                    if (CheckExplored(current.Children[i]))
                    {
                        // we have already explored this choice => no need to add into the frontier
                        continue;
                    }

                    _frontier.Push(current.Children[i]);
                }

                _current = current.Children[decision];
            }
            // case 2
            // current node is already expanded
            // => verify whether the expansion is correct??
            // set current node based on the decision
            else
            {
                Debug.Assert(current.Children.Count == choices.Length);
                _current = current.Children[decision];
            }
        }

        /// <summary>
        /// Finds the next constraint to solve in order to advance the exploration.
        /// Null if exploration is finished.
        /// </summary>
        /// <returns></returns>
        public Constraint GetNextPrecondition()
        {
            // this will be invoked by the concolic explorer before a new execution
            // should mark the last constraint node as covered
            if (_current != null) MarkCovered(_current);

            while (_frontier.TryPop(out ConstraintNode node))
            {
                if (!CheckExplored(node))
                {
                    _current = node.Tree.Root;
                    return node.GetPrecondition();
                }
            }

            return null;
        }

        public bool TryGetNextPrecondition([NotNullWhen(true)] out Constraint precondition)
        {
            precondition = GetNextPrecondition();
            return precondition != null;
        }

        private bool CheckExplored(ConstraintNode node)
        {
            if (node.IsExplored) return true;

            // check for the condition coverage
            CILLocation location = node.Location;
            if (location != CILLocation.None)
            {
                if (!_conditionCoverage.TryGetValue(location, out Dictionary<Expression, bool> coverage))
                {
                    coverage = new Dictionary<Expression, bool>();
                    _conditionCoverage[location] = coverage;
                }

                if (coverage.TryGetValue(node.Condition, out bool covered) &&
                    covered)
                {
                    node.MarkExplored();
                    return true;
                }                
            }
            return false;
        }

        private void MarkCovered(ConstraintNode node)
        {
            if (!node.IsExplored)
            {
                node.MarkExplored();

                Expression condition = node.Condition;
                if (condition != null)
                {
                    CILLocation currentLocation = node.Location;
                    Dictionary<Expression, bool> coverage = _conditionCoverage[currentLocation];

                    coverage[condition] = true;
                }
            }
        }
    }
}
