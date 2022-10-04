using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public abstract class ControlFlowNode : ControlFlowElement
    {
        private readonly List<ControlFlowEdge> _inEdges = new List<ControlFlowEdge>();
        private readonly List<ControlFlowEdge> _outEdges = new List<ControlFlowEdge>();

        protected ControlFlowNode(MethodDef method) : base(method)
        {
        }

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
            return _outEdges.Where(e => e.Target == successor);
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
