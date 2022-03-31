using dnWalker.Symbolic;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Linq.Expressions;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemString
    {
        private class Substring : IMethodHandler
        {
            // defined for offset only & offset + length, handle the latter only
            private static readonly System.Reflection.MethodInfo _substring = typeof(string).GetMethod("Substring", new Type[] { typeof(int), typeof(int) });

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // null reference, out of range
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                Expression theString = arguments[0];
                Expression offset = arguments[1];
                Expression length = arguments[2];


                Expression substring = Expression.Call(theString, _substring, offset, length);
                cur.EvalStack.Peek().SetExpression(substring, cur);
            }
        }
    }
}
