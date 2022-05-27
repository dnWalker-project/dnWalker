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
    public static partial class STFLD
    {
        public class NullReferenceHandler : DecisionMaker
        {
            private static readonly OpCode[] _supportedOpCodes = new[] { OpCodes.Stfld };

            public override IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                IDataElement instance = cur.EvalStack.Peek(1).ResolvePointer();

                IIEReturnValue returnVaalue = next(baseExecutor, cur);

                if (ExpressionUtils.GetExpressions(cur, instance, out Expression instanceExpression))
                {
                    MakeDecision(cur, returnVaalue, static (cur, edge, instance) =>
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

                return returnVaalue;
            }
        }
    }
}
