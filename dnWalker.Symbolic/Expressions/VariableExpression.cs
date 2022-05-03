using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class VariableExpression : Expression
    {
        public override TypeSig Type => Variable.Type;

        public IVariable Variable { get; }

        internal VariableExpression(IVariable variable)
        {
            Variable = variable;
        }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitVariable(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitVariable(this, context);
    }
}
