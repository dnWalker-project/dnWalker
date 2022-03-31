using dnWalker.Symbolic;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Linq.Expressions;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class EndsWith : IMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _endsWith = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // null reference
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Expression endsWith = Expression.Call(arguments[0], _endsWith, arguments[1]);
                cur.EvalStack.Peek(0).SetExpression(endsWith, cur);
            }
        }
    }
}
