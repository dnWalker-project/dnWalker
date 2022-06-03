using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class RET : IInstructionExecutor
    {
        private static OpCode[] _supportedOpCodes = new[] { OpCodes.Ret };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            MethodDef method = cur.CurrentMethod.Definition;

            if (cur.CallStack.StackPointer > 1 ||
                !cur.TryGetSymbolicContext(out SymbolicContext context) ||
                !method.HasReturnType)
            {
                return next(baseExecutor, cur);
            }

            TypeSig retType = method.ReturnType;
            IDataElement retVal = cur.EvalStack.Peek();

            IIEReturnValue returnValue = next(baseExecutor, cur);

            context.SetReturnValue(retVal, cur, retType);

            return returnValue;
        }
    }
}
