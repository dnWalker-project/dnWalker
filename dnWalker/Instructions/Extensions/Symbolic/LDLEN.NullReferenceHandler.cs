using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class LDLEN
    {
        public class NullReferenceHandler : LDLEN
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                IDataElement instanceDE = cur.EvalStack.Peek();

                IIEReturnValue retValue = next(baseExecutor, cur);

                if (!ExpressionUtils.GetExpressions(cur, instanceDE, out Expression arrayExpression))
                {
                    return retValue;
                }

                DecisionHelper.ThrowNullOrNext(cur, retValue, arrayExpression);

                return retValue;
            }
        }
    }
}
