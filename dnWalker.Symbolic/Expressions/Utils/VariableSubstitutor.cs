using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public class VariableSubstitutor : ExpressionVisitor<IReadOnlyDictionary<IVariable, Expression>>
    {
        [ThreadStatic]
        private static readonly VariableSubstitutor _visitor = new VariableSubstitutor();

        public static Expression Substitute(Expression expression, IReadOnlyDictionary<IVariable, Expression> substitution)
        {
            return _visitor.Visit(expression, substitution);
        }

        protected internal override Expression VisitVariable(VariableExpression variable, IReadOnlyDictionary<IVariable, Expression> substitution)
        {
            // very uneffective aproach, we traverse the variable chain many times, setup some smart structure like prefix tree...

            IVariable v = variable.Variable;
            foreach ((IVariable from, Expression to) in substitution)
            {
                if (v.Equals(from))
                {
                    return to;
                }
            }

            // no substitution found, return
            return variable;
        }
    }
}
