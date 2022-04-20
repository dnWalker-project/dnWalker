using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract class ConstantExpression<T> : Expression
        where T : struct
    {
        public T Value { get; }

        protected ConstantExpression(T value)
        {
            Value = value;
        }
    }
}
