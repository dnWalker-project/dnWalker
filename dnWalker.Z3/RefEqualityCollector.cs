using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3
{
    public class RefEqualityCollector : ExpressionVisitor<ICollection<(IVariable v1, IVariable v2)>>
    {
        public static readonly RefEqualityCollector Instance = new RefEqualityCollector();

        public static ICollection<(IVariable v1, IVariable v2)> GetRefEqualities(IEnumerable<Expression> expressions)
        {
            HashSet<(IVariable v1, IVariable v2)> refEqualities = new HashSet<(IVariable v1, IVariable v2)>();

            foreach (Expression expression in expressions)
            {
                GetRefEqualities(expression, refEqualities);
            }

            return refEqualities;
        }

        public static void GetRefEqualities(Expression expression, ICollection<(IVariable v1, IVariable v2)> refEqualities)
        {
            Instance.Visit(expression, refEqualities);
        }

        protected override Expression VisitBinary(BinaryExpression binary, ICollection<(IVariable v1, IVariable v2)> context)
        {
            if (binary.Operator == Operator.Equal &&
                binary.Left is VariableExpression leftVarExpr &&
                binary.Right is VariableExpression rightVarExpr)
            {
                // we have an equality comparer and both parts are variables
                IVariable lVar = leftVarExpr.Variable;
                IVariable rVar = rightVarExpr.Variable;

                context.Add((lVar, rVar));

                return binary;
            }
            else
            {
                return base.VisitBinary(binary, context);
            }
        }
    }
}
