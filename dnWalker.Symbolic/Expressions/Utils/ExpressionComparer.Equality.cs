using dnlib.DotNet;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public sealed partial class ExpressionComparer
    {
        private static bool BaseEquals(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y)) return true;

            switch ((x, y))
            {
                case (ConstantExpression constX, ConstantExpression constY): 
                    return ConstantEquals(constX, constY);

                case (VariableExpression varX, VariableExpression varY): 
                    return VariableEquals(varX, varY);

                case (UnaryExpression unX, UnaryExpression unY):
                    return UnaryEquals(unX, unY);

                case (BinaryExpression binX, BinaryExpression binY):
                    return BinaryEquals(binX, binY);

                case (StringOperationExpression strX, StringOperationExpression strY):
                    return StringOperationEquals(strX, strY);

                default: return false;
            }

        }

        private static bool StringOperationEquals(StringOperationExpression strX, StringOperationExpression strY)
        {
            if (strX.Operator == strY.Operator &&
                strX.Operands.Length == strY.Operands.Length)
            {
                bool equal = true;
                for (int i = 0; i < strX.Operands.Length; ++i)
                {
                    equal &= BaseEquals(strX.Operands[i], strY.Operands[i]);
                }
                return equal;
            }
            return false;
        }

        private static bool BinaryEquals(BinaryExpression binX, BinaryExpression binY)
        {
            return binX.Operator == binY.Operator &&
                BaseEquals(binX.Left, binY.Left) &&
                BaseEquals(binX.Right, binY.Right);
        }

        private static bool UnaryEquals(UnaryExpression unX, UnaryExpression unY)
        {
            return unX.Operator == unY.Operator &&
                BaseEquals(unX.Inner, unY.Inner);
        }

        private static bool VariableEquals(VariableExpression varX, VariableExpression varY)
        {
            return varX.Variable.Equals(varY.Variable);
        }

        private static bool ConstantEquals(ConstantExpression constX, ConstantExpression constY)
        {
            return TypeEqualityComparer.Instance.Equals(constX.Type, constY.Type) &&
                Equals(constX.Value, constY.Value);
        }
    }
}
