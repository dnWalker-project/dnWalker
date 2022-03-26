using dnlib.DotNet.Emit;

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
    public class UnaryOperation : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, ExpressionType> _operatorLookup = new Dictionary<OpCode, ExpressionType>()
        {
            [OpCodes.Neg] = ExpressionType.Negate,
            [OpCodes.Not] = ExpressionType.Not,
        };
        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _operatorLookup.Keys;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement operand = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!operand.TryGetExpression(cur, out Expression expression))
            {
                return retValue;
            }

            IDataElement result = cur.EvalStack.Peek();

            Expression resultExpression = Expression.MakeUnary(_operatorLookup[baseExecutor.Instruction.OpCode], expression, expression.Type);
            result.SetExpression(resultExpression, cur);

            return retValue;
        }
    }
}
