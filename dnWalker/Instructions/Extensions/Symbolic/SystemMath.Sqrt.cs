using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public partial class SystemMath
    {
        private class Sqrt : IMethodHandler
        {
            //private static readonly MethodInfo _sqrtMethod = typeof(Math).GetMethod(nameof(Math.Sqrt), _oneDouble);

            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // the argument is negative...
                // TODO: add the constraint into the path condition
                throw new NotImplementedException();
            }

            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur)
            {
                // the argument is non-negative...
                // TODO: add the constraint into the path condition
                // how to handle non-linear constraint??

                //Expression sqrtExpression = Expression.Call(null, _sqrtMethod, arguments[0]);
                //cur.EvalStack.Peek(0).SetExpression(sqrtExpression, cur);
                throw new NotImplementedException();
            }
        }
    }
}
