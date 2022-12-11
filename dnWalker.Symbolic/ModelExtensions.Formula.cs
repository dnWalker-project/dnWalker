using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static partial class ModelExtensions
    {
        [ThreadStatic]
        private static FormulaModelVisitor _instance;// = new FormulaModelVisitor();
        private class FormulaModelVisitor : ModelVisitorBase
        {

            private readonly Dictionary<Location, IVariable> _loc2var = new Dictionary<Location, IVariable>();
            private readonly List<Expression> _terms = new List<Expression>();

            private ExpressionFactory? _expressionFactory;

            public Expression GetFormula(IReadOnlyModel model, ExpressionFactory? expressionFactory = null)
            {
                _expressionFactory = expressionFactory ?? ExpressionFactory.Default;
                _loc2var.Clear();
                _terms.Clear();

                Visit(model);

                Expression result = _terms.Count == 0 ? ExpressionFactory.Default.MakeBooleanConstant(true) : _terms.Aggregate(_expressionFactory.MakeAnd);

                _loc2var.Clear();
                _terms.Clear();
                _expressionFactory = null;

                return result;
            }


            public override void VisitVariable(IVariable variable)
            {
                Variables.Push(variable);

                if (TryGetValue(variable, out IValue? value))
                {
                    if (value is Location location && location != Location.Null)
                    {
                        if (_loc2var.TryGetValue(location, out IVariable? other))
                        {
                            // since the loc2var has already mapping for this location, we can assume it was already explored
                            // just add the equality between the variable associated with this location and the current variable
                            AddVariableEqualityTerm(variable, other);
                        }
                        else
                        {
                            // no variable is associated with this location, we will associate the current variable
                            // and we will advance the exploration forward.
                            _loc2var[location] = variable;
                            AddVariableNotNullTerm(variable);
                            if (TryGetHeapNode(location, out IReadOnlyHeapNode? nodeToVisit))
                            {
                                Visit(nodeToVisit);
                            }
                        }
                    }
                    else
                    {
                        AddConstantEqualityTerm(variable, value);
                    }
                }

                Variables.Pop();
            }

            private void AddVariableNotNullTerm(IVariable variable)
            {
                ExpressionFactory ef = _expressionFactory!;
                AddTerm(ef.MakeNotEqual(ef.MakeVariable(variable), ef.NullExpression));
            }

            private void AddVariableEqualityTerm(IVariable variable, IVariable other)
            {
                ExpressionFactory ef = _expressionFactory!;
                Expression varExpr = ef.MakeVariable(variable);

                Expression otherVarExpr = ef.MakeVariable(other);

                AddTerm(ef.MakeEqual(varExpr, otherVarExpr));
            }

            private void AddConstantEqualityTerm(IVariable variable, IValue value)
            {
                ExpressionFactory ef = _expressionFactory!;
                Expression varExpr = ef.MakeVariable(variable);
                switch (value)
                {
                    case PrimitiveValue<bool> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeBooleanConstant(v.Value))); break;
                    //TODO: chars are work in progress, duality of integer vs string
                    //case PrimitiveValue<char> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeCharConstant(v.Value)); break;
                    case PrimitiveValue<byte> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<ushort> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<uint> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<ulong> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant((long)v.Value))); break;
                    case PrimitiveValue<sbyte> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<short> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<int> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<long> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeIntegerConstant(v.Value))); break;
                    case PrimitiveValue<float> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeRealConstant(v.Value))); break;
                    case PrimitiveValue<double> v: AddTerm(ef.MakeEqual(varExpr, ef.MakeRealConstant(v.Value))); break;
                    case StringValue v: AddTerm(ef.MakeEqual(varExpr, v.Content == null ? ef.StringNullExpression : ef.MakeStringConstant(v.Content))); break;
                    // non null locations should not be here at all
                    case Location _: AddTerm(ef.MakeEqual(varExpr, ef.NullExpression)); break;
                }

            }

            private void AddTerm(Expression expression)
            {
                _terms.Add(expression);
            }
        }

        public static Expression GetFormula(this IReadOnlyModel model, ExpressionFactory? expressionFactory = null)
        {
            _instance ??= new FormulaModelVisitor();
            return _instance.GetFormula(model, expressionFactory);
        }
    }
}
