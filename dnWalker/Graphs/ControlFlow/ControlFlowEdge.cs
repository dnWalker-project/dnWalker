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
    public abstract class ControlFlowEdge
    {
        private readonly ControlFlowNode _source;
        private readonly ControlFlowNode _target;

        protected ControlFlowEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public ControlFlowNode Source => _source;
        public ControlFlowNode Target => _target;

        public override string ToString()
        {
            return $"{{{Source}}}->{{{Target}}}";
        }
    }

    public class NextEdge : ControlFlowEdge
    {
        public NextEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {
        }

        public override string ToString()
        {
            return $"{{{Source}}}->{{NEXT}}";
        }
    }

    public class JumpEdge : ControlFlowEdge
    {
        public JumpEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {
        }
    }

    public class ExceptionEdge : ControlFlowEdge
    {
        private readonly TypeDef _exceptionType;

        public ExceptionEdge(TypeDef exceptionType, [NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {
            _exceptionType = exceptionType;
        }

        public TypeDef ExceptionType => _exceptionType;

        public override string ToString()
        {
            return $"{{{Source}}}- [{ExceptionType.Name}] >{{{Target}}}";
        }
    }
}
