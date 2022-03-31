using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class Length : IMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _get_Length = typeof(string).GetMethod("get_Length");

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // null reference exception...
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                IDataElement lengthElement = cur.EvalStack.Peek();

                Expression lengthExpression = Expression.Call(arguments[0], _get_Length);
                lengthElement.SetExpression(lengthExpression, cur);
            }
        }
    }
}
