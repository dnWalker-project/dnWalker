using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public static class SystemLinqExpressionExtensions
    {
        private static readonly Dictionary<Type, object> _zeros = new Dictionary<Type, object>();
        public static Expression AsBoolean(this Expression expression)
        {
            Type t = expression.Type;
            if (t == typeof(bool)) return expression;

            return Expression.NotEqual(expression, Expression.Constant(GetZero(t)));

            static object GetZero(Type t)
            {
                if (!_zeros.TryGetValue(t, out object zero))
                {
                    zero = Convert.ChangeType(0, t);
                    _zeros.Add(t, zero);
                }
                return zero;
            }
        }

        public static Expression Optimize(this Expression expression)
        {
            if (expression is BinaryExpression binary)
            {
                Expression lhs = Optimize(binary.Left);
                Expression rhs = Optimize(binary.Right);

                switch (expression.NodeType)
                {
                    //case ExpressionType.Add: return OptimizeAdd(lhs, rhs);
                    //case ExpressionType.Subtract: return OptimizeSubtract(lhs, rhs);
                    //case ExpressionType.Multiply: return OptimizeMultiply(lhs, rhs);
                    //case ExpressionType.Divide: return OptimizeDivide(lhs, rhs);

                    //case ExpressionType.Equal: return OptimizeEqual(lhs, rhs);
                    //case ExpressionType.NotEqual: return OptimizeNotEqual(lhs, rhs);

                    //case ExpressionType.GreaterThan: return OptimizeGreaterThan(lhs, rhs);
                    //case ExpressionType.GreaterThanOrEqual: return OptimizeGreaterThanOrEqual(lhs, rhs);
                    //case ExpressionType.LessThan: return OptimizeLessThan(lhs, rhs);
                    //case ExpressionType.LessThanOrEqual: return OptimizeLessThanOrEqual(lhs, rhs);
                }


                return Expression.MakeBinary(expression.NodeType, lhs, rhs);
            }
            else if (expression is UnaryExpression unary)
            {
                Expression operand = unary.Operand;

                switch (expression.NodeType)
                {
                    //case ExpressionType.Negate: return OptimizeNegate(operand);
                    case ExpressionType.Not: return OptimizeNot(operand);
                }

                return Expression.MakeUnary(expression.NodeType, operand, expression.Type);
            }
            else
            {
                return expression;
            }
        }

        private static Expression OptimizeAdd(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeSubtract(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeMultiply(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeDivide(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeEqual(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeNotEqual(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeGreaterThan(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeGreaterThanOrEqual(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeLessThan(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeLessThanOrEqual(Expression lhs, Expression rhs)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeNegate(Expression operand)
        {
            throw new NotImplementedException();
        }

        private static Expression OptimizeNot(Expression operand)
        {
            if (operand is UnaryExpression unaryOperand && operand.NodeType == ExpressionType.Not)
            {
                // double not => negate them => just return the operand of the operand
                return unaryOperand.Operand;
            }

            if (operand is BinaryExpression binaryOperand)
            {
                ExpressionType? negatedNodeType = binaryOperand.NodeType switch
                {
                    ExpressionType.GreaterThan => ExpressionType.LessThanOrEqual,
                    ExpressionType.GreaterThanOrEqual => ExpressionType.LessThan,
                    ExpressionType.LessThan => ExpressionType.GreaterThanOrEqual,
                    ExpressionType.LessThanOrEqual => ExpressionType.GreaterThan,
                    ExpressionType.Equal => ExpressionType.NotEqual,
                    ExpressionType.NotEqual => ExpressionType.Equal,
                    _ => null
                };

                if (negatedNodeType.HasValue) return BinaryExpression.MakeBinary(negatedNodeType.Value, binaryOperand.Left, binaryOperand.Right);
            }

            return Expression.Not(operand);
        }
    }
}
