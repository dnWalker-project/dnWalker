using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class STFLD
    {
        public class NullReferenceHandler : STFLD
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                IDataElement instance = cur.EvalStack.Peek(1).ResolvePointer();

                IIEReturnValue returnValue = next(baseExecutor, cur);

                if (ExpressionUtils.GetExpressions(cur, instance, out Expression instanceExpression))
                {
                    DecisionMaker.MakeDecision(cur, returnValue, static (cur, edge, instance) =>
                    {
                        Expression nullExpression = cur.GetExpressionFactory().NullExpression;
                        return edge switch
                        {
                            NextEdge => Expression.MakeNotEqual(instance, nullExpression),
                            ExceptionEdge { ExceptionType.FullName : "System.NullReferenceException" } => Expression.MakeEqual(instance, nullExpression),
                            _ => throw new NotSupportedException("Only next instruction or null reference exception edges are supported for STFLD instruction.")
                        };
                    }, instanceExpression);
                }

                return returnValue;
            }
        }
    }
}
