using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class Equality : IMethodHandler
        {
            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Debug.Fail("Handling exception of method which should never throw an exception!");
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(Expression.Equal(arguments[0], arguments[1]), cur);
            }
        }

    }
}
