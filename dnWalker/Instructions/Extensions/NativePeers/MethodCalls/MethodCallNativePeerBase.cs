using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    public abstract class MethodCallNativePeerBase : IMethodCallNativePeer
    {
        public abstract bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue);
        protected static bool PushReturnValue(IDataElement result, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (result == null)
            {
                returnValue = null;
                return false;
            }

            cur.EvalStack.Push(result);
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        protected static bool Fail(out IIEReturnValue returnValue)
        {
            returnValue = null;
            return false;
        }

        protected static bool Next(out IIEReturnValue returnValue)
        {
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
