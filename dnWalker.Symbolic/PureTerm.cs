using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;

namespace dnWalker.Symbolic
{
    public abstract class PureTerm : Term
    {
        public abstract Expression GetExpression();
    }


    public class BooleanExpressionTerm : PureTerm
    {
        public BooleanExpressionTerm(Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));

            TypeSig type = expression.Type;
            if (!TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Boolean))
            {
                throw new ArgumentException("The expression must be of boolean type.", nameof(expression));
            }
        }

        public Expression Expression { get; }

        public override void GetVariables(ICollection<IVariable> variables)
        {
            VariableGatherer.GetVariables(Expression, variables);
        }

        public override Expression GetExpression() => Expression;

        public override string ToString() => Expression.ToString();
    }
}