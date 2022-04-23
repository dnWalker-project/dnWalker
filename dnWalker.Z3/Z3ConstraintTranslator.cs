using System;
using System.Collections.Generic;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;

using Microsoft.Z3;

namespace dnWalker.Z3
{



    public class Z3ConstraintTranslator : ExpressionVisitor<Z3TranslatorContext>
    {
        public static Z3TranslatorContext Translate(Context context, IReadOnlyList<Expression> terms)
        {
            Z3TranslatorContext ctx = new Z3TranslatorContext(context);

            // gather the variables
            ICollection<IVariable> variables = VariableGatherer.GetVariables(terms);

            // initialize the trait expressions
            foreach (IVariable v in variables)
            {
                ctx.SetupTraits(v);
            }

            // translate the constraint terms
            BoolExpr[] constraints = new BoolExpr[terms.Count];
            for (int i = 0;i < constraints.Length; ++i)
            {
                Instance.Visit(terms[i], ctx);
                constraints[i] = (BoolExpr)ctx.Result;
            }

            // create a conjunction of all of the constraint terms
            // set it as a result within the Z3TranslatorContext
            ctx.Push(ctx.Context.MkAnd(constraints));

            return ctx;
        }

        public static readonly Z3ConstraintTranslator Instance = new Z3ConstraintTranslator();


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
            return stringConstantExpression;
        }

        // binary and unary operations
        public override Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression, Z3TranslatorContext state)
        {
            Operator op = binaryOperationExpression.Operator;
            Visit(binaryOperationExpression.Right, state);
            Visit(binaryOperationExpression.Left, state);

            Expr left = state.Pop();
            Expr right = state.Pop();

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

                case Operator.Equal:
                    state.Push(state.Context.MkEq(left, right));
                    break;
                case Operator.NotEqual:
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
            Expr operand = state.Pop();

            if (op == Operator.Not)
                state.Push(state.Context.MkNot((BoolExpr)operand));
            else if (op == Operator.Negate)
                state.Push(state.Context.MkUnaryMinus((ArithExpr)operand));

            return unaryOperationExpression;
        }

        // conversions
        public override Expression VisitToInteger(ToIntegerExpression toIntegerExpression, Z3TranslatorContext state)
        {
            throw new NotImplementedException();
        }

        public override Expression VisitToReal(ToRealExpression toRealExpression, Z3TranslatorContext state)
        {
            throw new NotImplementedException();
        }

        // variables and variable like expressions
        public override Expression VisitLength(LengthExpression lengthExpression, Z3TranslatorContext state)
        {
            Expression expression = lengthExpression.Expression;
            if (expression is VariableExpression varExpr)
            {
                // we want length of a variable
                state.Push(state.GetLengthTrait(varExpr.Variable));
            }
            else if (expression.Type != ExpressionType.String)
            {
                // a constant string or created by some extract operation
                Visit(expression, state);
                state.Push(state.Context.MkLength((SeqExpr)state.Pop()));
            }

            return lengthExpression;
        }

        public override Expression VisitNull(NullExpression nullExpression, Z3TranslatorContext state)
        {
            state.Push(state.NullExpr);
            return nullExpression;
        }

        public override Expression VisitLocation(LocationExpression locationExpression, Z3TranslatorContext state)
        {
            Expression expression = locationExpression.Expression;
            if (expression is VariableExpression varExpr)
            {
                // location of a variable
                state.Push(state.GetLocationTrait(varExpr.Variable));
            }
            else if (expression.Type == ExpressionType.String)
            {
                // a location of some 'extracted' or constant string
                // we get the z3 expression and create a new location constant
                Visit(locationExpression, state);
                Expr strExpr = state.Pop();
                uint id = strExpr.Id;
                Expr locExpr = state.GetConstantStringLocationTrait(id);
                state.Push(locExpr);

            }

            return locationExpression;
        }

        public override Expression VisitVariable(VariableExpression variableExpression, Z3TranslatorContext state)
        {
            // this should happen ONLY if the variable expression is of a PrimitiveValue expression
            state.Push(state.GetValueTrait(variableExpression.Variable));

            return variableExpression;
        }
    }
}


