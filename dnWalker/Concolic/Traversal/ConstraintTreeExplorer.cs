using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class ConstraintTreeExplorer : IActiveStateService
    {
        private readonly ConstraintTree _tree;
        private readonly Stack<ConstraintNode> _frontier;

        private ExplicitActiveState _cur;

        private ConstraintNode _current;

        // private readonly Dictionary<> _conditionCoverage // a more complex structure will be needed

        public ConstraintTreeExplorer() : this(new ConstraintTree())
        { }

        public ConstraintTreeExplorer(ConstraintTree tree)
        {
            _tree = tree ?? throw new ArgumentNullException(nameof(tree));
            _frontier = new Stack<ConstraintNode>();
            _frontier.Push(tree.Root);
        }

        public ConstraintTree Tree => _tree;
        public ConstraintNode Current => _current;

        void IActiveStateService.Attach(ExplicitActiveState cur)
        {
            _cur = cur;
            // should be invoked just before the execution => move current to root
            _current = _tree.Root;
        }
        void IActiveStateService.Detach(ExplicitActiveState cur)
        {
            _cur = null;
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
        public void MakeDecision(int decision, params Expression[] choices)
        {
            Debug.Assert(decision >= 0);
            Debug.Assert(decision < choices.Length);
            //uint offset = _cur.CurrentMethod.ProgramCounter.Offset;
            //MethodDef method = _cur.CurrentMethod.Definition;
            ConstraintNode current = _current;
            // case 1
            // the node is not yet expanded (node.Children.Count == 0)
            // create the decision nodes and select the correct one
            if (!current.IsExpanded)
            {
                current.Expand();
                foreach (ConstraintNode expansionNode in current.Children)
                {
                    _frontier.Push(expansionNode);
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
            // in any case, once we visit the node, we can mark it explored
            _current.MarkExplored();
        }

        /// <summary>
        /// Finds the next constraint to solve in order to advance the exploration.
        /// Null if exploration is finished.
        /// </summary>
        /// <returns></returns>
        public IPrecondition GetNextPrecondition()
        {
            while (_frontier.TryPop(out ConstraintNode node))
            {
                if (!node.IsExplored)
                {
                    return node.GetPrecondition();
                }
            }

            return null;
        }

        public bool TryGetNextPrecondition([NotNullWhen(true)]out IPrecondition precondition)
        {
            precondition = GetNextPrecondition();
            return precondition != null;
        }
    }
}
