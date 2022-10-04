using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public abstract class ControlFlowElement
    {
        protected ControlFlowElement(MethodDef method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public MethodDef Method { get; }
    }
}
