﻿using dnlib.DotNet;

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
        private bool _isCovered;
        private bool _isUnreachable;
        private readonly ControlFlowNode _source;
        private readonly ControlFlowNode _target;

        protected ControlFlowEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }


        public void MarkCovered()
        {
            _isCovered = true;
        }

        public void MarkUnreachable()
        {
            _isUnreachable = true;
        }

        public bool IsCovered => _isCovered;
        public bool IsReachable => !_isUnreachable;

        public ControlFlowNode Source => _source;
        public ControlFlowNode Target => _target;
    }

    public class NextEdge : ControlFlowEdge
    {
        public NextEdge([NotNull] ControlFlowNode source, [NotNull] ControlFlowNode target) : base(source, target)
        {
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
    }
}
