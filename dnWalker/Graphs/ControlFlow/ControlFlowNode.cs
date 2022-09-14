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

        private readonly List<ControlFlowEdge> _inEdges = new List<ControlFlowEdge>();
        private readonly List<ControlFlowEdge> _outEdges = new List<ControlFlowEdge>();

        public IReadOnlyCollection<ControlFlowEdge> InEdges => _inEdges;
        public IReadOnlyCollection<ControlFlowEdge> OutEdges => _outEdges;

        public IEnumerable<ControlFlowNode> Predecessors => _inEdges.Select(e => e.Source);
        public IEnumerable<ControlFlowNode> Successors => _outEdges.Select(e => e.Target);

        public IEnumerable<ControlFlowEdge> GetEdgesFrom(ControlFlowNode predecessor)
        {
            return _inEdges.Where(e => e.Source == predecessor);
        }

        public IEnumerable<ControlFlowEdge> GetEdgesTo(ControlFlowNode successor)
        {
            // WIP: this is very bad, but some method depend on a single edge between two nodes.
            return _outEdges.Where(e => e.Target == successor);
        }

        public ControlFlowEdge GetEdgeTo(ControlFlowNode successor)
        {
            // WIP: this is very bad, but some method depend on a single edge between two nodes.
            return _outEdges.Where(e => e.Target == successor).Single();
        }

        internal void AddInEdge(ControlFlowEdge edge)
        {
            _inEdges.Add(edge);
        }

        internal void AddOutEdge(ControlFlowEdge edge)
        {
            _outEdges.Add(edge);
        }
    }
}
