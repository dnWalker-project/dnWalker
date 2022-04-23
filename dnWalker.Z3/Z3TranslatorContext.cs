using System.Collections.Generic;

using dnWalker.Symbolic;

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
}

