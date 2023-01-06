using dnWalker.Symbolic;

namespace dnWalker.Concolic.Traversal
{
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
}
