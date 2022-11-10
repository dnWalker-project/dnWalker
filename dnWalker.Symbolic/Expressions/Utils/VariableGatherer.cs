using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public class VariableGatherer : ExpressionVisitor<ICollection<IVariable>>
    {
        protected internal override Expression VisitVariable(VariableExpression variable, ICollection<IVariable> context)
        {
            context.Add(variable.Variable);
            return variable;
        }

        public static readonly VariableGatherer Instance = new VariableGatherer();

        public static TCollection GetVariables<TCollection>(Expression expression)
            where TCollection : ICollection<IVariable>, new()
        {
            TCollection variables = new TCollection();
            GetVariables(expression, variables);
            return variables;
        }
        public static void GetVariables<TCollection>(Expression expression, TCollection variables)
            where TCollection : ICollection<IVariable>
        {
            Instance.Visit(expression, variables);
        }
        public static TCollection GetVariables<TCollection>(IEnumerable<Expression> expressions)
            where TCollection : ICollection<IVariable>, new()
        {
            TCollection variables = new TCollection();
            GetVariables(expressions, variables);
            return variables;
        }
        public static void GetVariables<TCollection>(IEnumerable<Expression> expressions, TCollection variables)
            where TCollection : ICollection<IVariable>
        {
            foreach (Expression expression in expressions)
            {
                Instance.Visit(expression, variables);
            }
        }
    }
}
