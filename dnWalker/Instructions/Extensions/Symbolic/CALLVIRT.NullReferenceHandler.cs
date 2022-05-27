using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public static partial class CALLVIRT
    {
        public class NullReferenceHandler : DecisionMaker
        {
            private static readonly OpCode[] _supportedOpCodes = new[] { OpCodes.Callvirt };

            public override IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                CallInstructionExec callModel = (CallInstructionExec)baseExecutor;
                MethodDef method = callModel.Method;
                
                if (!method.IsStatic &&
                    ExpressionUtils.GetExpressions(cur, GetInstance(method, cur), out Expression instanceExpression))
                {
                    IIEReturnValue returnValue = next(baseExecutor, cur);

                    MakeDecision(cur, returnValue, (cur, edge, instance) =>
                    {
                        ExpressionFactory ef = cur.GetExpressionFactory();
                        return edge switch
                        {
                            NextEdge _ => Expression.MakeNotEqual(instanceExpression, ef.NullExpression),
                            ExceptionEdge _ => Expression.MakeEqual(instanceExpression, ef.NullExpression),
                            _ => throw new NotSupportedException("Only next edge and null reference exception edge are supported in CALLVIRT insruction.")
                        };
                    }, instanceExpression);
                
                    return returnValue;
                }

                return next(baseExecutor, cur);
            }
        }
    }
}

