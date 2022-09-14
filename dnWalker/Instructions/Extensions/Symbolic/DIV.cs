using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class DIV : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new[]
        {
            OpCodes.Div,
            OpCodes.Div_Un
        };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            INumericElement divider = (INumericElement)cur.EvalStack.Peek();

            IIEReturnValue returnValue = next(baseExecutor, cur);

            if (divider is not IIntegerElement)
            {
                return returnValue;
            }

            if (ExpressionUtils.GetExpressions(cur, divider, out Expression dividerExpression))
            {
                DecisionHelper.ThrowZeroOrNext(cur, returnValue, dividerExpression);
                //DecisionHelper.MakeDecision(cur, returnValue, static (cur, edge, dividerExpression) =>
                //{
                //    return edge switch
                //    {
                //        NextEdge => Expression.MakeNotEqual(dividerExpression, cur.GetExpressionFactory().MakeIntegerConstant(0)),
                //        ExceptionEdge => Expression.MakeEqual(dividerExpression, cur.GetExpressionFactory().MakeIntegerConstant(0)),
                //        _ => throw new NotSupportedException()
                //    };
                //}, dividerExpression);
            }

            return returnValue;
        }
    }
}
