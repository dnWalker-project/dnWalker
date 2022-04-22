using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public class ExpressionWriter : ExpressionVisitor<StringBuilder>
    {
        public static readonly ExpressionWriter Instance = new ExpressionWriter();
        public static string ToString(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            Instance.Visit(expression, sb);
            return sb.ToString();
        }

        public override Expression VisitVariable(VariableExpression variableExpression, StringBuilder state)
        {
            state.Append(variableExpression.Variable.ToString());
            return variableExpression;
        }

        public override Expression VisitBooleanConstant(BooleanConstantExpression booleanConstantExpression, StringBuilder state)
        {
            state.Append(booleanConstantExpression.Value.ToString());
            return booleanConstantExpression;
        }

        public override Expression VisitLength(LengthExpression lengthExpression, StringBuilder state)
        {
            state.Append("Length(");
            Visit(lengthExpression.Expression, state);
            state.Append(')');
            return lengthExpression;
        }

        public override Expression VisitLocation(LocationExpression locationExpression, StringBuilder state)
        {
            // what is the role of LocationExpression???
            return locationExpression;
        }

        public override Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression, StringBuilder state)
        {
            state.Append('(');
            Visit(binaryOperationExpression.Left, state);
            state.Append(' ');
            state.Append(GetUnaryOrBinarySymbol(binaryOperationExpression.Operator));
            state.Append(' ');
            Visit(binaryOperationExpression.Right, state);
            state.Append(')');
            return binaryOperationExpression;

        }

        public override Expression VisitNull(NullExpression nullExpression, StringBuilder state)
        {
            state.Append(NullLiteral);
            return nullExpression;
        }

        public override Expression VisitRealConstant(RealConstantExpression realConstantExpression, StringBuilder state)
        {
            state.Append(realConstantExpression.Value);
            return realConstantExpression;
        }

        public override Expression VisitIntegerConstant(IntegerConstantExpression integerConstantExpression, StringBuilder state)
        {
            state.Append(integerConstantExpression.Value);
            return integerConstantExpression;
        }

        public override Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression, StringBuilder state)
        {
            state.Append(GetUnaryOrBinarySymbol(unaryOperationExpression.Operator));
            Visit(unaryOperationExpression.Operand, state);
            return unaryOperationExpression;
        }

        public override Expression VisitCharConstant(CharConstantExpression charConstantExpression, StringBuilder state)
        {
            state.Append('\'');
            state.Append(charConstantExpression.Value);
            state.Append('\'');
            return charConstantExpression;
        }

        public override Expression VisitStringConstant(StringConstantExpression stringConstantExpression, StringBuilder state)
        {
            state.Append('\"');
            state.Append(stringConstantExpression.Value);
            state.Append('\"');
            return stringConstantExpression;
        }

        private static string GetUnaryOrBinarySymbol(Operator op)
        {
            return op switch
            {
                Operator.And => "&",
                Operator.Or => "|",
                Operator.Not => "!",
                Operator.Add => "+",
                Operator.Subtract => "-",
                Operator.Multiply => "*",
                Operator.Divide => "/",
                Operator.Remainder => "%",
                Operator.Negate => "-",
                Operator.Equal => "=",
                Operator.NotEqual => "!=",
                Operator.GreaterThan => ">",
                Operator.GreaterThanOrEqual => ">=",
                Operator.LessThan => "<",
                Operator.LessThanOrEqual => "<=",

                _ => throw new NotSupportedException(),
            };
        }

        public override Expression VisitIntegerToReal(IntegerToRealExpression integerToRealExpression, StringBuilder state)
        {
            state.Append("ToReal(");
            Visit(integerToRealExpression.Inner, state);
            state.Append(')');
            return integerToRealExpression;
        }

        public override Expression VisitRealToInteger(RealToIntegerExpression realToIntegerExpression, StringBuilder state)
        {
            state.Append("ToInteger(");
            Visit(realToIntegerExpression.Inner, state);
            state.Append(')');
            return realToIntegerExpression;
        }

        public static readonly string NullLiteral = "null";
    }
}
