using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

namespace dnWalker.Symbolic.Expressions
{
    public enum Operator
    {
        // logical operators
        And,
        Or,
        Not,
        Xor,

        // arithmetic operators
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Negate,

        // bit-vector operators
        ShiftLeft,
        ShiftRight,
        // BitAnd
        // BitOr
        // BitNot
        // BitXor

        // comparison operators
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,

        // TODO later
        // sequence operators
        StringSelect,
        StringExtract,
        StringPrefix,
        StringSuffix,
        StringContains,
        StringLength,
    }

    public static class OperatorExtensions
    {
        public static bool IsBinary(this Operator op)
        {
            switch (op)
            {
                // logical bin operators
                case Operator.And:
                case Operator.Or:
                case Operator.Xor:

                // arithmetic bin operators
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Modulo:
                case Operator.ShiftLeft:
                case Operator.ShiftRight:


                // comparison bin operators
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.GreaterThan:
                case Operator.GreaterThanOrEqual:
                case Operator.LessThan:
                case Operator.LessThanOrEqual:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsUnary(this Operator op)
        {
            switch (op)
            {
                case Operator.Negate:
                case Operator.Not:
                case Operator.StringLength:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsComparison(this Operator op)
        {
            switch (op)
            {
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.GreaterThan:
                case Operator.GreaterThanOrEqual:
                case Operator.LessThan:
                case Operator.LessThanOrEqual:
                    return true;
                default:
                    return false;
            }
        }

        public static Operator Negate(this Operator op)
        {
            switch (op)
            {
                case Operator.Equal: return Operator.NotEqual;
                case Operator.NotEqual: return Operator.Equal;
                case Operator.GreaterThan: return Operator.LessThanOrEqual;
                case Operator.GreaterThanOrEqual: return Operator.LessThan;
                case Operator.LessThan: return Operator.GreaterThanOrEqual;
                case Operator.LessThanOrEqual: return Operator.GreaterThan;
                default:
                    throw new NotSupportedException("Unsupported operation, cannot be negated.");
            }
        }

        public static bool IsLogical(this Operator op)
        {
            switch (op)
            {
                case Operator.And:
                case Operator.Or:
                case Operator.Not:
                case Operator.Xor:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsArithmetic(this Operator op)
        {
            switch (op)
            {
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Modulo:
                case Operator.Negate:
                case Operator.ShiftLeft:
                case Operator.ShiftRight:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsString(this Operator op)
        {
            switch (op)
            {
                case Operator.StringSelect: // select n-th character from a string
                case Operator.StringExtract: // extract a substring
                case Operator.StringPrefix: // the string starts with a prefix
                case Operator.StringSuffix: // the string ends with a suffix
                case Operator.StringLength: // the length of the string
                    return true;

                default:
                    return false;
            }
        }

        public static bool CheckOperand(this Operator op, Expression operand)
        {
            switch (op)
            {
                case Operator.Not:
                    return operand.Type.IsBoolean();
                case Operator.Negate:
                    return operand.Type.IsNumber();
                case Operator.StringLength:
                    return operand.Type.IsString();
                default:
                    return false;
            }
        }

        public static bool CheckOperands(this Operator op, Expression op1, Expression op2)
        {
            switch (op)
            {
                // logical
                case Operator.And:
                case Operator.Or:
                case Operator.Xor:
                    return op1.Type.IsBoolean() && op2.Type.IsBoolean();


                // arithmetic - do not care about the type
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Negate:
                    return op1.Type.IsNumber() && op2.Type.IsNumber();

                // arithmetic - must be integers
                case Operator.Modulo:
                case Operator.ShiftLeft:
                case Operator.ShiftRight:
                    return op1.Type.IsInteger() && op2.Type.IsInteger();

                // comparison - do not care about type
                case Operator.Equal:
                case Operator.NotEqual:
                    return true; // we can have equality between anything

                // comparison - numeric
                case Operator.GreaterThan:
                case Operator.GreaterThanOrEqual:
                case Operator.LessThan:
                case Operator.LessThanOrEqual:
                    return op1.Type.IsNumber() && op2.Type.IsNumber();

                // string operators
                case Operator.StringSelect:
                    return op1.Type.IsString() && op2.Type.IsInteger();
                case Operator.StringPrefix:
                    return op1.Type.IsString() && op2.Type.IsString();
                case Operator.StringSuffix:
                    return op1.Type.IsString() && op2.Type.IsString();
                case Operator.StringContains:
                    return op1.Type.IsString() && op2.Type.IsString();

                default:
                    return false;
            }
        }
        public static bool CheckOperands(this Operator op, Expression op1, Expression op2, Expression op3)
        {
            switch (op)
            {
                case Operator.StringExtract:
                    return 
                        op1.Type.IsString() && // the source string
                        op2.Type.IsInteger() && // offset
                        op3.Type.IsInteger(); // length

                default:
                    return false;
            }
        }
        public static bool CheckOperands(this Operator op, params Expression[] operands)
        {
            if (operands.Length == 0) return false;
            if (operands.Length == 1) return CheckOperand(op, operands[0]);
            if (operands.Length == 2) return CheckOperands(op, operands[0], operands[1]);
            if (operands.Length == 3) return CheckOperands(op, operands[0], operands[1], operands[2]);

            return false;
        }


        public static TypeSig GetResultType(this Operator op, Expression operand)
        {
            switch (op)
            {
                case Operator.Not:
                case Operator.Negate: 
                    return operand.Type;

                case Operator.StringLength: 
                    return operand.Type.Module.CorLibTypes.Int32;

                default:
                    throw new InvalidOperationException($"Invalid operator: '{op}'");
            }
        }
        public static TypeSig GetResultType(this Operator op, Expression op1, Expression op2)
        {
            switch (op)
            {
                // logical bin operators
                case Operator.And:
                case Operator.Or:
                case Operator.Xor:
                    return op1.Type;

                // arithmetic bin operators
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                    return (op1.Type.IsReal() || op2.Type.IsReal()) ? // at least one of them is a real expression?
                        op1.Type.Module.CorLibTypes.Double :
                        op1.Type.Module.CorLibTypes.Int32;

                case Operator.Modulo:
                case Operator.ShiftLeft:
                case Operator.ShiftRight:
                    return op1.Type;


                // comparison bin operators
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.GreaterThan:
                case Operator.GreaterThanOrEqual:
                case Operator.LessThan:
                case Operator.LessThanOrEqual:
                    return op1.Type.Module.CorLibTypes.Boolean;


                default:
                    throw new InvalidOperationException($"Invalid operator: '{op}'");
            }
        }

        public static TypeSig GetResultType(this Operator op, params Expression[] operands)
        {
            if (operands.Length == 0) throw new InvalidOperationException("At least one operand is necessary.");
            if (operands.Length == 1) return GetResultType(op, operands[0]);
            if (operands.Length == 2) return GetResultType(op, operands[0], operands[1]);
            if (operands.Length == 3) return GetResultType(op, operands[0], operands[1], operands[2]);

            throw new InvalidOperationException("Invalid operand count.");
        }

    }
}