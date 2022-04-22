using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class Expression
    {
        public abstract ExpressionType Type { get; }

        public abstract Expression Accept(ExpressionVisitor visitor);
        public abstract Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state);


        public static readonly Expression False = new BooleanConstantExpression(false);
        public static readonly Expression True = new BooleanConstantExpression(true);
        public static readonly Expression RealZero = new RealConstantExpression(0.0);
        public static readonly Expression IntegerZero = new IntegerConstantExpression(0);
        public static readonly Expression Null = new NullExpression();

        public virtual Expression AsBoolean()
        {
            ExpressionType type = Type;
            switch (type)
            {
                case ExpressionType.Boolean: return this;
                case ExpressionType.Integer: return new BinaryOperationExpression(Operator.NotEqual, this, Expression.IntegerZero);
                case ExpressionType.Real: return new BinaryOperationExpression(Operator.NotEqual, this, Expression.RealZero);
                case ExpressionType.Location: return new BinaryOperationExpression(Operator.NotEqual, this, Expression.Null);
                case ExpressionType.String: return True;
            }
            throw new NotSupportedException("Unexpected resulting type.");
        }

        public override string ToString()
        {
            return Utils.ExpressionWriter.ToString(this);
        }

    }
}
