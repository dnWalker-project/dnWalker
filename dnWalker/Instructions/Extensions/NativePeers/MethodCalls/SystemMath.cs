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

        private static bool Max(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            // Math.Max(v1, v2)
            INumericElement v1 = (INumericElement)args[0];
            INumericElement v2 = (INumericElement)args[1];
            if (v1.CompareTo(v2) > 0)
            {
                cur.EvalStack.Push(v1);
            }
            else
            {
                cur.EvalStack.Push(v2);
            }
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        private static bool Min(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            // Math.Min(v1, v2)
            INumericElement v1 = (INumericElement)args[0];
            INumericElement v2 = (INumericElement)args[1];
            if (v1.CompareTo(v2) < 0)
            {
                cur.EvalStack.Push(v1);
            }
            else
            {
                cur.EvalStack.Push(v2);
            }
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        private static bool Abs(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            // Math.Abs(v1)
            INumericElement v1 = (INumericElement)args[0];
            if (v1.CompareTo(Int4.Zero) < 0)
            {
                cur.EvalStack.Push(v1.Mul(new Int4(-1), false));
            }
            else
            {
                cur.EvalStack.Push(v1);
            }
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        private static bool Pow(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            INumericElement a = (INumericElement)args[0];
            INumericElement b = (INumericElement)args[1];

            double aDbl = a.ToFloat8(false).Value;
            double bDbl = b.ToFloat8(false).Value;

            IDataElement result = new Float8(Math.Pow(aDbl, bDbl));
            cur.EvalStack.Push(result);

            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
