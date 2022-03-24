using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Parameters
{
    public class RET : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Ret };
        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedOpCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            // we are not in the entry point
            if (cur.CallStack.StackPointer > 1)
            {
                return next(baseExecutor, cur);
            }

            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                return next(baseExecutor, cur);
            }


            MethodDef method = cur.CurrentMethod.Definition;
            if (method.HasReturnType)
            {
                TypeSig retType = method.ReturnType;

                IDataElement returnValue = cur.EvalStack.Peek();
                store.SetReturnValue(returnValue, cur, retType);
            }

            return next(baseExecutor, cur);
        }
    }
}
