using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class ParameterTrait
    {
        private readonly ParameterExpression _expression;
        private readonly object _result;

        public ParameterTrait(ParameterExpression expression, object result)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public ParameterExpression Expression 
        {
            get { return _expression; }
        }
        
        public object Result
        {
            get { return _result; }
        }

        public T ResultAs<T>()
            where T : struct
        {
            return (T)_result;
        }

        public bool TryApplyTo(ParameterStore store)
        {
            return Expression.TryApplyTo(store, Result);
        }
    }
}
