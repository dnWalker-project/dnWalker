using dnWalker.Symbolic;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Linq.Expressions;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class Contains : IMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // null reference
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Expression contains = Expression.Call(arguments[0], _contains, arguments[1]);
                cur.EvalStack.Peek().SetExpression(contains, cur);
            }
        }
    }
}
