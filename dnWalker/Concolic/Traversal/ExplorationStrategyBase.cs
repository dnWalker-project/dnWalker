using dnlib.DotNet;

using dnWalker.Configuration;
using dnWalker.Symbolic;

using MMC.State;

using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Concolic.Traversal
{
    public abstract class ExplorationStrategyBase : IExplorationStrategy
    {
        protected virtual void NewIteration()
        {
            _currentIteration++;
        }

        public virtual void Initialize(ExplicitActiveState activeState, MethodDef entryPoint, IConfiguration configuration)
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

        protected bool TrySolve(ISolver solver, ConstraintNode node, [NotNullWhen(true)] out IModel model)
        {
            (ResultType type, model) = solver.Solve(node.GetPrecondition());

            switch (type)
            {
                case ResultType.Satisfiable:
                    return HandleSatisfiableNode(node, model);

                case ResultType.Unsatisfiable:
                    return HandleUnsatisfiableNode(node);

                case ResultType.Undecidable:
                default:
                    return HandleUndecidable(node);
            }
        }

        protected virtual bool HandleSatisfiableNode(ConstraintNode node, IModel model)
        {
            NewIteration();
            node.MarkPreconditionSource(CurrentIteration);
            return true;
        }

        protected virtual bool HandleUndecidable(ConstraintNode node)
        {
            node.MarkUndecidable();
            return false;
        }


        protected virtual bool HandleUnsatisfiableNode(ConstraintNode node)
        {
            node.MarkUnsatisfiable();
            return false;
        }
    }
}
