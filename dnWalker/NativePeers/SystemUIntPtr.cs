//using dnlib.DotNet;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.NativePeers
//{
//    public class SystemUIntPtr : NativePeer
//    {
//        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
//        {
//            iieReturnValue = null;
//            return false;
//        }

//        public override bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
//        {

//            if (methodDef.FullName == "System.Void System.UIntPtr::.ctor(System.UInt32)")
//            {
//                IDataElement dataElement = new UnsignedInt4(((UnsignedInt4)args[1]).Value);
//                cur.EvalStack.Push(dataElement);
//                return true;
//            }
//            else if (methodDef.FullName == "System.Void System.UIntPtr::.ctor(System.UInt64)")
//            {
//                IDataElement dataElement = new UnsignedInt8(((UnsignedInt8)args[1]).Value);
//                cur.EvalStack.Push(dataElement);
//                return true;
//            }

//            return false;
//        }
//    }
//}
