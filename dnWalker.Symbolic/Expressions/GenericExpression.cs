using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class GenericExpression : Expression
    {
        private readonly TypeSig _type;
        private readonly string _operation;
        private readonly Expression[] _operands;

        public GenericExpression(TypeSig type, string operation, params Expression[] operands)
        {
            _type = type;
            _operation = operation;
            _operands = operands;
        }

        public override TypeSig Type => _type;

        public string Operation => _operation;

        public IReadOnlyList<Expression> Operands => _operands;

        protected internal override Expression Accept(ExpressionVisitor visitor)
        {
            return visitor.VisitGeneric(this);
        }

        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context)
        {
            return visitor.VisitGeneric(this, context);
        }
    }
}
