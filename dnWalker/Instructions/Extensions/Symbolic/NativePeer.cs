using dnlib.DotNet;
using dnlib.DotNet.Emit;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract class NativePeer : IInstructionExecutor
    {
        protected interface IMethodHandler
        {
            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur);
            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur);
        }


        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get { return _supportedOpCodes; }
        }

        protected static MethodDef GetMethod(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (instruction.Instruction.OpCode == OpCodes.Call ||
                instruction.Instruction.OpCode == OpCodes.Callvirt)
            {
                return (instruction.Operand as IMethod).ResolveMethodDefThrow();
            }
            else if (instruction.Instruction.OpCode == OpCodes.Calli)
            {
                MethodPointer methodPointer = (MethodPointer)cur.EvalStack.Peek(0);
                return methodPointer.Value;
            }

            throw new NotSupportedException();
        }

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }
}
