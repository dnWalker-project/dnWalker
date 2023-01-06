using dnWalker.Graphs.ControlFlow;

using System.Collections.Generic;

namespace dnWalker.Concolic.Traversal
{
    public class NewEdgeDiscoverer : IConstraintFilter
    {
        private readonly int _maxValue;

        public NewEdgeDiscoverer(int maxValue)
        {
            _maxValue = maxValue;
        }

        private int _counter = 0;
        private HashSet<ControlFlowEdge> _knownEdges = new HashSet<ControlFlowEdge>();

        private bool _newEdgeFound = false;

        public bool UseConstraint(ConstraintNode node)
        {
            ControlFlowEdge edge = node.Edge;

            if (edge == null)
            {
                return true;
            }

            if (_knownEdges.Add(edge))
            {
                // a new previously uknown edge was found => reset the counter
                _newEdgeFound = true;
                _counter = 0;
            }

            return _counter < _maxValue;
        }

        public void StartIteration(int i)
        {
            if (!_newEdgeFound)
            {
                _counter++;
            }
            _newEdgeFound = false;
        }
    }
}
