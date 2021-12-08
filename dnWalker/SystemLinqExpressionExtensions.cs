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
        public static Expression Simplify(this Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    return SimplifyBinary(binaryExpression);
                case UnaryExpression unaryExpression:
                    return SimplifyUnary(unaryExpression);                 
                default:
                    return expression;
            }


            //while (expression.CanReduce)
            //{
            //    expression = expression.Reduce();
            //}
            //return expression;
        }

        private static Expression SimplifyUnary(UnaryExpression unaryExpression)
        {
            Expression operand = unaryExpression.Operand.Simplify();

            switch (unaryExpression.NodeType)
            {
               
                case ExpressionType.Negate:
                    break;
                case ExpressionType.Not:
                    Expression lhs = ((BinaryExpression)operand).Left;
                    Expression rhs = ((BinaryExpression)operand).Right;

                    ExpressionType negation = operand.NodeType.LogicalNot();
                    if (negation != operand.NodeType)
                    {
                        return Expression.MakeBinary(negation, lhs, rhs);
                    }

                    break;

                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
            }

            return Expression.MakeUnary(unaryExpression.NodeType, operand, operand.Type);
        }


        private static Expression SimplifyBinary(BinaryExpression binaryExpression)
        {
            Expression lhs = binaryExpression.Left.Simplify();
            Expression rhs = binaryExpression.Right.Simplify();

            return Expression.MakeBinary(binaryExpression.NodeType, lhs, rhs);
        }

        private static ExpressionType LogicalNot(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal: return ExpressionType.NotEqual;
                case ExpressionType.LessThan: return ExpressionType.GreaterThanOrEqual;
                case ExpressionType.LessThanOrEqual: return ExpressionType.GreaterThan;
                case ExpressionType.GreaterThan: return ExpressionType.LessThanOrEqual;
                case ExpressionType.GreaterThanOrEqual: return ExpressionType.LessThan;
                default:
                    return type;
            }
        }
    }
}
