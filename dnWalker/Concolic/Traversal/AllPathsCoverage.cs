using dnWalker.Symbolic;

using System.Collections.Generic;

namespace dnWalker.Concolic.Traversal
{
    public class AllPathsCoverage : ExplorationStrategyBase
    {
        private readonly Stack<ConstraintNode> _frontier = new Stack<ConstraintNode>();

        public override bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node, out IModel inputModel)
        {
            while (_frontier.TryPop(out node))
            {
                if (node.IsExplored) continue;

                if (TrySolve(solver, node, out inputModel)) return true;
            }
            inputModel = null;
            return false;
        }

        public override void AddDiscoveredNode(ConstraintNode node)
        {
            _frontier.Push(node);
        }
    }
}
