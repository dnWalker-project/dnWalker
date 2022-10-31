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
                    DecisionHelper.ThrowNullOrNext(cur, returnValue, instanceExpression);
                }

                return returnValue;
            }
        }
    }
}
