using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

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


        public override Expression AsBoolean()
        {
            bool val;
            if (Type.IsBoolean())
            {
                return this;
            }
            else if (Type.IsNumber())
            {
                object? value = Value;

                switch (value)
                {
                    case long integer: val = integer != 0; break;
                    case double real: val = real != 0; break;
                    default: val = value != null; break;
                }
            }
            else
            {
                val = Value != null;
            }

            return new ConstantExpression(Type.Module.CorLibTypes.Boolean, val);
        }
    }
}
