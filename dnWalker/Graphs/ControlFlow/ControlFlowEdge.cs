using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public class ControlFlowEdge : QuikGraph.Edge<ControlFlowNode>
    {
        public ControlFlowEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {

        }

        private readonly List<Constraint> _reachabilityProofs = new List<Constraint>();
        private readonly List<Constraint> _unreachabilityProofs = new List<Constraint>();

        public void MarkCovered(Constraint proof)
        {
            _reachabilityProofs.Add(proof);
        }

        public void MarkUnreachable(Constraint proof)
        {
            _unreachabilityProofs.Add(proof);
        }

        public bool IsCovered => _reachabilityProofs.Count > 0;
        public bool IsReachable => _reachabilityProofs.Count > 0 || _unreachabilityProofs.Count == 0;
    }

    public class ExceptionEdge : ControlFlowEdge
    {
        private readonly TypeDef _exceptionType;

        public ExceptionEdge(TypeDef exceptionType, [NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {
            _exceptionType = exceptionType;
        }

        public TypeDef ExceptionType => _exceptionType;
    }
}
