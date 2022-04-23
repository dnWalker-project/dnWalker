using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3
{
    public class Solver2 : ISolver
    {
        // remove as soon as possible
        Dictionary<string, object> ISolver.Solve(System.Linq.Expressions.Expression expression, IList<System.Linq.Expressions.ParameterExpression> parameters)
        {
            return new Solver().Solve(expression, parameters);
        }

        public SolverResult Solve(IEnumerable<Expression> constraints)
        {
            Context z3 = new Context();
            Z3TranslatorContext translatorContext = Z3ConstraintTranslator.Translate(z3, constraints.ToList());

            Expr constraint = translatorContext.Result;

            Microsoft.Z3.Solver solver = z3.MkSolver();
            solver.Assert(translatorContext.Constraints.Append((BoolExpr)constraint).ToArray());
            Microsoft.Z3.Status status = solver.Check();

            if (status == Microsoft.Z3.Status.UNSATISFIABLE)
            {
                // TODO: custom exception
                // OR make it part of the return value
                return SolverResult.Unsatisfiable;
            }
            else
            {
                List<Valuation> valuations = new List<Valuation>();

                Microsoft.Z3.Model model = solver.Model;

                foreach (IVariable variable in translatorContext.Variables)
                {
                    // based on the variable type, create the valuation
                    // TODO: currently cannot support dynamic traits (location, length etc...) - must redesign the interface
                    Expr expr = translatorContext.GetValueTrait(variable);
                    Expr resValue = model.Evaluate(expr);
                    IValue value = null;
                    switch (variable.VariableType)
                    {
                        case VariableType.UInt8:
                            value = new PrimitiveValue<byte>((byte)GetIntegerValue(resValue));
                            break;

                        case VariableType.UInt16:
                            value = new PrimitiveValue<ushort>((ushort)GetIntegerValue(resValue));
                            break;

                        case VariableType.UInt32:
                            value = new PrimitiveValue<uint>((uint)GetIntegerValue(resValue));
                            break;

                        case VariableType.UInt64:
                            value = new PrimitiveValue<ulong>((ulong)GetIntegerValue(resValue));
                            break;

                        case VariableType.Int8:
                            value = new PrimitiveValue<sbyte>((sbyte)GetIntegerValue(resValue));
                            break;

                        case VariableType.Int16:
                            value = new PrimitiveValue<short>((short)GetIntegerValue(resValue));
                            break;

                        case VariableType.Int32:
                            value = new PrimitiveValue<int>((int)GetIntegerValue(resValue));
                            break;

                        case VariableType.Int64:
                            value = new PrimitiveValue<long>(GetIntegerValue(resValue));
                            break;

                        case VariableType.Boolean:
                            value = new PrimitiveValue<bool>(GetBooleanValue(resValue));
                            break;

                        case VariableType.Char:
                            value = new PrimitiveValue<char>(GetCharValue(resValue));
                            break;

                        case VariableType.Single:
                            value = new PrimitiveValue<float>((float)GetRealValue(resValue));
                            break;

                        case VariableType.Double:
                            value = new PrimitiveValue<double>(GetRealValue(resValue));
                            break;



                        case VariableType.String:
                            throw new NotImplementedException();
                        case VariableType.Object:
                            throw new NotImplementedException();
                        case VariableType.Array:
                            throw new NotImplementedException();
                        default:
                            throw new NotSupportedException("Unexpected variable type!");
                    }

                    valuations.Add(new Valuation(variable, value));
                }

                return SolverResult.Satisfiable(valuations);
            }
        }

        private static char GetCharValue(Expr expr)
        {
            throw new NotImplementedException();
        }

        private static long GetIntegerValue(Expr expr)
        {
            if (expr is IntNum num) return num.Int64;
            if (expr is IntExpr) return 0;
            throw new ArgumentException("Provided expression is not an integer number kind.");
        }
        private static bool GetBooleanValue(Expr expr)
        {
            return expr.IsTrue;
        }
        private static double GetRealValue(Expr expr)
        {
            if (expr is RatNum num) return num.Double;
            if (expr is RealExpr) return 0;
            throw new ArgumentException("Provided expression is not a real number kind.");
        }
    }
}
