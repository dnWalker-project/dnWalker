using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
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
        public virtual bool ShouldBeExplored(ConstraintNode constraintNode) => true;

        public virtual void OnNodeExplored(ConstraintNode constraintNode) { }
        public virtual void OnUnsatisfiableNodePruned(ConstraintNode constraintNode) { }

        public virtual void NewIteration()
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

    }

    public class AllPathsCoverage : ExplorationStrategyBase
    {
        public override bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            return !constraintNode.IsExplored;
        }

        public override void OnNodeExplored(ConstraintNode constraintNode)
        {
            constraintNode.MarkExplored(CurrentIteration);
        }
    }

    public class AllConditionsCoverage : ExplorationStrategyBase
    {
        private readonly Dictionary<ControlFlowNode, Dictionary<Expression, bool>> _conditionCoverage = new Dictionary<ControlFlowNode, Dictionary<Expression, bool>>();

        public override bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            if (constraintNode.IsExplored) return false;

            // check for the condition coverage
            ControlFlowNode location = constraintNode.Location;
            if (location != null)
            {
                if (!_conditionCoverage.TryGetValue(location, out Dictionary<Expression, bool> coverage))
                {
                    coverage = new Dictionary<Expression, bool>();
                    _conditionCoverage[location] = coverage;
                }

                if (coverage.TryGetValue(constraintNode.Condition, out bool covered))
                {
                    if (covered)
                    {
                        //constraintNode.MarkExplored();
                        return false;
                    }
                }
            }
            return true;
        }

        public override void OnNodeExplored(ConstraintNode constraintNode)
        {

            if (!constraintNode.IsExplored)
            {
                constraintNode.MarkExplored(CurrentIteration);

                Expression condition = constraintNode.Condition;
                if (condition != null)
                {
                    ControlFlowNode currentLocation = constraintNode.Location;
                    Dictionary<Expression, bool> coverage = _conditionCoverage[currentLocation];

                    coverage[condition] = true;
                }
            }
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            _conditionCoverage.Clear();
            base.Initialize(activeState, entryPoint);
        }
    }

    public class AllEdgesCoverage : ExplorationStrategyBase
    {
        private readonly Dictionary<(ControlFlowNode src, ControlFlowNode trg), bool> _coverage = new Dictionary<(ControlFlowNode src, ControlFlowNode trg), bool>();
        private ExplicitActiveState _activeState;

        public override bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            if (constraintNode.IsExplored) return false;

            if (constraintNode.IsRoot) return true;

            (ControlFlowNode src, ControlFlowNode dst) edge = (constraintNode.Parent.Location, constraintNode.Location);
            if (_coverage.TryGetValue(edge, out bool isCovered) && isCovered)
            {
                return false;
            }
            return true;
        }

        public override void OnNodeExplored(ConstraintNode constraintNode)
        {
            constraintNode.MarkExplored(CurrentIteration);

            if (constraintNode.IsRoot) return;

            (ControlFlowNode src, ControlFlowNode dst) edge = (constraintNode.Parent.Location, constraintNode.Location);
            _coverage[edge] = true;
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            _activeState = activeState;

            _coverage.Clear();
            base.Initialize(activeState, entryPoint);
        }
    }

    public class AllNodesCoverage : ExplorationStrategyBase
    {
        private readonly Dictionary<ControlFlowNode, bool> _coverage = new Dictionary<ControlFlowNode, bool>();

        public override bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            //return !constraintNode.IsExplored ||
            //    !constraintNode.Location.IsCovered;

            if (constraintNode == null) return false;

            if (_coverage.TryGetValue(constraintNode.Location, out bool isCovered) && isCovered)
            {
                return false;
            }

            return true;
        }

        public override void OnNodeExplored(ConstraintNode constraintNode)
        {
            constraintNode.MarkExplored(CurrentIteration);

            _coverage[constraintNode.Location] = true;
        }

        public override void Initialize(ExplicitActiveState activeState, MethodDef entryPoint)
        {
            _coverage.Clear();
            base.Initialize(activeState, entryPoint);
        }
    }
}
