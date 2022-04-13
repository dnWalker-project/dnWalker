using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public class SystemLinqExpressionOptimizer : ExpressionVisitor
    {
        public static readonly SystemLinqExpressionOptimizer Instance = new SystemLinqExpressionOptimizer();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    {
                        if (node.Right is ConstantExpression constExpr &&
                            constExpr.Value is bool b)
                        {
                            if (b)
                            {
                                // lhs == True => return lhs
                                return Visit(node.Left);
                            }
                            else
                            {
                                // lhs == False => return Negate(lhs)
                                return Negate(node.Left) ?? Expression.Not(Visit(node.Left));
                            }
                        }
                    }
                    break;

                case ExpressionType.NotEqual:
                    {
                        if (node.Right is ConstantExpression constExpr &&
                        constExpr.Value is bool b)
                        {
                            if (b)
                            {
                                // lhs != True => return Negate(lhs)
                                return Negate(node.Left) ?? Expression.Not(Visit(node.Left));
                            }
                            else
                            {
                                // lhs != False => return lhs
                                return Visit(node.Left);
                            }
                        }
                    }
                    break;
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not: return Negate(node.Operand) ?? base.VisitUnary(node);
            }

            return base.VisitUnary(node);
        }

        private static readonly Dictionary<ExpressionType, ExpressionType> _compNegations = new Dictionary<ExpressionType, ExpressionType>()
        {
            [ExpressionType.Equal] = ExpressionType.NotEqual,
            [ExpressionType.NotEqual] = ExpressionType.Equal,
            [ExpressionType.GreaterThan] = ExpressionType.LessThanOrEqual,
            [ExpressionType.GreaterThanOrEqual] = ExpressionType.LessThan,
            [ExpressionType.LessThan] = ExpressionType.GreaterThanOrEqual,
            [ExpressionType.LessThanOrEqual] = ExpressionType.GreaterThan,

        };

        private Expression Negate(Expression expression)
        {
            expression = Visit(expression);
            if (expression is BinaryExpression binExpr &&
                _compNegations.TryGetValue(binExpr.NodeType, out ExpressionType negComp))
            {
                // negate comparator
                return Expression.MakeBinary(negComp, binExpr.Left, binExpr.Right);
            }

            else if (expression is UnaryExpression unaryExpr && 
                unaryExpr.NodeType == ExpressionType.Not)
            {
                // double negation
                return unaryExpr.Operand;
            }

            return null;
        }
    }


}
