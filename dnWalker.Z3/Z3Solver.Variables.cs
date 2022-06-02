using dnWalker.Symbolic;
using dnWalker.Symbolic.Variables;
using dnWalker.Symbolic.Expressions.Utils;

using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

using IVariable = dnWalker.Symbolic.IVariable;


namespace dnWalker.Z3
{
    public partial class Z3Solver
    {
        private static void SetupVariableMapping(Constraint constraint, ref SolverContext context)
        {
            foreach (IVariable variable in GatherVariables(constraint))
            {
                Expr varExpr = CreateVariableExpression(variable.Type, ref context);
                context.VariableMapping[variable] = varExpr;
            }


            static void AddVarConstraint(ArithExpr var, long min, long max, ref SolverContext context)
            {
                BoolExpr constraint = context.Z3.MkAnd(
                    context.Z3.MkGe(var, context.Z3.MkInt(min)),
                    context.Z3.MkLe(var, context.Z3.MkInt(max)));
                context.Solver.Assert(constraint);
            }

            static Expr CreateVariableExpression(TypeSig type, ref SolverContext context)
            {
                if (type.IsPrimitive)
                {
                    if (type.IsBoolean()) return context.Z3.MkBoolConst(context.Symbol());
                    if (type.IsChar()) return context.Z3.MkConst(context.Symbol(), context.Z3.CharSort);
                    if (type.IsByte())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, byte.MinValue, byte.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsUInt16())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, ushort.MinValue, ushort.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsUInt32())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, uint.MinValue, uint.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsUInt64())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, 0, long.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsSByte())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, sbyte.MinValue, sbyte.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsInt16())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, short.MinValue, short.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsInt32())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, int.MinValue, int.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsInt64())
                    {
                        ArithExpr varExpr = context.Z3.MkIntConst(context.Symbol());
                        AddVarConstraint(varExpr, long.MinValue, long.MaxValue, ref context);
                        return varExpr;
                    }
                    if (type.IsSingle()) return context.Z3.MkRealConst(context.Symbol());
                    if (type.IsDouble()) return context.Z3.MkRealConst(context.Symbol());

                    throw new NotSupportedException("Unexpected primitive type.");
                }
                else if (type.IsString())
                {
                    return context.Z3.MkConst(context.Symbol(), context.Z3.StringSort);
                }
                else
                {
                    return context.Z3.MkConst(context.Symbol(), context.LocSort);
                }
            }

            static HashSet<IVariable> GatherVariables(Constraint constraint)
            {
                HashSet<IVariable> variables = new HashSet<IVariable>();
                foreach (Term t in constraint.PureTerms)
                {
                    t.GetVariables(variables);
                }
                foreach (Term t in constraint.HeapTerms)
                {
                    t.GetVariables(variables);
                }
                return variables;
            }
        }
    }
}
