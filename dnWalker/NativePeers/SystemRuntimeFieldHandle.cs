using System;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;
using MMC.InstructionExec;

namespace dnWalker.NativePeers
{
    public class SystemRuntimeFieldHandle : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (method.FullName == "System.IntPtr System.RuntimeFieldHandle::get_Value()")
            {
                var value = args[0];
                switch (value)
                {
                    case LocalVariablePointer localVariablePointer:
                        args = new DataElementList(1);
                        args[0] = localVariablePointer.Value;
                        return TryGetValue(method, args, cur, out iieReturnValue);
                    case FieldHandle fieldHandle:
                        cur.EvalStack.Push(cur.DefinitionProvider.CreateDataElement(fieldHandle.Value.GetHashCode()));
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
