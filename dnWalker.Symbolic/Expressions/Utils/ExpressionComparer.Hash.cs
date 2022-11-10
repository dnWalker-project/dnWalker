using dnlib.DotNet;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public sealed partial class ExpressionComparer
    {
        private static void BaseHash(Expression expression, ref HashCode hash)
        {
            switch (expression)
            {
                case ConstantExpression constant: 
                    ConstantHash(constant, ref hash);
                    break;

                case VariableExpression variable: 
                    VariableHash(variable, ref hash);
                    break;

                case UnaryExpression unary:
                    UnaryHash(unary, ref hash);
                    break;

                case BinaryExpression binary:
                    BinaryHash(binary, ref hash); 
                    break;

                case StringOperationExpression stringOperation:
                    StringOperationHash(stringOperation, ref hash);
                    break;

                default: 
                    throw new NotSupportedException("Unexpected expression type.");
            }

        }

        private static void StringOperationHash(StringOperationExpression str, ref HashCode hash)
        {
            foreach (Expression operand in str.Operands)
            {
                BaseHash(operand, ref hash);
            }
            hash.Add(str.Operator);
        }
        private static void BinaryHash(BinaryExpression binary, ref HashCode hash)
        {
            BaseHash(binary.Left, ref hash);
            BaseHash(binary.Right, ref hash);
            hash.Add(binary.Operator);
        }

        private static void UnaryHash(UnaryExpression unary, ref HashCode hash)
        {
            BaseHash(unary.Inner, ref hash);
            hash.Add(unary.Operator);
        }

        private static void VariableHash(VariableExpression variable, ref HashCode hash)
        {
            hash.Add(variable.Variable);
        }

        private static void ConstantHash(ConstantExpression constant, ref HashCode hash)
        {
            hash.Add(TypeEqualityComparer.Instance.GetHashCode(constant.Type));
            hash.Add(constant.Value);
        }
    }
}
