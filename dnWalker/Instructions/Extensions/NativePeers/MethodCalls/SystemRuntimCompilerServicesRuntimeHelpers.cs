using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer(typeof(System.Runtime.CompilerServices.RuntimeHelpers))]
    public class SystemRuntimCompilerServicesRuntimeHelpers : CompiledMethodCallNativePeer<SystemRuntimCompilerServicesRuntimeHelpers>
    {
        public static bool GetHashCode(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            cur.EvalStack.Push(new Int4(args[0].GetHashCode()));
            return Next(out returnValue);
        }
    }
}
