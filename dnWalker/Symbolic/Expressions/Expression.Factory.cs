using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class Expression
    {
        public static Expression MakeBinary(Operator op, Expression left, Expression right)
        {
            return new BinaryOperationExpression(op, left, right);
        }
        public static Expression MakeUnary(Operator op, Expression operand)
        {
            return new UnaryOperationExpression(op, operand);
        }

        public static Expression And(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.And, left, right);
        }
        public static Expression Or(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Or, left, right);
        }
        public static Expression Not(Expression operand)
        {
            return new UnaryOperationExpression(Operator.Not, operand);
        }


        public static Expression Add(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Add, left, right);
        }
        public static Expression Subtract(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Subtract, left, right);
        }
        public static Expression Multiply(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Multiply, left, right);
        }
        public static Expression Divide(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Divide, left, right);
        }
        public static Expression Remainder(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Remainder, left, right);
        }
        public static Expression Negate(Expression operand)
        {
            return new UnaryOperationExpression(Operator.Negate, operand);
        }

        public static Expression Equals(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.Equal, left, right);
        }
        public static Expression NotEquals(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.NotEqual, left, right);
        }
        public static Expression GreaterThan(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.GreaterThan, left, right);
        }
        public static Expression GreaterThanOrEqual(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.GreaterThanOrEqual, left, right);
        }
        public static Expression LessThan(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.LessThan, left, right);
        }
        public static Expression LessThanOrEqual(Expression left, Expression right)
        {
            return new BinaryOperationExpression(Operator.LessThanOrEqual, left, right);
        }

        public static Expression Constant(bool value)
        {
            return value ? True : False;
        }

        private static readonly Dictionary<long, Expression> _intConstants = new Dictionary<long, Expression>
        {
            [0] = IntegerZero
        };
        private static readonly Dictionary<char, Expression> _charConstants = new Dictionary<char, Expression>();
        private static readonly Dictionary<string, Expression> _stringConstants = new Dictionary<string, Expression>();
        private static readonly Dictionary<double, Expression> _realConstants = new Dictionary<double, Expression>
        {
            [0] = RealZero
        };

        public static Expression Constant(long value)
        {
            if (!_intConstants.TryGetValue(value, out var result))
            {
                result = new IntegerConstantExpression(value);
                _intConstants[value] = result;
            }
            return result;
        }
        public static Expression Constant(char value)
        {
            if (!_charConstants.TryGetValue(value, out var result))
            {
                result = new IntegerConstantExpression(value);
                _charConstants[value] = result;
            }
            return result;
        }
        public static Expression Constant(string value)
        {
            if (!_stringConstants.TryGetValue(value, out var result))
            {
                result = new StringConstantExpression(value);
                _stringConstants[value] = result;
            }
            return result;
        }
        public static Expression Constant(double value)
        {
            if (!_realConstants.TryGetValue(value, out var result))
            {
                result = new RealConstantExpression(value);
                _realConstants[value] = result;
            }
            return result;
        }

        private static readonly Dictionary<IVariable, Expression> _variables = new Dictionary<IVariable, Expression>();
        public static Expression Variable(IVariable variable)
        {
            if (!_variables.TryGetValue(variable, out var result))
            {
                result = new VariableExpression(variable);
                _variables[variable] = result;
            }
            return result;
        }

        public static Expression Length(Expression stringOrArray)
        {
            return new LengthExpression(stringOrArray);
        }
        public static Expression Location(Expression stringOrArrayOrObject)
        {
            return new LocationExpression(stringOrArrayOrObject);
        }


        public static Expression IntegerToReal(Expression expression)
        {
            return new IntegerToRealExpression(expression);
        }
        public static Expression RealToInteger(Expression expression)
        {
            return new RealToIntegerExpression(expression);
        }
    }
}
