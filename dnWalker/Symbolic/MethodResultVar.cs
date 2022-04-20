using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    // TODO implement
    public readonly struct MethodResultVar // : IVariable
    {
        public IMethod Method { get; }
        public int Invocation { get; }
    }
}
