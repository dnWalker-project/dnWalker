using dnlib.DotNet.Emit;
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

            var symbA = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(a, "expression", out var exprA);
            var symbB = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(b, "expression", out var exprB);

            var isSymbolic = symbA || symbB;
            if (!isSymbolic)
            {
                cur.EvalStack.Push(CompareOperands(a, b) < 0 ? new Int4(1) : new Int4(0));
                return nextRetval;
            }

            var cltValue = CompareOperands(a, b) < 0 ? 1 : 0;

            var expression = Expression.MakeBinary(ExpressionType.LessThan, 
                exprA ?? a.AsExpression(),
                exprB ?? b.AsExpression());

            var newValue = new Int4(cltValue);
            cur.PathStore.CurrentPath.SetObjectAttribute(newValue, "expression", expression);

            cur.EvalStack.Push(newValue);//, expression));
            return nextRetval;
        }
    }

    public class CEQ : BranchInstructionExec
    {
        public CEQ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();

            var symbA = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(a, "expression", out var exprA);
            var symbB = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(b, "expression", out var exprB);

            var isSymbolic = symbA || symbB;
            if (!isSymbolic)
            {
                cur.EvalStack.Push(CompareOperands(a, b) == 0 ? new Int4(1) : new Int4(0));
                return nextRetval;
            }

            var ceqValue = CompareOperands(a, b) == 0 ? 1 : 0;

            var expression = Expression.MakeBinary(ExpressionType.Equal,
                exprA ?? a.AsExpression(),
                exprB ?? b.AsExpression());

            var newValue = new Int4(ceqValue);
            cur.PathStore.CurrentPath.SetObjectAttribute(newValue, "expression", expression);
            cur.EvalStack.Push(newValue);
            return nextRetval;
        }
    }
}
