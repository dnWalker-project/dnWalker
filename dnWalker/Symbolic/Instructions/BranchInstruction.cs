using dnlib.DotNet.Emit;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.Symbolic.Instructions
{
    public class BranchInstruction
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
                if (a is ISymbolic)
                {
                    var path = cur.PathStore.CurrentPath;
                    path.AddPathConstraint();
                }

                return !a.ToBool() ? new JumpReturnValue((Instruction)Operand) : nextRetval;
            }
        }
    }
}
