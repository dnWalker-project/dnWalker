using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public class ExpressionWriter : ExpressionVisitor<StringBuilder>
    {
        public static readonly ExpressionWriter Instance = new ExpressionWriter();
        public static readonly string NullLiteral = "null";

        public static string ToString(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            Instance.Visit(expression, sb);
            return sb.ToString();
        }

        public override Expression Visit(Expression expression, StringBuilder sb)
        {
            return base.Visit(expression, sb);
        }

        protected internal override Expression VisitBinary(BinaryExpression binary, StringBuilder sb)
        {
            sb.Append('(');
            Visit(binary.Left, sb);
            sb.Append(' ');
            sb.Append(GetUnaryOrBinarySymbol(binary.Operator));
            sb.Append(' ');
            Visit(binary.Right, sb);
            sb.Append(')');
            return binary;
        }

        protected internal override Expression VisitUnary(UnaryExpression unary, StringBuilder sb)
        {
            Operator op = unary.Operator;
            sb.Append(GetUnaryOrBinarySymbol(op));
            Visit(unary.Inner, sb);
            return unary;
        }

        protected internal override Expression VisitStringOperation(StringOperationExpression stringOperation, StringBuilder sb)
        {
            Expression[] operands = stringOperation.Operands;
            Operator op = stringOperation.Operator;
            switch (op)
            {
                case Operator.StringLength:
                    Visit(operands[0], sb);
                    sb.Append(".Length");
                    break;

                case Operator.StringSelect:
                    Visit(operands[0], sb);
                    sb.Append('[');
                    Visit(operands[1], sb);
                    sb.Append (']');
                    break;

                case Operator.StringExtract:
                    Visit(operands[0], sb);
                    sb.Append(".Substring(");
                    Visit(operands[1], sb);
                    sb.Append(',');
                    Visit(operands[2], sb);
                    sb.Append(')');
                    break;

                case Operator.StringPrefix:
                    Visit(operands[0], sb);
                    sb.Append(".StartsWith(");
                    Visit(operands[1], sb);
                    sb.Append(')');
                    break;

                case Operator.StringSuffix:
                    Visit(operands[0], sb);
                    sb.Append(".EndsWith(");
                    Visit(operands[1], sb);
                    sb.Append(')');
                    break;

                case Operator.StringContains:
                    Visit(operands[0], sb);
                    sb.Append(".Contains(");
                    Visit(operands[1], sb);
                    sb.Append(')');
                    break;

            }
            return stringOperation;
        }

        protected internal override Expression VisitVariable(VariableExpression variable, StringBuilder sb)
        {
            sb.Append(variable.Variable.Name);
            return variable;
        }

        protected internal override Expression VisitConstant(ConstantExpression constant, StringBuilder sb)
        {
            object? value = constant.Value;
            if (value is null) sb.Append(NullLiteral);
            else if (value is String str) sb.Append($"\"{str}\"");
            else if (value is IFormattable form) sb.Append(form.ToString(null, CultureInfo.InvariantCulture));
            else sb.Append(value.ToString());

            return constant;
        }

        private static string GetUnaryOrBinarySymbol(Operator op)
        {
            return op switch
            {
                Operator.And => "&",
                Operator.Or => "|",
                Operator.Xor => "^",
                Operator.Not => "!",

                Operator.Add => "+",
                Operator.Subtract => "-",
                Operator.Multiply => "*",
                Operator.Divide => "/",
                Operator.Modulo => "%",
                Operator.ShiftLeft => "<<",
                Operator.ShiftRight => ">>",


                Operator.Negate => "-",
                Operator.Equal => "==",
                Operator.NotEqual => "!=",
                Operator.GreaterThan => ">",
                Operator.GreaterThanOrEqual => ">=",
                Operator.LessThan => "<",
                Operator.LessThanOrEqual => "<=",

                _ => throw new NotSupportedException(),
            };
        }
    }
}
