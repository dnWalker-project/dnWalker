using dnlib.DotNet.Emit;
using MMC.InstructionExec;
using MMC.State;
using System.Linq.Expressions;

namespace dnWalker.Symbolic.Instructions
{
    public class BRFALSE : BranchInstructionExec
    {
        public BRFALSE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            var value = a.ToBool();
            var operand = (Instruction)Operand;

            if (a is ISymbolic s)
            {
                cur.PathStore.AddPathConstraint(
                    !value ? Expression.Not(s.Expression) : s.Expression, 
                    !value ? operand : null, // fall-through
                    cur);
            }

            return !value ? new JumpReturnValue(operand) : nextRetval;
        }
    }
}
