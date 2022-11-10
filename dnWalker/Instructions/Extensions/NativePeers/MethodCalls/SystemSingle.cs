using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer("System.Single", MatchMethods = true)]
    public class SystemSingle : CompiledMethodCallNativePeer<SystemSingle>
    {
        private static bool Equals(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var left = args[0];
            if (left is IManagedPointer lp)
            {
                left = lp.Value;
            }

            var right = args[1];
            if (right is IManagedPointer rp)
            {
                right = rp.Value;
            }

            var dataElement = new Int4(left.CompareTo(right) == 0 ? 1 : 0);
            cur.EvalStack.Push(dataElement);
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        private static bool IsNaN(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var dataElement = new Int4(double.IsNaN(((Float4)args[0]).Value) ? 1 : 0);
            cur.EvalStack.Push(dataElement);
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
