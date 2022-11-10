using dnlib.DotNet;
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

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class LDFLD
    {
        public class StateInitializer : LDFLD
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                if (cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    ObjectReference instanceDE = (ObjectReference)cur.EvalStack.Peek();

                    if (ExpressionUtils.GetExpressions(cur, instanceDE, out Expression instanceExpression))
                    {
                        Debug.Assert(instanceExpression is VariableExpression);

                        context.EnsureInitialized(Variable.InstanceField(((VariableExpression)instanceExpression).Variable, (IField)baseExecutor.Operand), cur);
                    }
                }

                return next(baseExecutor, cur);
            }
        }
    }
}
