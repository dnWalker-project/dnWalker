using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract class Branch : IInstructionExecutor
    {

        public abstract IEnumerable<OpCode> SupportedOpCodes { get; }

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);

        protected static void MakeDecision(ExplicitActiveState cur, int decision, params dnWalker.Symbolic.Expressions.Expression[] choices)
        {
            cur.Services.GetService<ConstraintTreeExplorer>().MakeDecision(cur, decision, choices);
        }

        protected static Instruction GetNextInstruction(IIEReturnValue retValue, ExplicitActiveState cur)
        {
            if (retValue is NextReturnValue) return null;
            if (retValue is JumpReturnValue jrv) return jrv.GetNextInstruction(cur.CurrentMethod);

            throw new NotSupportedException("Unexpected return value type...");
        }
    }
}
