using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public abstract class ControlFlowNode
    {
        private bool _isCovered;

        public void MarkCovered()
        {
            _isCovered = true;
        }

        public bool IsCovered => _isCovered;

        private readonly Dictionary<ControlFlowNode, ControlFlowEdge> _predecessors = new Dictionary<ControlFlowNode, ControlFlowEdge>();
        private readonly Dictionary<ControlFlowNode, ControlFlowEdge> _successors = new Dictionary<ControlFlowNode, ControlFlowEdge>();

        public IReadOnlyCollection<ControlFlowEdge> InEdges => _predecessors.Values;
        public IReadOnlyCollection<ControlFlowEdge> OutEdges => _successors.Values;

        public IReadOnlyCollection<ControlFlowNode> Predecessors => _predecessors.Keys;
        public IReadOnlyCollection<ControlFlowNode> Successors => _successors.Keys;

        public ControlFlowEdge GetEdgeFrom(ControlFlowNode predecessor)
        {
            return _predecessors[predecessor];
        }

        public ControlFlowEdge GetEdgeTo(ControlFlowNode successor)
        {
            return _successors[successor];
        }

        public void AddEdgeFrom(ControlFlowNode predecessor, ControlFlowEdge edge)
        {
            _predecessors.Add(predecessor, edge);
        }
        public void AddEdgeTo(ControlFlowNode successor, ControlFlowEdge edge)
        {
            _successors.Add(successor, edge);
        }
    }
}
