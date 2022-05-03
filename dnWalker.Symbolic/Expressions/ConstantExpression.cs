using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(TypeSig type, object? value)
        {
            Type = type;
            Value = value;
        }

        public override TypeSig Type { get; }
        public object? Value { get; }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitConstant(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitConstant(this, context);
    }
}
