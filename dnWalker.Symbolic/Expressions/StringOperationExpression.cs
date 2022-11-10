using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class StringOperationExpression : Expression
    {
        public StringOperationExpression(Operator op, Expression[] operands)
        {
            Operator = op;
            Operands = operands ?? throw new ArgumentNullException(nameof(operands));

            Type = op.GetResultType(operands);
        }

        public Operator Operator { get; }

        public Expression[] Operands { get; }

        public override TypeSig Type { get; }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitStringOperation(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitStringOperation(this, context);
    }
}
