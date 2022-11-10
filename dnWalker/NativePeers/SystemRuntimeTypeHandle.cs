//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using dnlib.DotNet;

//using dnWalker.TypeSystem;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//namespace dnWalker.NativePeers
//{
//    public class SystemRuntimeTypeHandle : NativePeer
//    {
//        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
//        {
//            if (method.FullName == "System.IntPtr System.RuntimeTypeHandle::get_Value()")
//            {
//                var value = args[0];
//                switch (value)
//                {
//                    case LocalVariablePointer localVariablePointer:
//                        args = new DataElementList(1);
//                        args[0] = localVariablePointer.Value;
//                        return TryGetValue(method, args, cur, out iieReturnValue);
//                    case TypePointer typePointer:
//                        var type = typePointer.Type;
//                        if (type.TryGetTypeHandle(out var handle))
//                        {
//                            cur.EvalStack.Push(DataElement.CreateDataElement(handle.Value, cur.DefinitionProvider));
//                            iieReturnValue = InstructionExecBase.nextRetval;
//                            return true;
//                        }

//                        throw new NotImplementedException(type.FullName);
//                    default:
//                        throw new NotImplementedException(value.GetType().FullName);
//                }
//            }

//            throw new NotImplementedException("Native peer for " + method.FullName);
//        }
//    }
//}
