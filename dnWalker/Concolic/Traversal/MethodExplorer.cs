using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Traversal;

using MMC.State;
using MMC.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Traversal
{
    public class MethodExplorer
    {
        private readonly ControlFlowGraph _cfg;
        private readonly MethodDef _method;
        
        public MethodExplorer(MethodDef method)
        {
            _method = method;

            _cfg = ControlFlowGraph.Build(method);
        }

        private Instruction _lastInstruction;

        public Instruction LastInstruction => _lastInstruction;

        public void OnInstructionExecuted(Instruction instruction)
        {
            _lastInstruction = instruction;
        }

        public ControlFlowGraph Graph => _cfg;
    }
}
