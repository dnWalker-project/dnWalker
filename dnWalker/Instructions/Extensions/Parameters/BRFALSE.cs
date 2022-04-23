//using dnlib.DotNet.Emit;

//using dnWalker.Parameters;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Instructions.Extensions.Parameters
//{

//    public class BRFALSE : IInstructionExecutor
//    {
//        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Brfalse, OpCodes.Brfalse_S};

//        public IEnumerable<OpCode> SupportedOpCodes
//        {
//            get
//            {
//                return _supportedOpCodes;
//            }
//        }

//        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
//        {
//            // we need to handle situations like:
//            // if (o != null)
//            // { ... }
//            // we replace the object reference with a boolean value (true iff o != null) which has attached proper expression

//            IDataElement operand = cur.EvalStack.Peek();
//            if (operand.TryGetParameter(cur, out IReferenceTypeParameter referenceTypeParameter))
//            {
//                cur.EvalStack.Pop();
//                cur.EvalStack.Push(cur.GetIsNotNullDataElement(referenceTypeParameter));
//            }

//            return next(instruction, cur);
//        }
//    }
//}
