using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public class ControlFlowGraphProvider : CacheBase<MethodDef, ControlFlowGraph>
    {
        protected override ControlFlowGraph CreateValue(MethodDef key)
        {
            return ControlFlowGraph.Build(key);
        }
    }
}
