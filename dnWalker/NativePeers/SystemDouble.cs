//using dnlib.DotNet;
//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//namespace dnWalker.NativePeers
//{
//    public class SystemDouble : NativePeer
//    {
//        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
//        {
//            if (method.FullName == "System.Boolean System.Double::IsNaN(System.Double)")
//            {
//                var dataElement = new Int4(double.IsNaN(((Float8)args[0]).Value) ? 1 : 0);
//                cur.EvalStack.Push(dataElement);
//                iieReturnValue = InstructionExecBase.nextRetval;
//                return true;
//            }

//            if (method.FullName == "System.Boolean System.Double::Equals(System.Double)")
//            {
//                var left = args[0];
//                if (left is IManagedPointer lp)
//                {
//                    left = lp.Value;
//                }

//                var right = args[1];
//                if (right is IManagedPointer rp)
//                {
//                    right = rp.Value;
//                }

//                var dataElement = new Int4(left.CompareTo(right) == 0 ? 1 : 0);
//                cur.EvalStack.Push(dataElement);
//                iieReturnValue = InstructionExecBase.nextRetval;
//                return true;
//            }

//            iieReturnValue = null;
//            return false;
//        }
//    }
//}
