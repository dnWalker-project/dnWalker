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
    public abstract partial class STELEM
    {
        public class ExceptionsHandler : STELEM
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                if (cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    ObjectReference arrayDE = (ObjectReference)cur.EvalStack.Peek(2);
                    IDataElement indexDE = cur.EvalStack.Peek(1);

                    if (ExpressionUtils.GetExpressions(cur, arrayDE, indexDE, out Expression arrayExpression, out Expression indexExpression))
                    {
                        Debug.Assert(arrayExpression is VariableExpression);

                        IIEReturnValue retValue = next(baseExecutor, cur);

                        DecisionHelper.ArrayElementAccess(cur, retValue, arrayExpression, indexExpression);

                        return retValue;
                    }
                }
                return next(baseExecutor, cur);
            }
        }
    }
}
