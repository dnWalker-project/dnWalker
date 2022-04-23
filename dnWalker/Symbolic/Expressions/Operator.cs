using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Remainder,
        Negate,
        ShiftLeft,
        ShiftRight,

        // comparison operators
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,

        // TODO later
        // sequence operators
        Select,
        Extract,
        Prefix,
        Suffix,
        Contains
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
                case Operator.Remainder:
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
                case Operator.Remainder:
                case Operator.Negate:
                case Operator.ShiftLeft:
                case Operator.ShiftRight:
                    return true;

                default:
                    return false;
            }
        }
    }
}
