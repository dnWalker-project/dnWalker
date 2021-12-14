using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dnWalker.Parameters.Expressions
{
    public abstract class ParameterExpression
    {
        public static ParameterExpression Parse(ReadOnlySpan<char> str)
        {
            if (TryParse(str, out ParameterExpression? expression))
            {
                return expression;
            }
            throw new FormatException();
        }

        protected abstract char Identifier { get; }

        public static bool TryParse(ReadOnlySpan<char> str, [NotNullWhen(true)] out ParameterExpression? expression)
        {
            expression = null;

            char exprId = str[0];

            bool isBinary = str.Length == 17; // 1 + 8 + 8
            bool isUnary = str.Length == 9; // 1 + 8

            if (isBinary &&
                ParameterReference.TryParse(str.Slice(1, 8), out var lhsRef) &&
                ParameterReference.TryParse(str.Slice(9, 8), out var rhsRef))
            {
                switch (exprId)
                {
                    case 'R':
                        expression = new RefEqualsParameterExpression(lhsRef, rhsRef);
                        break;
                }
            }
            else if (isUnary && ParameterReference.TryParse(str.Slice(1, 8), out var operand))
            {
                switch (exprId)
                {
                    case 'V':
                        expression = new ValueOfParameterExpression(operand);
                        break;
                    case 'L':
                        expression = new LengthOfParameterExpression(operand);
                        break;
                    case 'N':
                        expression = new IsNullParameterExpression(operand);
                        break;
                }
            }

            return expression != null;
        }

        public abstract bool TryApplyTo(ParameterStore store, object value);


        public static ValueOfParameterExpression MakeValueOf(IPrimitiveValueParameter parameter)
        {
            return new ValueOfParameterExpression(parameter.Id);
        }
        public static LengthOfParameterExpression MakeLengthOf(IArrayParameter parameter)
        {
            return new LengthOfParameterExpression(parameter.Id);
        }
        public static LengthOfParameterExpression MakeLengthOf(IAliasParameter parameter)
        {
            return new LengthOfParameterExpression(parameter.Id);
        }
        public static IsNullParameterExpression MakeIsNull(IReferenceTypeParameter parameter)
        {
            return new IsNullParameterExpression(parameter.Id);
        }
        //public static IsNullParameterExpression MakeIsNull(IAliasParameter parameter)
        //{
        //    return new IsNullParameterExpression(parameter.Id);
        //}

        public static RefEqualsParameterExpression MakeRefEquals(IReferenceTypeParameter lhs, IReferenceTypeParameter rhs)
        {
            return new RefEqualsParameterExpression(lhs.Id, rhs.Id);
        }

        //public static RefEqualsParameterExpression MakeRefEquals(IReferenceTypeParameter lhs, IAliasParameter rhs)
        //{
        //    return new RefEqualsParameterExpression(lhs.Id, rhs.Id);
        //}
        //public static RefEqualsParameterExpression MakeRefEquals(IAliasParameter lhs, IReferenceTypeParameter rhs)
        //{
        //    return new RefEqualsParameterExpression(lhs.Id, rhs.Id);
        //}

        //public static RefEqualsParameterExpression MakeRefEquals(IAliasParameter lhs, IAliasParameter rhs)
        //{
        //    return new RefEqualsParameterExpression(lhs.Id, rhs.Id);
        //}

    }
}

