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

        // arithmetic operators
        Add,
        Subtract,
        Multiply,
        Divide,
        Remainder,
        Negate,

        // comparison operators
        Equals,
        NotEquals,
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

                // arithmetic bin operators
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Remainder:

                // comparison bin operators
                case Operator.Equals:
                case Operator.NotEquals:
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
                case Operator.Equals:
                case Operator.NotEquals:
                case Operator.GreaterThan:
                case Operator.GreaterThanOrEqual:
                case Operator.LessThan:
                case Operator.LessThanOrEqual:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLogical(this Operator op)
        {
            switch (op)
            {
                case Operator.And:
                case Operator.Or:
                case Operator.Not:
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
                    return true;

                default:
                    return false;
            }
        }
    }
}
