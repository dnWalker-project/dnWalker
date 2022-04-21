using System;
using System.Collections.Generic;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using Microsoft.Z3;

namespace dnWalker.Z3
{
    public class Z3TranslatorContext
    {
        private readonly Stack<Expr> _operands = new Stack<Expr>();
        private readonly List<BoolExpr> _constraints = new List<BoolExpr>();

        public Z3TranslatorContext(Context context)
        {
            Context = context;
        }

        public Context Context { get; }
        public Expr Result => _operands.Peek();
        public IReadOnlyCollection<BoolExpr> Constraints => _constraints;

        public void Push(Expr operand)
        {
            _operands.Push(operand);
        }

        public Expr Pop()
        {
            return _operands.Pop();
        }

        private readonly Dictionary<IVariable, Expr> _variableLookup = new Dictionary<IVariable, Expr>();

        public Expr GetVariable(IVariable variable)
        {
            if (!_variableLookup.TryGetValue(variable, out Expr z3))
            {
                z3 = CreateVariableExpression(variable);
                _variableLookup[variable] = z3;
            }
            return z3;
        }

        private Expr CreateVariableExpression(IVariable variable)
        {
            switch (variable.VariableType)
            {
                case VariableType.UInt8:
                    break;
                case VariableType.UInt16:
                    break;
                case VariableType.UInt32:
                    break;
                case VariableType.UInt64:
                    break;
                case VariableType.Int8:
                    break;
                case VariableType.Int16:
                    break;
                case VariableType.Int32:
                    break;
                case VariableType.Int64:
                    break;
                case VariableType.Boolean:
                    break;
                case VariableType.Char:
                    break;
                case VariableType.Single:
                    break;
                case VariableType.Double:
                    break;
                case VariableType.String:
                    break;
                case VariableType.Object:
                    break;
                case VariableType.Array:
                    break;
            }
        }

        private void CreateConstraint(IntExpr varExpr, long min, long max)
        {
            _constraints.Add(Context.MkAnd
                (
                    Context.MkGe(varExpr, Context.MkInt(min)),
                    Context.MkLe(varExpr, Context.MkInt(max))
                ));
        }
    }


    public class Z3Translator : ExpressionVisitor<Z3TranslatorContext>
    {

        // constants
        public override Expression VisitBooleanConstant(BooleanConstantExpression booleanConstantExpression, Z3TranslatorContext state)
        {
            state.Push(booleanConstantExpression.Value ? state.Context.MkTrue() : state.Context.MkFalse());
            return booleanConstantExpression;
        }
        public override Expression VisitCharConstant(CharConstantExpression charConstantExpression, Z3TranslatorContext state)
        {
            Expr expr = state.Context.MkNumeral(charConstantExpression.Value, state.Context.CharSort);
            state.Push(expr);
            return charConstantExpression;
        }
        public override Expression VisitIntegerConstant(IntegerConstantExpression integerConstantExpression, Z3TranslatorContext state)
        {
            IntNum intConst = state.Context.MkInt(integerConstantExpression.Value);
            state.Push(intConst);
            return integerConstantExpression;
        }
        public override Expression VisitRealConstant(RealConstantExpression realConstantExpression, Z3TranslatorContext state)
        {
            RationalNumber rat = RationalNumber.FromDouble(realConstantExpression.Value);
            RatNum ratConst = state.Context.MkReal((int)rat.Numerator, (int)rat.Denominator);
            state.Push(ratConst);
            return realConstantExpression;
        }
        public override Expression VisitStringConstant(StringConstantExpression stringConstantExpression, Z3TranslatorContext state)
        {
            SeqExpr str = state.Context.MkString(stringConstantExpression.Value);
            state.Push(str);
            return base.VisitStringConstant(stringConstantExpression, state);
        }

        // binary and unary operations
        public override Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression, Z3TranslatorContext state)
        {
            Operator op = binaryOperationExpression.Operator;
            Visit(binaryOperationExpression.Right, state);
            Visit(binaryOperationExpression.Left, state);

            Expr left = state.Pop<Expr>();
            Expr right = state.Pop<Expr>();

            switch (op)
            {
                case Operator.And:
                    state.Push(state.Context.MkAnd((BoolExpr)left, (BoolExpr)right));
                    break;

                case Operator.Or:
                    state.Push(state.Context.MkOr((BoolExpr)left, (BoolExpr)right));
                    break;

                case Operator.Add:
                    state.Push(state.Context.MkAdd((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.Subtract:
                    state.Push(state.Context.MkSub((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.Multiply:
                    state.Push(state.Context.MkMul((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.Divide:
                    state.Push(state.Context.MkDiv((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.Remainder:
                    state.Push(state.Context.MkMod((IntExpr)left, (IntExpr)right));
                    break;

                case Operator.Equals:
                    state.Push(state.Context.MkEq(left, right));
                    break;
                case Operator.NotEquals:
                    state.Push(state.Context.MkNot(state.Context.MkEq(left, right)));
                    break;
                case Operator.GreaterThan:
                    state.Push(state.Context.MkGt((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.GreaterThanOrEqual:
                    state.Push(state.Context.MkGe((ArithExpr)left, (ArithExpr)right));
                    break;
                case Operator.LessThan:
                    state.Push(state.Context.MkNot(state.Context.MkGe((ArithExpr)left, (ArithExpr)right)));
                    break;
                case Operator.LessThanOrEqual:
                    state.Push(state.Context.MkNot(state.Context.MkGt((ArithExpr)left, (ArithExpr)right)));
                    break;
            }

            return binaryOperationExpression;
        }

        public override Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression, Z3TranslatorContext state)
        {
            Operator op = unaryOperationExpression.Operator;
            Visit(unaryOperationExpression.Operand, state);
            Expr operand = state.Pop<Expr>();

            if (op == Operator.Not)
                state.Push(state.Context.MkNot((BoolExpr)operand));
            else if (op == Operator.Negate)
                state.Push(state.Context.MkUnaryMinus((ArithExpr)operand));

            return unaryOperationExpression;
        }

        // variables and variable like expressions
        public override Expression VisitLength(LengthExpression lengthExpression, Z3TranslatorContext state)
        {
            return base.VisitLength(lengthExpression, state);
        }

        public override Expression VisitNull(NullExpression nullExpression, Z3TranslatorContext state)
        {
            return base.VisitNull(nullExpression, state);
        }

        public override Expression VisitLocation(LocationExpression locationExpression, Z3TranslatorContext state)
        {
            return base.VisitLocation(locationExpression, state);
        }

        public override Expression VisitVariable(VariableExpression variableExpression, Z3TranslatorContext state)
        {
            return base.VisitVariable(variableExpression, state);
        }
    }
}

