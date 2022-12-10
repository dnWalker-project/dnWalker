using System;
using System.Collections.Generic;
using System.Diagnostics;

using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;


using Microsoft.Z3;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Z3
{
    public class Z3ExpressionTranslator : ExpressionVisitor
    {
        private readonly Context _z3;
        private readonly Dictionary<IVariable, Expr> _variableLookup;
        private readonly Stack<Expr> _operands;
        private readonly Expr _null;
        private readonly Expr _stringNull;

        public Z3ExpressionTranslator(Context z3, Dictionary<IVariable, Expr> variableLookup, Expr nullExpr, Expr stringNullExpr)
        {
            _z3 = z3 ?? throw new ArgumentNullException(nameof(z3));
            _variableLookup = variableLookup ?? throw new ArgumentNullException(nameof(variableLookup));
            _operands = new Stack<Expr>();
            _null = nullExpr ?? throw new ArgumentNullException(nameof(nullExpr));
            _stringNull = stringNullExpr ?? throw new ArgumentNullException(nameof(stringNullExpr));
        }

        public Expr Translate(Expression expression)
        {
            Debug.Assert(_operands.Count == 0);

            Visit(expression);
            return _operands.Pop();
        }


        // basic logic & arithmetic
        protected override Expression VisitConstant(ConstantExpression constant)
        {
            TypeSig type = constant.Type;
            object? value = constant.Value;

            if (type.IsObject() && value == null)
            {
                _operands.Push(_null);
            }
            else if (type.IsString())
            {
                string? strValue = value as string;
                if (strValue == null) _operands.Push(_stringNull);
                else _operands.Push(_z3.MkString(strValue));
            }
            else if (type.IsInteger())
            {
                Debug.Assert(value != null);
                long intValue = (long)value;
                _operands.Push(_z3.MkInt(intValue));
            }
            else if (type.IsReal())
            {
                Debug.Assert(value != null);
                double dblValue = (double)value;
                RationalNumber ratValue = RationalNumber.FromDouble(dblValue);
                _operands.Push(_z3.MkReal(ratValue.Numerator, ratValue.Denominator));
            }
            else if (type.IsBoolean())
            {
                Debug.Assert(value != null);
                bool boolValue = (bool)value;
                _operands.Push(_z3.MkBool(boolValue));
            }

            return constant;
        }

        protected override Expression VisitVariable(VariableExpression variable)
        {
            _operands.Push(_variableLookup[variable.Variable]);
            return variable;
        }
        protected override Expression VisitBinary(BinaryExpression binary)
        {
            Visit(binary.Right);
            Visit(binary.Left);

            Expr left = _operands.Pop();
            Expr right = _operands.Pop();

            switch (binary.Operator)
            {
                case Operator.And:
                    _operands.Push(_z3.MkAnd((BoolExpr)left, (BoolExpr)right));
                    break;

                case Operator.Or:
                    _operands.Push(_z3.MkOr((BoolExpr)left, (BoolExpr)right));
                    break;

                case Operator.Xor:
                    _operands.Push(_z3.MkXor((BoolExpr)left, (BoolExpr)right));
                    break;

                case Operator.Add:
                    _operands.Push(_z3.MkAdd((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.Subtract:
                    _operands.Push(_z3.MkSub((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.Multiply:
                    _operands.Push(_z3.MkMul((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.Divide:
                    _operands.Push(_z3.MkDiv((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.Modulo:
                    _operands.Push(_z3.MkMod((IntExpr)left, (IntExpr)right));
                    break;

                case Operator.Equal:
                    _operands.Push(_z3.MkEq(left, right));
                    break;

                case Operator.NotEqual:
                    _operands.Push(_z3.MkNot(_z3.MkEq(left, right)));
                    break;

                case Operator.GreaterThan:
                    _operands.Push(_z3.MkGt((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.GreaterThanOrEqual:
                    _operands.Push(_z3.MkGe((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.LessThan:
                    _operands.Push(_z3.MkLt((ArithExpr)left, (ArithExpr)right));
                    break;

                case Operator.LessThanOrEqual:
                    _operands.Push(_z3.MkLe((ArithExpr)left, (ArithExpr)right));
                    break;
            }

            return binary;
        }
        protected override Expression VisitUnary(UnaryExpression unary)
        {
            Visit(unary.Inner);
            Expr operand = _operands.Pop();

            switch (unary.Operator)
            {
                case Operator.Not:
                    _operands.Push(_z3.MkNot((BoolExpr)operand));
                    break;

                case Operator.Negate:
                    _operands.Push(_z3.MkUnaryMinus((ArithExpr)operand));
                    break;
            }

            return unary;
        }

        // string operations
        protected override Expression VisitStringOperation(StringOperationExpression stringOperation)
        {
            // we need to visit the operands in inversed order
            Expression[] operands = stringOperation.Operands;
            for (int i = operands.Length - 1; i >= 0; --i)
            {
                Visit(operands[i]);
            }

            Expr[] z3Ops = new Expr[operands.Length];
            for (int i = 0; i < z3Ops.Length; ++i)
            {
                z3Ops[i] = _operands.Pop();
            }

            switch (stringOperation.Operator)
            {
                case Operator.StringPrefix:
                    _operands.Push(_z3.MkPrefixOf((SeqExpr)z3Ops[0], (SeqExpr)z3Ops[1]));
                    break;

                case Operator.StringSuffix:
                    _operands.Push(_z3.MkSuffixOf((SeqExpr)z3Ops[0], (SeqExpr)z3Ops[1]));
                    break;

                case Operator.StringLength:
                    _operands.Push(_z3.MkLength((SeqExpr)z3Ops[0]));
                    break;

                case Operator.StringContains:
                    _operands.Push(_z3.MkContains((SeqExpr)z3Ops[1], (SeqExpr)z3Ops[0]));
                    break;

                case Operator.StringExtract:
                    _operands.Push(_z3.MkExtract((SeqExpr)z3Ops[0], (IntExpr)z3Ops[1], (IntExpr)z3Ops[2]));
                    break;

                case Operator.StringSelect:
                    _operands.Push(_z3.MkExtract((SeqExpr)z3Ops[0], (IntExpr)z3Ops[1], _z3.MkInt(1)));
                    break;
            }

            return stringOperation;
        }
    }
}


