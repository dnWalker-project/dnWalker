﻿using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
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
    public abstract class ExplorationStrategyBase : IExplorationStrategy
    {
        protected virtual void NewIteration()
        {
            _currentIteration++;
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

        public abstract bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel);

        public abstract void AddDiscoveredNode(ConstraintNode node);
        public virtual void AddExploredNode(ConstraintNode node) => node.MarkExplored(_currentIteration);

    }

    public class FirstNPaths : AllPathsCoverage
    {
        private readonly int _maxPaths;

        public FirstNPaths(int maxPaths)
        {
            _maxPaths = maxPaths;
        }

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            node = null;
            inputModel = null;
            return CurrentIteration < _maxPaths && base.TryGetNextSatisfiableNode(solver, out node, out inputModel);
        }
    }

    public class AllPathsCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            while (_frontier.TryPop(out node))
            {
                if (node.IsExplored) continue;

                inputModel = solver.Solve(node.GetPrecondition());
                if (inputModel != null)
                {
                    NewIteration();
                    node.MarkPreconditionSource(CurrentIteration);
                    return true;
                }
                else
                {
                    // UNSAT
                    node.MarkUnsatisfiable();
                }
            }
            inputModel = null;
            return false;
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

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            while (_frontier.TryPop(out node))
            {
                if (node.IsExplored) continue;

                if (GetEdgeCovered(node)) continue;

                inputModel = solver.Solve(node.GetPrecondition());
                if (inputModel != null)
                {
                    NewIteration();
                    node.MarkPreconditionSource(CurrentIteration);
                    return true;
                }
                else
                {
                    // UNSAT
                    node.MarkUnsatisfiable();
                }
            }
            inputModel = null;
            return false;
        }

        private void SetEdgeCovered(ConstraintNode node)
        {

            if (node.Edge == null) return;

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

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            base.Initialize(activeState, entryPoint);
        }

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            while (_frontier.TryPop(out node))
            {
                if (node.IsExplored) continue;

                if (GetNodeCovered(node)) continue;

                inputModel = solver.Solve(node.GetPrecondition());
                if (inputModel != null)
                {
                    NewIteration();
                    node.MarkPreconditionSource(CurrentIteration);
                    return true;
                }
                else
                {
                    // UNSAT
                    node.MarkUnsatisfiable();
                }
            }
            inputModel = null;
            return false;
        }

        private void SetNodeCovered(ConstraintNode node)
        {

            if (node.Edge == null) return;

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
