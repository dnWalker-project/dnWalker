﻿using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public class VirtualExceptionHandlerNode : ControlFlowNode
    {
        private readonly TypeDef _exceptionType;

        public VirtualExceptionHandlerNode(TypeDef exceptionType)
        {
            _exceptionType = exceptionType ?? throw new ArgumentNullException(nameof(exceptionType));
        }

        public TypeDef ExceptionType => _exceptionType;
    }
}
