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
    [NativePeer("System.Runtime.TypeHandle")]
    public class SystemRuntimeTypeHandle : CompiledMethodCallNativePeer<SystemRuntimeMethodHandle>
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
                case TypePointer typePointer:
                    var type = typePointer.Type;
                    if (type.TryGetTypeHandle(out var handle))
                    {
                        cur.EvalStack.Push(DataElement.CreateDataElement(handle.Value, cur.DefinitionProvider));
                        returnValue = InstructionExecBase.nextRetval;
                        return true;
                    }

                    throw new NotImplementedException(type.FullName);
                default:
                    throw new NotImplementedException(value.GetType().FullName);
            }
        }
    }
}
