using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public abstract class UnaryParameterExpression : ParameterExpression
    {
        protected UnaryParameterExpression(ParameterRef operand)
        {
            Operand = operand;
        }

        public ParameterRef Operand { get; }
    }
}
