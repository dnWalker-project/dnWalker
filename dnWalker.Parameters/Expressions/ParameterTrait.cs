using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class ParameterTrait
    {
        public ParameterTrait(ParameterExpression expression, object value)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Value = value;
        }

        public ParameterExpression Expression { get; }
        public object Value { get; }


        public void ApplyTo(IParameterSet ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));

            Expression.ApplyTo(ctx, Value);
        }
    }
}
