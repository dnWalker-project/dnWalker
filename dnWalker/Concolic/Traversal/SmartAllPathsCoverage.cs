using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class SmartAllPathsCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();
        private readonly Stack<ConstraintNode> _delayed  = new Stack<ConstraintNode>();

        private readonly Dictionary<ControlFlowEdge, int> _counters = new Dictionary<ControlFlowEdge, int>();
        private int _maxCounter = 1;

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            base.Initialize(activeState, entryPoint);

            _maxCounter = 1;
        }

        private int GetCounter(ControlFlowEdge edge)
        {
            if (edge == null) return 0;

            if (!_counters.TryGetValue(edge, out int counter))
            {
                counter = 0;
                _counters.Add(edge, counter);
            }
            return counter;
        }

        private int IncreaseCounter(ControlFlowEdge edge)
        {
            if (edge == null) return 0;

            if (!_counters.ContainsKey(edge))
            {
                _counters.Add(edge, 1);
                return 1;
            }
            else
            {
                return ++_counters[edge];
            }
        }

        private bool CheckSAT(ISolver solver, ConstraintNode node, [NotNullWhen(true)] out IModel inputModel)
        {
            inputModel = solver.Solve(node.GetPrecondition());
            return inputModel != null;
        }

        public override bool TryGetNextSatisfiableNode(ISolver solver, [NotNullWhen(true)] out ConstraintNode node, [NotNullWhen(true)]out IModel inputModel)
        {
            // check the frontier first
            while (_frontier.TryPeek(out node))
            {
                int counter = GetCounter(node.Edge);
                if (counter >= _maxCounter)
                {
                    _delayed.Push(node);
                    continue;
                }

                if (CheckSAT(solver, node, out inputModel))
                {
                    IncreaseCounter(node.Edge);
                    return true;
                }
            }

            while (_delayed.TryPop(out node))
            {
                if (CheckSAT(solver, node, out inputModel))
                {
                    _maxCounter = IncreaseCounter(node.Edge);
                    return true;
                }
            }

            node = null;
            inputModel = null;
            return false;
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }
    }
}
