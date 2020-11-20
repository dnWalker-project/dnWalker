using dnlib.DotNet.Emit;
using dnWalker.Symbolic.Primitives;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System.Linq.Expressions;

namespace dnWalker.Symbolic.Instructions
{
    public class CompareInstruction : InstructionExecBase
    {
        public CompareInstruction(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        protected int CompareOperands(IDataElement a, IDataElement b)
        {
            if (Unsigned && a is INumericElement na && b is INumericElement nb)
            {
                if (na.Equals(nb))
                {
                    return 0;
                }

                return na.ToUnsignedInt8(CheckOverflow).CompareTo(nb.ToUnsignedInt8(CheckOverflow));
            }

            return a.CompareTo(b);
        }
    }

    public class CLT : MMC.InstructionExec.CLT
    {
        public CLT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();

            var symbA = a as ISymbolic;
            var symbB = b as ISymbolic;

            var isSymbolic = symbA != null || symbB != null;
            if (!isSymbolic)
            {
                cur.EvalStack.Push(CompareOperands(a, b) < 0 ? new Int4(1) : new Int4(0));
                return nextRetval;
            }

            var cltValue = CompareOperands(a, b) < 0 ? 1 : 0;

            var expression = Expression.MakeBinary(ExpressionType.LessThan, 
                symbA?.Expression ?? a.AsExpression(),
                symbB?.Expression ?? b.AsExpression());

            cur.EvalStack.Push(new SymbolicInt4(cltValue, expression));
            return nextRetval;
        }
    }
}
