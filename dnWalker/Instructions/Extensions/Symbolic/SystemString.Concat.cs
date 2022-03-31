using dnWalker.Symbolic;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class Concat : IMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _concat = typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) });

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // should not be invoked
                Debug.Fail("Handling exception of method which should never throw an exception!");
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Expression concat = Expression.Call(null, _concat, arguments);
                cur.EvalStack.Peek().SetExpression(concat, cur);
            }
        }
    }
}
