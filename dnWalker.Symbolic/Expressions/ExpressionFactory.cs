using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class ExpressionFactory
    {
        private Expression? _nullExpression = null;
        private Expression? _stringNullExpression = null;

        protected ExpressionFactory()
        {
        }


        public virtual Expression NullExpression
        {
            get
            {
                if (_nullExpression == null) _nullExpression = MakeConstant(GetObjectSig(), null);
                return _nullExpression;
            }
        }
        public virtual Expression StringNullExpression
        {
            get
            {
                if (_stringNullExpression == null) _stringNullExpression = MakeConstant(GetStringSig(), null);
                return _stringNullExpression;
            }
        }



        public virtual Expression MakeUnary(Operator op, Expression inner)
        {
            return Expression.MakeUnary(op, inner);
        }

        public virtual Expression MakeBinary(Operator op, Expression left, Expression right)
        {
            return Expression.MakeBinary(op, left, right);
        }

        public virtual Expression MakeConstant(TypeSig type, object? value) => new ConstantExpression(type, value);
        public virtual Expression MakeVariable(IVariable variable) => new VariableExpression(variable);


        public virtual Expression MakeNot(Expression inner) => MakeUnary(Operator.Not, inner);
        public virtual Expression MakeAnd(Expression left, Expression right) => MakeBinary(Operator.And, left, right);
        public virtual Expression MakeOr(Expression left, Expression right) => MakeBinary(Operator.And, left, right);
        public virtual Expression MakeXor(Expression left, Expression right) => MakeBinary(Operator.And, left, right);


        public virtual Expression MakeAdd(Expression left, Expression right) => MakeBinary(Operator.Add, left, right);
        public virtual Expression MakeSubtract(Expression left, Expression right) => MakeBinary(Operator.Subtract, left, right);
        public virtual Expression MakeMultiply(Expression left, Expression right) => MakeBinary(Operator.Multiply, left, right);
        public virtual Expression MakeDivide(Expression left, Expression right) => MakeBinary(Operator.Divide, left, right);
        public virtual Expression MakeRemainder(Expression left, Expression right) => MakeBinary(Operator.Modulo, left, right);
        public virtual Expression MakeNegate(Expression inner) => MakeUnary(Operator.Negate, inner);
        public virtual Expression MakeShiftLeft(Expression left, Expression right) => MakeBinary(Operator.ShiftLeft, left, right);
        public virtual Expression MakeShiftRight(Expression left, Expression right) => MakeBinary(Operator.ShiftRight, left, right);


        public virtual Expression MakeEqual(Expression left, Expression right) => MakeBinary(Operator.Equal, left, right);
        public virtual Expression MakeNotEqual(Expression left, Expression right) => MakeBinary(Operator.NotEqual, left, right);
        public virtual Expression MakeGreaterThan(Expression left, Expression right) => MakeBinary(Operator.GreaterThan, left, right);
        public virtual Expression MakeGreaterThanOrEqual(Expression left, Expression right) => MakeBinary(Operator.GreaterThanOrEqual, left, right);
        public virtual Expression MakeLessThan(Expression left, Expression right) => MakeBinary(Operator.LessThan, left, right);
        public virtual Expression MakeLessThanOrEqual(Expression left, Expression right) => MakeBinary(Operator.LessThanOrEqual, left, right);

        public virtual Expression MakeConvert(TypeSig type, Expression inner) => Expression.MakeConvert(type, inner);


        public virtual Expression MakeIntegerConstant(long value) => MakeConstant(GetIntegerSig(), value);
        public virtual Expression MakeRealConstant(double value) => MakeConstant(GetDoubleSig(), value);
        public virtual Expression MakeStringConstant(string value) => MakeConstant(GetStringSig(), value);
        public virtual Expression MakeBooleanConstant(bool value) => MakeConstant(GetBooleanSig(), value);


        protected abstract TypeSig GetIntegerSig();
        protected abstract TypeSig GetBooleanSig();
        protected abstract TypeSig GetDoubleSig();
        protected abstract TypeSig GetStringSig();
        protected abstract TypeSig GetObjectSig();
    }
}
