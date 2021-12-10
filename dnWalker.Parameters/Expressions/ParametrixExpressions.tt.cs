using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dnWalker.Parameters.Expressions
{
    public abstract class ParametricExpression
    {
        public static ParametricExpression Parse(ReadOnlySpan<char> str)
        {
            if (TryParse(str, out ParametricExpression? expression))
            {
                return expression;
            }
            throw new FormatException();
        }
        
        protected abstract char Identifier { get; }

        public static bool TryParse(ReadOnlySpan<char> str, [NotNullWhen(true)] out ParametricExpression? expression)
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
                        expression = new RefEqualsParametricExpression(lhsRef, rhsRef);
                        break;
                }
            }
            else if (isUnary && ParameterReference.TryParse(str.Slice(1,8), out var operand))
            {
                switch (exprId)
                {
                    case 'V':
                        expression = new ValueOfParametricExpression(operand);
                        break;
                    case 'L':
                        expression = new LengthOfParametricExpression(operand);
                        break;
                    case 'N':
                        expression = new IsNullParametricExpression(operand);
                        break;
                }
            }

            return expression != null;
        }

        public static ValueOfParametricExpression MakeValueOf(IPrimitiveValueParameter parameter)
        {
            return new ValueOfParametricExpression(parameter.Id);
        }
        public static LengthOfParametricExpression MakeLengthOf(IArrayParameter parameter)
        {
            return new LengthOfParametricExpression(parameter.Id);
        }
        public static LengthOfParametricExpression MakeLengthOf(IAliasParameter parameter)
        {
            return new LengthOfParametricExpression(parameter.Id);
        }
        public static IsNullParametricExpression MakeIsNull(IReferenceTypeParameter parameter)
        {
            return new IsNullParametricExpression(parameter.Id);
        }
        public static IsNullParametricExpression MakeIsNull(IAliasParameter parameter)
        {
            return new IsNullParametricExpression(parameter.Id);
        }

        public static RefEqualsParametricExpression MakeRefEquals(IReferenceTypeParameter lhs, IReferenceTypeParameter rhs)
        {
            return new RefEqualsParametricExpression(lhs.Id, rhs.Id);
        }

        public static RefEqualsParametricExpression MakeRefEquals(IReferenceTypeParameter lhs, IAliasParameter rhs)
        {
            return new RefEqualsParametricExpression(lhs.Id, rhs.Id);
        }
        public static RefEqualsParametricExpression MakeRefEquals(IAliasParameter lhs, IReferenceTypeParameter rhs)
        {
            return new RefEqualsParametricExpression(lhs.Id, rhs.Id);
        }

        public static RefEqualsParametricExpression MakeRefEquals(IAliasParameter lhs, IAliasParameter rhs)
        {
            return new RefEqualsParametricExpression(lhs.Id, rhs.Id);
        }

    }

    public abstract class BinaryParametricExpression : ParametricExpression
    {
        protected BinaryParametricExpression(ParameterReference lhs, ParameterReference rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public ParameterReference Lhs { get; }
        public ParameterReference Rhs { get; }

        public override string ToString()
        {
            return $"{Identifier}{Lhs}{Rhs}";
        }
    }

    public abstract class UnaryParametricExpression : ParametricExpression
    {
        protected UnaryParametricExpression(ParameterReference operand)
        {
            Operand = operand;
        }

        public ParameterReference Operand { get; }
        
        public override string ToString()
        {
            return $"{Identifier}{Operand}";
        }
    }

    public class ValueOfParametricExpression : UnaryParametricExpression
    {
        public ValueOfParametricExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'V';

        protected override char Identifier 
        {
            get { return IdChar; }
        }
    }
    public class LengthOfParametricExpression : UnaryParametricExpression
    {
        public LengthOfParametricExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'L';

        protected override char Identifier 
        {
            get { return IdChar; }
        }
    }
    public class IsNullParametricExpression : UnaryParametricExpression
    {
        public IsNullParametricExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'N';

        protected override char Identifier 
        {
            get { return IdChar; }
        }
    }
    public class RefEqualsParametricExpression : BinaryParametricExpression
    {
        public RefEqualsParametricExpression(ParameterReference lhs, ParameterReference rhs) : base(lhs, rhs)
        {
        }

        internal static readonly char IdChar = 'R';

        protected override char Identifier 
        {
            get { return IdChar; }
        }
    }
}

