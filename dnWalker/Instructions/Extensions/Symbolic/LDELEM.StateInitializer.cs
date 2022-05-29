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
        public class StateInitializer : LDELEM
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                if (cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    ObjectReference arrayDE = (ObjectReference)cur.EvalStack.Peek(1);
                    IDataElement indexDE = cur.EvalStack.Peek(0);
                    int idx = ((IIntegerElement)indexDE).ToInt4(false).Value;

                    if (ExpressionUtils.GetExpressions(cur, arrayDE, indexDE, out Expression arrayExpression, out Expression indexExpression))
                    {
                        Debug.Assert(arrayExpression is VariableExpression);

                        context.EnsureInitialized(Variable.ArrayElement(((VariableExpression)arrayExpression).Variable, idx), cur);
                    }
                }
                return next(baseExecutor, cur);
            }
        }
    }
}
