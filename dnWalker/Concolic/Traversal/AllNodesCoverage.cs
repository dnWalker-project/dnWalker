using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{

    public class AllNodesCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();
        private readonly Dictionary<ControlFlowNode, bool> _coverage = new Dictionary<ControlFlowNode, bool>();

        public override void AddExploredNode(ConstraintNode node)
        {
            base.AddExploredNode(node);
            SetNodeCovered(node);
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint, IConfiguration configuration)
        {
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

                if (GetNodeCovered(node))
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

        private void SetNodeCovered(ConstraintNode node)
        {

            if (node.Edge == null)
            {
                return;
            }

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            _coverage[node.Edge.Target] = true;
        }

        private bool GetNodeCovered(ConstraintNode node)
        {
            if (node.Edge == null)
            {
                // by default the root node is not covered this way, as it would disqualify it from the exploration
                return false;
            }

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            return _coverage.TryGetValue(node.Edge.Target, out bool covered) && covered;
        }
    }
}
