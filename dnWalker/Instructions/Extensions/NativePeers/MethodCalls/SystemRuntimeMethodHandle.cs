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
    [NativePeer("System.Runtime.MethodHandle")]
    public class SystemRuntimeMethodHandle : CompiledMethodCallNativePeer<SystemRuntimeMethodHandle>
    {
        private static bool get_Value(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var value = args[0];
            switch (value)
            {
                case LocalVariablePointer localVariablePointer:
                    args = new DataElementList(1);
                    args[0] = localVariablePointer.Value;
                    return get_Value(method, args, cur, out returnValue);
                case MethodPointer methodPointer:
                    cur.EvalStack.Push(DataElement.CreateDataElement(methodPointer.Value.MethodSig.GetHashCode(), cur.DefinitionProvider));
                    returnValue = InstructionExecBase.nextRetval;
                    return true;
                default:
                    throw new NotImplementedException(value.GetType().FullName);
            }
        }
    }
}
