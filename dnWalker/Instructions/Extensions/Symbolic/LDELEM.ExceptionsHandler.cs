using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class LDELEM
    {
        public class ExceptionsHandler : LDELEM
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                if (cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    ObjectReference arrayDE = (ObjectReference)cur.EvalStack.Peek(1).ResolvePointer();
                    IDataElement indexDE = cur.EvalStack.Peek(0);
                    
                    if (ExpressionUtils.GetExpressions(cur, arrayDE, indexDE, out Expression arrayExpression, out Expression indexExpression))
                    {
                        Debug.Assert(arrayExpression is VariableExpression);

                        IIEReturnValue retValue = next(baseExecutor, cur);

                        ConstraintsHelper.MakeDecision(cur, retValue, (cur, edge, array, index) =>
                        {
                            ExpressionFactory ef = cur.GetExpressionFactory();
                            IVariable arrayVariable = ((VariableExpression)array).Variable;
                            return edge switch
                            {
                                NextEdge => Expression.MakeAnd(
                                                Expression.MakeNotEqual(array, ef.NullExpression),
                                                Expression.MakeAnd(
                                                    Expression.MakeGreaterThanOrEqual(index, ef.MakeIntegerConstant(0)),
                                                    Expression.MakeLessThan(index, Expression.MakeVariable(Variable.ArrayLength(arrayVariable)))
                                                    )),
                                ExceptionEdge { ExceptionType.FullName: "System.NullReferenceException" } => Expression.MakeEqual(array, ef.NullExpression),
                                ExceptionEdge { ExceptionType.FullName: "System.IndexOutOfRangeException" } => Expression.MakeAnd(
                                                Expression.MakeNotEqual(array, ef.NullExpression),
                                                Expression.MakeOr(
                                                    Expression.MakeLessThan(index, ef.MakeIntegerConstant(0)),
                                                    Expression.MakeGreaterThanOrEqual(index, Expression.MakeVariable(Variable.ArrayLength(arrayVariable)))
                                                    )),
                                _ => throw new NotSupportedException("Only next edge and exception edge with NullReference and IndexOutOfRange are supported by LDELEM.")
                            };
                        }, arrayExpression, indexExpression);

                        return retValue;
                    }
                }
                return next(baseExecutor, cur);
            }
        }
    }
}
