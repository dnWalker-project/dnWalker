using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public abstract class ExplorationStrategyBase : IExplorationStrategy
    {
        protected virtual bool NewIteration()
        {
            _currentIteration++;
            return true;
        }

        public virtual void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            _currentIteration = 0;
            _activeState = activeState;
            _entryPoint = entryPoint;
        }

        private int _currentIteration = 0;
        private ExplicitActiveState _activeState;
        private MethodDef _entryPoint;

        public int CurrentIteration => _currentIteration;
        public ExplicitActiveState ActiveState => _activeState;
        public MethodDef EntryPoint => _entryPoint;

        //public abstract bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel);

        public abstract void AddDiscoveredNode(ConstraintNode node);
        public virtual void AddExploredNode(ConstraintNode node) => node.MarkExplored(_currentIteration);

        public virtual IterationInfo NextIteration(ISolver solver)
        {
            List<ConstraintNode> unsatNodes = new List<ConstraintNode>();
            List<ConstraintNode> skippedNodes = new List<ConstraintNode>();
            IModel inputModel = null;

            ConstraintNode selectedNode = null;

            if (!NewIteration()) return IterationInfo.InvalidEmpty;

            while (TryGetNextNode(out ConstraintNode node))
            {
                if (SkipNode(node))
                {
                    skippedNodes.Add(node);
                }
                else if (CheckSat(solver, node, out inputModel))
                {
                    selectedNode = node;
                    break;
                }
                else
                {
                    unsatNodes.Add(node);
                }
            }

            IterationInfo info = new IterationInfo(selectedNode, inputModel, unsatNodes, skippedNodes);
            return info;
        }

        protected abstract bool TryGetNextNode([NotNullWhen(true)] out ConstraintNode node);

        protected abstract bool SkipNode(ConstraintNode node);

        protected virtual bool CheckSat(ISolver solver, ConstraintNode node, [NotNullWhen(true)] out IModel model)
        {
            model = solver.Solve(node.GetPrecondition());
            return model != null;
        }
    }

    public class FirstNPaths : AllPathsCoverage
    {
        private readonly int _maxPaths;

        public FirstNPaths(int maxPaths)
        {
            _maxPaths = maxPaths;
        }

        protected override bool NewIteration()
        {
            return base.NewIteration() && CurrentIteration < _maxPaths;
        }
    }

    public class AllPathsCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();

        protected override bool SkipNode(ConstraintNode node)
        {
            return node.IsExplored;
        }

        protected override bool TryGetNextNode([NotNullWhen(true)] out ConstraintNode node)
        {
            return _frontier.TryPop(out node);
        }
        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }
    }

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

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            _coverage.Clear();
            base.Initialize(activeState, entryPoint);
        }

        protected override bool SkipNode(ConstraintNode node)
        {
            return node.IsExplored || GetEdgeCovered(node);
        }

        protected override bool TryGetNextNode([NotNullWhen(true)] out ConstraintNode node)
        {
            return _frontier.TryPop(out node);
        }

        private void SetEdgeCovered(ConstraintNode node)
        {

            if (node.IsRoot) return;

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            _coverage[node.Edge] = true;
        }

        private bool GetEdgeCovered(ConstraintNode node)
        {
            if (node.IsRoot)
            {
                // by default the root node is not covered this way, as it would disqualify it from the exploration
                return false;
            }

            // cfg nodes may be from different methods or the edge may not exist (for example with unconditional branching...)
            return _coverage.TryGetValue(node.Edge, out bool covered) && covered;
        }
    }

    public class AllNodesCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();
        private readonly Dictionary<ControlFlowNode, bool> _coverage = new Dictionary<ControlFlowNode, bool>();

        public override void AddExploredNode(ConstraintNode node)
        {
            base.AddExploredNode(node);
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            base.Initialize(activeState, entryPoint);
        }

        private void SetNodesCovered(ConstraintNode node)
        {
            _coverage[node.Edge.Source] = true;
            _coverage[node.Edge.Target] = true;
        }

        private bool GetTargetCovered(ConstraintNode node)
        {
            return _coverage.TryGetValue(node.Edge.Target, out bool isCovered) && isCovered;
        }

        protected override bool SkipNode(ConstraintNode node)
        {
            return node.IsExplored || GetTargetCovered(node);
        }

        protected override bool TryGetNextNode([NotNullWhen(true)] out ConstraintNode node)
        {
            return _frontier.TryPop(out node);
        }
    }
}
