using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Traversal;
using Echo.Platforms.Dnlib;
using MMC.State;
using MMC.Util;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Traversal
{
    public class MethodExplorer
    {
        private readonly ControlFlowGraph _cfg;
        private readonly Dictionary<Instruction, int> _coverageMap;
        private readonly MethodDef _method;
        
        public MethodExplorer(MethodDef method)
        {
            _coverageMap = method.Body.Instructions.ToDictionary(i => i, i => 0);
            _method = method;

            _cfg = ControlFlowGraph.Build(method);
        }

        public void OnInstructionExecuted(CILLocation location, Path path)
        {
            if (location.Method != _method)
            {
                return;
            }

            _coverageMap[location.Instruction]++;

            var node = _cfg.GetNode(location.Instruction);
            node.SetIsCovered();

            path.AddVisitedNode(node);
        }

        public Coverage GetCoverage()
        {
            return _cfg.GetCoverage();
        }
    }
}
