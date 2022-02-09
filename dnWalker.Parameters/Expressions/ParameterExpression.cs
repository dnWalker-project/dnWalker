using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public abstract class ParameterExpression
    {
        public abstract ParameterExpressionType ExpressionType { get; }

        public static ParameterExpression Parse(ReadOnlySpan<char> exprString)
        {
            if (TryParse(exprString, out ParameterExpression? expression)) return expression;
            throw new FormatException();
        }

        public static bool TryParse(ReadOnlySpan<char> exprString, [NotNullWhen(true)] out ParameterExpression? expression)
        {
            // [N] - is null    expression
            // [L] - length of  expression
            // [V] - value of   expression
            // [E] - ref equals expression

            const char IsNullChar = 'N';
            const char LengthOfChar = 'L';
            const char ValueOfChar = 'V';
            const char RefEqualsChar = 'E';

            if (exprString.Length == 11) // unary
            {
                switch (exprString[0])
                {
                    case IsNullChar: expression = new IsNullParameterExpression(int.Parse(exprString.Slice(3, 8), NumberStyles.HexNumber)); break;
                    case LengthOfChar: expression = new LengthOfParameterExpression(int.Parse(exprString.Slice(3, 8), NumberStyles.HexNumber)); break;
                    case ValueOfChar: expression = new ValueOfParameterExpression(int.Parse(exprString.Slice(3, 8), NumberStyles.HexNumber)); break;
                    default: expression = null; break;
                }
            }
            else if (exprString.Length == 21) // binary
            {
                ParameterRef lhsOperandRef = int.Parse(exprString.Slice(3, 8), NumberStyles.HexNumber);
                ParameterRef rhsOperandRef = int.Parse(exprString.Slice(13, 8), NumberStyles.HexNumber);

                switch (exprString[0])
                {
                    case RefEqualsChar: expression = new RefEqualsParameterExpression(lhsOperandRef, rhsOperandRef); break;
                    default: expression = null; break;
                }
            }
            else
            {
                expression = null;
            }

            return expression != null;
        }

        public abstract void ApplyTo(IParameterSet ctx, object value);
    }
}
