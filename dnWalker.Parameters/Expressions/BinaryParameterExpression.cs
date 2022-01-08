using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public abstract class BinaryParameterExpression : ParameterExpression
    {
        protected BinaryParameterExpression(ParameterRef leftOperand, ParameterRef rightOperand)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public ParameterRef LeftOperand { get; }
        public ParameterRef RightOperand { get; }
    }
}
