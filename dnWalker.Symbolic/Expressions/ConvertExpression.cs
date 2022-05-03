using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class ConvertExpression : Expression
    {
        public Expression Inner { get; }

        internal ConvertExpression(TypeSig type, Expression inner)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override TypeSig Type { get; }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitConvert(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitConvert(this, context);
    }
}
