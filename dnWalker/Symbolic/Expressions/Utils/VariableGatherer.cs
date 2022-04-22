using System;
using System.Collections.Generic;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public class VariableGatherer : ExpressionVisitor<ICollection<IVariable>>
    {
        public static readonly VariableGatherer Instance = new VariableGatherer();

        public static ICollection<IVariable> GetVariables(Expression expression)
        {
            HashSet<IVariable> variables = new HashSet<IVariable>();
            Instance.Visit(expression, variables);
            return variables;
        }

        public static ICollection<IVariable> GetVariables(IEnumerable<Expression> expressions)
        {
            HashSet<IVariable> variables = new HashSet<IVariable>();
            foreach (Expression expression in expressions)
            {
                Instance.Visit(expression, variables);
            }
            return variables;
        }


        public override Expression VisitVariable(VariableExpression variableExpression, ICollection<IVariable> state)
        {
            state.Add(variableExpression.Variable);
            return base.VisitVariable(variableExpression, state);
        }
    }
}

