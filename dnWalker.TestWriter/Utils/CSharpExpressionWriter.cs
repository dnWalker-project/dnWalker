using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Utils
{
    public class CSharpExpressionWriter : ExpressionVisitor<StringBuilder>
    {
        [ThreadStatic]
        private static readonly CSharpExpressionWriter _instance = new CSharpExpressionWriter();

        public static string GetCSharpExpression(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            _instance.Visit(expression, sb);
            return sb.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression binary, StringBuilder sb)
        {
            Expression left = binary.Left;
            Expression right = binary.Right;

            sb.Append('(');
            Visit(left, sb);
            sb.Append(' ');
            sb.Append(GetSymbol(binary.Operator));
            sb.Append(' ');
            Visit(right, sb);
            sb.Append(')');

            return binary;
        }

        protected override Expression VisitUnary(UnaryExpression unary, StringBuilder sb)
        {
            sb.Append(GetSymbol(unary.Operator));

            Visit(unary.Inner, sb);

            return unary;
        }

        protected override Expression VisitConstant(ConstantExpression constant, StringBuilder sb)
        {
            sb.Append(constant.Value?.ToString() ?? "null");

            return constant;
        }

        protected override Expression VisitVariable(VariableExpression variable, StringBuilder sb)
        {
            if (variable.Variable is not NamedVariable namedVariable) 
            {
                throw new NotSupportedException("Only named varibles are supported, for now.");
            }

            sb.Append(namedVariable.Name);

            return variable;
        }

        private static string GetSymbol(Operator op)
        {
            return op switch
            {
                Operator.And => "&&",
                Operator.Or => "||",
                Operator.Not => "!",
                Operator.Add => "+",
                Operator.Subtract => "-",
                Operator.Multiply => "*",
                Operator.Divide => "/",
                Operator.Modulo => "%",
                Operator.Negate => "-",
                Operator.Equal => "==",
                Operator.NotEqual => "!=",
                Operator.GreaterThan => ">",
                Operator.GreaterThanOrEqual => ">=",
                Operator.LessThan => "<",
                Operator.LessThanOrEqual => "<=",
                _ => throw new NotSupportedException($"Operator {op} not yet supported.")
            };
        }
    }
}
