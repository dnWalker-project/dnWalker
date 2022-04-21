using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class VariableExpression : Expression
    {
        internal VariableExpression(IVariable variable)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            Type = GetExpressionType(variable.VariableType);
        }

        private ExpressionType GetExpressionType(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.UInt8:
                case VariableType.UInt16:
                case VariableType.UInt32:
                case VariableType.UInt64:
                case VariableType.Int8:
                case VariableType.Int16:
                case VariableType.Int32:
                case VariableType.Int64:
                    return ExpressionType.Integer;

                case VariableType.Boolean:
                    return ExpressionType.Boolean;

                case VariableType.Char:
                    return ExpressionType.Char;

                case VariableType.Single:
                case VariableType.Double:
                    return ExpressionType.Real;

                // TODO: resolve how to handle VariableType.String
                case VariableType.String:
                    return ExpressionType.String;

                case VariableType.Object:
                case VariableType.Array:
                    return ExpressionType.Location;

                default:
                    throw new NotSupportedException("Unexpected variable type.");
            }
        }

        public IVariable Variable { get; }
        public override ExpressionType Type { get; }

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitVariable(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitVariable(this, state);
    }
}
