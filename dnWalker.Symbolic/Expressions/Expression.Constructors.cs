using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class Expression
    {
        public static Expression MakeUnary(Operator op, Expression inner)
        {
            if (!op.CheckOperand(inner)) throw new InvalidOperationException($"Cannot create the '{op}' operation with operand {inner}");

            return new UnaryExpression(op, inner);
        }

        public static Expression MakeBinary(Operator op, Expression left, Expression right)
        {
            if (!op.CheckOperands(left, right)) throw new InvalidOperationException($"Cannot create the '{op}' operation with operands '{left}' and '{right}'");

            return new BinaryExpression(op, left, right);
        }

        public static Expression MakeConstant(TypeSig type, object? value) => new ConstantExpression(type, value);
        public static Expression MakeVariable(IVariable variable) => new VariableExpression(variable);


        public static Expression MakeNot(Expression inner)
        {
            if (inner is BinaryExpression bin &&
                bin.Operator.IsComparison())
            {
                // !(x != y) = (x == y)
                return new BinaryExpression(bin.Operator.Negate(), bin.Left, bin.Right);
            }
            else if (inner is UnaryExpression un &&
                un.Operator == Operator.Not)
            {
                // !!x = x
                return un.Inner;
            }
            return MakeUnary(Operator.Not, inner);
        }

        public static Expression MakeAnd(Expression left, Expression right) => MakeBinary(Operator.And, left, right);
        public static Expression MakeOr(Expression left, Expression right) => MakeBinary(Operator.Or, left, right);
        public static Expression MakeXor(Expression left, Expression right) => MakeBinary(Operator.Xor, left, right);


        public static Expression MakeAdd(Expression left, Expression right) => MakeBinary(Operator.Add, left, right);
        public static Expression MakeSubtract(Expression left, Expression right) => MakeBinary(Operator.Subtract, left, right);
        public static Expression MakeMultiply(Expression left, Expression right) => MakeBinary(Operator.Multiply, left, right);
        public static Expression MakeDivide(Expression left, Expression right) => MakeBinary(Operator.Divide, left, right);
        public static Expression MakeRemainder(Expression left, Expression right) => MakeBinary(Operator.Modulo, left, right);
        public static Expression MakeNegate(Expression inner) => MakeUnary(Operator.Negate, inner);
        public static Expression MakeShiftLeft(Expression left, Expression right) => MakeBinary(Operator.ShiftLeft, left, right);
        public static Expression MakeShiftRight(Expression left, Expression right) => MakeBinary(Operator.ShiftRight, left, right);


        public static Expression MakeEqual(Expression left, Expression right) => MakeBinary(Operator.Equal, left, right);
        public static Expression MakeNotEqual(Expression left, Expression right) => MakeBinary(Operator.NotEqual, left, right);
        public static Expression MakeGreaterThan(Expression left, Expression right) => MakeBinary(Operator.GreaterThan, left, right);
        public static Expression MakeGreaterThanOrEqual(Expression left, Expression right) => MakeBinary(Operator.GreaterThanOrEqual, left, right);
        public static Expression MakeLessThan(Expression left, Expression right) => MakeBinary(Operator.LessThan, left, right);
        public static Expression MakeLessThanOrEqual(Expression left, Expression right) => MakeBinary(Operator.LessThanOrEqual, left, right);

        public static Expression MakeConvert(TypeSig type, Expression inner) => new ConvertExpression(type, inner);
    }
}
