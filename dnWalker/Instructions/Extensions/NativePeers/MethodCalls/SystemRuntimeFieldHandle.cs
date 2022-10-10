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
    [NativePeer("System.Runtime.FieldHandle", MatchMethods = true)]
    internal class SystemRuntimeFieldHandle : CompiledMethodCallNativePeer<SystemRuntimeFieldHandle>
    {
#pragma warning disable IDE1006 // Naming Styles - must match the bypassed method name
        private static bool get_Value(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var value = args[0];
            switch (value)
            {
                case LocalVariablePointer localVariablePointer:
                    args = new DataElementList(1);
                    args[0] = localVariablePointer.Value;
                    return get_Value(method, args, cur, out returnValue);
                case FieldHandle fieldHandle:
                    cur.EvalStack.Push(DataElement.CreateDataElement(fieldHandle.Value.GetHashCode(), cur.DefinitionProvider));
                    returnValue = InstructionExecBase.nextRetval;
                    return true;
                default:
                    throw new NotImplementedException(value.GetType().FullName);
            }
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
