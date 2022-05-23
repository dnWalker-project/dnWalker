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
    public class LDLEN : DecisionMaker
    {
        private static readonly OpCode[] _supportedOpCodes = new[] { OpCodes.Ldlen };

        public override IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedOpCodes;
            }
        }

        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement instanceDE = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!ExpressionUtils.GetExpressions(cur, instanceDE, out Expression arrayExpression))
            {
                return retValue;
            }

            IVariable arrayVariable = ((VariableExpression)arrayExpression).Variable;

            // make decision
            MakeDecision(cur, retValue, static (cur, edge, variable) =>
            {
                return edge switch
                {
                    NextEdge => Expression.MakeNotEqual(Expression.MakeVariable(variable), cur.GetExpressionFactory().NullExpression),
                    ExceptionEdge => Expression.MakeEqual(Expression.MakeVariable(variable), cur.GetExpressionFactory().NullExpression),
                    _ => throw new NotSupportedException("Invalid edge, only next or null ref exception are available for LDLEN.")
                };
            }, arrayVariable);

            if (retValue == InstructionExecBase.nextRetval)
            {
                IDataElement lengthDE = cur.EvalStack.Peek();
                Expression lengthExpression = Expression.MakeVariable(Variable.ArrayLength(arrayVariable));
                lengthDE.SetExpression(cur, lengthExpression);
            }

            return retValue;
        }
    }
}
