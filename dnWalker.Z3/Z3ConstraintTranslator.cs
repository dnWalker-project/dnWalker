using System;
using System.Collections.Generic;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;

using Microsoft.Z3;

namespace dnWalker.Z3
{
    public class Z3TranslatorContext
    {
        private readonly Stack<Expr> _operands = new Stack<Expr>();
        private readonly List<BoolExpr> _constraints = new List<BoolExpr>();
        private readonly HashSet<IVariable> _variables = new HashSet<IVariable>();

        private readonly Dictionary<IVariable, Expr> _valueTraits = new Dictionary<IVariable, Expr>();
        private readonly Dictionary<IVariable, IntExpr> _lengthTraits = new Dictionary<IVariable, IntExpr>();
        private readonly Dictionary<IVariable, IntExpr> _locationTraits = new Dictionary<IVariable, IntExpr>();
        private readonly Dictionary<uint, IntExpr> _constStringLocationTraits = new Dictionary<uint, IntExpr>();

        public Z3TranslatorContext(Context context)
        {
            Context = context;
            NullExpr = Context.MkInt(0);
        }

        public IReadOnlyCollection<IVariable> Variables => _variables;


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

        private int _lastTraitId = 0;

        private Symbol GetFreeSymbol()
        {
            return Context.MkSymbol(_lastTraitId++);
        }

        public Expr GetValueTrait(IVariable variable) => _valueTraits[variable];

        public IntExpr GetLengthTrait(IVariable variable) => _lengthTraits[variable];

        public IntExpr GetLocationTrait(IVariable variable) => _locationTraits[variable];

        public void SetupTraits(IVariable variable)
        {
            if (!_variables.Add(variable))
            {
                // already added => skip
                return;
            }

            switch (variable.VariableType)
            {
                case VariableType.UInt8:
                    SetupIntegerVariable(variable, byte.MinValue, byte.MaxValue);
                    break;
                case VariableType.UInt16:
                    SetupIntegerVariable(variable, ushort.MinValue, ushort.MaxValue);
                    break;
                case VariableType.UInt32:
                    SetupIntegerVariable(variable, uint.MinValue, uint.MaxValue);
                    break;
                case VariableType.UInt64:
                    SetupIntegerVariable(variable, long.MinValue, long.MaxValue); // we do not use ulong.MaxValue !!!
                    break;
                case VariableType.Int8:
                    SetupIntegerVariable(variable, sbyte.MinValue, sbyte.MaxValue);
                    break;
                case VariableType.Int16:
                    SetupIntegerVariable(variable, short.MinValue, short.MaxValue);
                    break;
                case VariableType.Int32:
                    SetupIntegerVariable(variable, int.MinValue, int.MaxValue);
                    break;
                case VariableType.Int64:
                    SetupIntegerVariable(variable, long.MinValue, long.MaxValue);
                    break;
                case VariableType.Boolean:
                    SetupBooleanVariable(variable);
                    break;
                case VariableType.Char:
                    SetupCharVariable(variable);
                    break;
                case VariableType.Single:
                case VariableType.Double:
                    SetupRealVariable(variable);
                    break;
                case VariableType.String:
                    SetupStringVariable(variable);
                    break;
                case VariableType.Object:
                    SetupObjectVariable(variable);
                    break;
                case VariableType.Array:
                    SetupArrayVariable(variable);
                    break;
            }
        }

        private void SetupArrayVariable(IVariable variable)
        {
            if (!_locationTraits.ContainsKey(variable))
            {
                IntExpr locExpr = Context.MkIntConst(GetFreeSymbol());
                _locationTraits[variable] = locExpr;
                _valueTraits[variable] = locExpr;

                _constraints.Add(CreateIntConstraint(locExpr, 0, long.MaxValue));

                IntExpr lengthExpr = Context.MkIntConst(GetFreeSymbol());
                _lengthTraits[variable] = lengthExpr;

                _constraints.Add(CreateIntConstraint(lengthExpr, 0, long.MaxValue));

                BoolExpr constraint = Context.MkImplies(
                    Context.MkEq(locExpr, NullExpr),  // location == 0 (is null
                    Context.MkEq(lengthExpr, Context.MkInt(0))     // implies length == 0
                    );
                _constraints.Add(constraint);
            }
        }

        private void SetupObjectVariable(IVariable variable)
        {
            if (!_locationTraits.ContainsKey(variable))
            {
                IntExpr locExpr = Context.MkIntConst(GetFreeSymbol());
                _locationTraits[variable] = locExpr;
                _valueTraits[variable] = locExpr;
                _constraints.Add(CreateIntConstraint(locExpr, 0, long.MaxValue));
            }
        }

        private void SetupStringVariable(IVariable variable)
        {
            if (!_valueTraits.ContainsKey(variable))
            {
                SeqExpr strExpr = (SeqExpr) Context.MkConst(GetFreeSymbol(), Context.StringSort);

                _valueTraits[variable] = strExpr;

                IntExpr locExpr = Context.MkIntConst(GetFreeSymbol());
                _locationTraits[variable] = locExpr;
                _constraints.Add(CreateIntConstraint(locExpr, 0, long.MaxValue));

                _lengthTraits[variable] = Context.MkLength(strExpr);

                BoolExpr constraint = Context.MkImplies(
                    Context.MkEq(_locationTraits[variable], Context.MkInt(0)),  // location == 0 (is null
                    Context.MkEq(_lengthTraits[variable], Context.MkInt(0))     // implies length == 0
                    );
                _constraints.Add(constraint);
            }
        }

        private void SetupRealVariable(IVariable variable)
        {
            if (!_valueTraits.ContainsKey(variable))
            {
                _valueTraits[variable] = Context.MkRealConst(GetFreeSymbol());
            }
        }

        private void SetupCharVariable(IVariable variable)
        {
            if (!_valueTraits.ContainsKey(variable))
            {
                _valueTraits[variable] = Context.MkConst(GetFreeSymbol(), Context.CharSort);
            }
        }

        private void SetupIntegerVariable(IVariable variable, long min, long max)
        {
            if (!_valueTraits.ContainsKey(variable))
            {
                IntExpr z3Var = Context.MkIntConst(GetFreeSymbol());
                _valueTraits[variable] = z3Var;
                _constraints.Add(CreateIntConstraint(z3Var, min, max));
            }
        }

        private BoolExpr CreateIntConstraint(IntExpr varExpr, long min, long max)
        {
            return Context.MkAnd
                    (
                        Context.MkGe(varExpr, Context.MkInt(min)),
                        Context.MkLe(varExpr, Context.MkInt(max))
                    );
        }

        private void SetupBooleanVariable(IVariable variable)
        {
            if (!_valueTraits.ContainsKey(variable))
            {
                _valueTraits[variable] = Context.MkBoolConst(GetFreeSymbol());
            }
        }

        public IntExpr GetConstantStringLocationTrait(uint id)
        {
            if (!_constStringLocationTraits.TryGetValue(id, out IntExpr loc))
            {
                loc = Context.MkIntConst(GetFreeSymbol());
                _constraints.Add(Context.MkGt(loc, NullExpr));
                _constStringLocationTraits[id] = loc;
            }
            return loc;
        }

        public IntExpr NullExpr { get; }

    }



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

