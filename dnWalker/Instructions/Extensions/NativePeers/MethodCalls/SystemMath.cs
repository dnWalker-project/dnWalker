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
    [NativePeer("System.Math", MatchMethods = true)]
    public class SystemMath : CompiledMethodCallNativePeer<SystemMath>
    {
        private static bool Ceiling(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters[0].Type.IsDouble())
                return PushReturnValue(new Float8(Math.Ceiling(((Float8)args[0]).Value)), cur, out returnValue);
            
            returnValue = null;
            return false;
        }

        private static bool Sqrt(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var arg = (Float8)args[0];
            var value = arg.Value;
            var dataElement = new Float8(Math.Sqrt(value));
            cur.EvalStack.Push(dataElement);
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
