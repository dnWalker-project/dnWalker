using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMC.InstructionExec;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemRuntimeMethodHandle : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (method.FullName == "System.IntPtr System.RuntimeMethodHandle::get_Value()")
            {
                var value = args[0];
                switch (value)
                {
                    case LocalVariablePointer localVariablePointer:
                        args = new DataElementList(1);
                        args[0] = localVariablePointer.Value;
                        return TryGetValue(method, args, cur, out iieReturnValue);
                    case MethodPointer methodPointer:
                        cur.EvalStack.Push(cur.DefinitionProvider.CreateDataElement(methodPointer.Value.MethodSig.GetHashCode()));
                        iieReturnValue = InstructionExecBase.nextRetval;
                        return true;
                    default:
                        throw new NotImplementedException(value.GetType().FullName);
                }
            }

            throw new NotImplementedException("Native peer for " + method.FullName);
        }
    }
}
