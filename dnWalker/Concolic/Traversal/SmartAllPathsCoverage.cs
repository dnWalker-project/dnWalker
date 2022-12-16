using dnlib.DotNet;

using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;

using MMC;
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

    public class SmartAllPathsCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();
        private readonly Stack<ConstraintNode> _delayed  = new Stack<ConstraintNode>();

        private readonly Dictionary<ControlFlowEdge, int> _counters = new Dictionary<ControlFlowEdge, int>();
        private readonly List<IConstraintFilter> _filters = new List<IConstraintFilter>();
        private int _maxCounter = 1;

        private Logger _logger;

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint, IConfiguration configuration)
        {
            base.Initialize(activeState, entryPoint, configuration);

            _logger = activeState.Logger;
            _maxCounter = 1;

            NewEdgeDiscoverer newEdgeDiscoverer = new NewEdgeDiscoverer(configuration.MaxIterationsWithoutNewEdge());
            _filters.Add(newEdgeDiscoverer);
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

        protected override bool HandleUnsatisfiableNode(ConstraintNode node)
        {
            Debug.WriteLine($"[STRATEGY|{CurrentIteration}] unsat node: {node}");
            return base.HandleUnsatisfiableNode(node);
        }

        protected override bool HandleUndecidable(ConstraintNode node)
        {
            Debug.WriteLine($"[STRATEGY|{CurrentIteration}] dont know node: {node}");
            return base.HandleUndecidable(node);
        }

        protected override bool HandleSatisfiableNode(ConstraintNode node, IModel model)
        {
            Debug.WriteLine($"[STRATEGY|{CurrentIteration}] sat node: {node}");
            return base.HandleSatisfiableNode(node, model);
        }


        private bool CheckSAT(ISolver solver, ConstraintNode node, [NotNullWhen(true)] out IModel inputModel)
        {
            return TrySolve(solver, node, out inputModel);
        }


        public override bool TryGetNextSatisfiableNode(ISolver solver, [NotNullWhen(true)] out ConstraintNode node, [NotNullWhen(true)]out IModel inputModel)
        {
            // check the frontier first
            while (_frontier.TryPop(out node))
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

        protected override void NewIteration()
        {
            base.NewIteration();
            foreach (IConstraintFilter filter in _filters)
            {
                filter.StartIteration(CurrentIteration);
            }
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            foreach (IConstraintFilter filter in _filters)
            {
                if (!filter.UseConstraint(node))
                {
                    Debug.WriteLine($"[STRATEGY|{CurrentIteration}] node {node}:{node.Edge} denied by {filter}");
                    return;
                }
            }

            Debug.WriteLine($"[STRATEGY|{CurrentIteration}] new node: {node}");
            _frontier.Push(node);
        }
    }
}
