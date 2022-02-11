using dnWalker.Parameters.Expressions;
using dnWalker.Traversal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using LinqParameterExpression = System.Linq.Expressions.ParameterExpression;
using ParameterExpression = dnWalker.Parameters.Expressions.ParameterExpression;

namespace dnWalker.Parameters
{
    public static class PathExtensions
    {
        private class SubstituteIdWithAccessVisitor : ExpressionVisitor
        {
            private readonly IParameterSet _context;


            public SubstituteIdWithAccessVisitor(IParameterSet context)
            {
                _context = context;
            }

            protected override Expression VisitParameter(LinqParameterExpression node)
            {
                string name = node.Name;
                Type type = node.Type;

                if (ParameterExpression.TryParse(name, out ParameterExpression expression))
                {
                    switch (expression)
                    {
                        case ValueOfParameterExpression valueOfExpr:
                            name = _context.Parameters[valueOfExpr.Operand].GetAccessString();
                            break;

                        case IsNullParameterExpression isNullExpr: break;
                            name = _context.Parameters[isNullExpr.Operand].GetAccessString();
                            break;

                        case LengthOfParameterExpression lengthOfExpr: break;
                            name = _context.Parameters[lengthOfExpr.Operand].GetAccessString();
                            break;

                        case RefEqualsParameterExpression refEqualsExpr: break;
                            Expression lhsExpr = Expression.Parameter(typeof(object), _context.Parameters[refEqualsExpr.LeftOperand].GetAccessString());
                            Expression rhsExpr = Expression.Parameter(typeof(object), _context.Parameters[refEqualsExpr.RightOperand].GetAccessString());

                            return Expression.ReferenceEqual(lhsExpr, rhsExpr);

                    }
                }

                return Expression.Parameter(type, name);
            }
        }
        public static Expression SubstitueRefWithAccess(this Expression expression, IParameterSet context)
        {
            return new SubstituteIdWithAccessVisitor(context).Visit(expression);
        }

        public static string GetConstraintStringWithAccesses(this Path path, IParameterSet context)
        {
            if (path.PathConstraint == null) return string.Empty;

            return path.PathConstraint.SubstitueRefWithAccess(context)?.ToString();
        }
    }
}
