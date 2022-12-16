using dnlib.DotNet;

using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;

using MMC.State;

using System.Collections.Generic;

namespace dnWalker.Concolic.Traversal
{
    public class AllEdgesCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();
        private readonly Dictionary<ControlFlowEdge, bool> _coverage = new Dictionary<ControlFlowEdge, bool>();

        public override void AddExploredNode(ConstraintNode node)
        {
            base.AddExploredNode(node);
            SetEdgeCovered(node);
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint, IConfiguration configuration)
        {
            _coverage.Clear();
            base.Initialize(activeState, entryPoint, configuration);
        }

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            while (_frontier.TryPop(out node))
            {
                if (node.IsExplored)
                {
                    continue;
                }

                if (GetEdgeCovered(node))
                {
                    continue;
                }

                if (TrySolve(solver, node, out inputModel))
                {
                    return true;
                }
            }
            inputModel = null;
            return false;
        }

        private void SetEdgeCovered(ConstraintNode node)
        {

            if (node.Edge == null)
            {
                return;
            }

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            _coverage[node.Edge] = true;
        }

        private bool GetEdgeCovered(ConstraintNode node)
        {
            if (node.Edge == null)
            {
                // by default the root node is not covered this way, as it would disqualify it from the exploration
                return false;
            }

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            return _coverage.TryGetValue(node.Edge, out bool covered) && covered;
        }
    }
}
