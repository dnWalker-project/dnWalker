using dnWalker.Symbolic;

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
        private class StartsWith : IMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _startsWith = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // null reference
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Expression startsWith = Expression.Call(arguments[0], _startsWith, arguments[1]);
                cur.EvalStack.Peek(0).SetExpression(startsWith, cur);
            }
        }
    }
}
