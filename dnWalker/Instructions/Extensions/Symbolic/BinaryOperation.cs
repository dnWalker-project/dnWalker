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
    public class BinaryOperation : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, ExpressionType> _operatorLookup = new Dictionary<OpCode, ExpressionType>()
        {
            [OpCodes.Ceq] = ExpressionType.Equal,

            [OpCodes.Cgt] = ExpressionType.GreaterThan,
            [OpCodes.Cgt_Un] = ExpressionType.GreaterThan,

            [OpCodes.Clt] = ExpressionType.LessThan,
            [OpCodes.Clt_Un] = ExpressionType.LessThan,

            [OpCodes.Add] = ExpressionType.Add,
            [OpCodes.Add_Ovf] = ExpressionType.Add, //Checked?
            [OpCodes.Add_Ovf_Un] = ExpressionType.Add, //Checked?

            [OpCodes.Div] = ExpressionType.Divide,
            [OpCodes.Div_Un] = ExpressionType.Divide,

            [OpCodes.Mul] = ExpressionType.Multiply,
            [OpCodes.Mul_Ovf] = ExpressionType.Multiply, //Checked?
            [OpCodes.Mul_Ovf_Un] = ExpressionType.Multiply, //Checked?

            [OpCodes.Rem] = ExpressionType.Modulo,
            [OpCodes.Rem_Un] = ExpressionType.Modulo,

            [OpCodes.Sub] = ExpressionType.Subtract,
            [OpCodes.Sub_Ovf] = ExpressionType.Subtract, //Checked?
            [OpCodes.Sub_Ovf_Un] = ExpressionType.Subtract, //Checked?

            // unary
            // [OpCodes.Neg] = ExpressionType.Negate,

            [OpCodes.And] = ExpressionType.And,
            //[OpCodes.And] = ExpressionType.AndAlso,
            // unary
            // [OpCodes.Not] = ExpressionType.Not,
            [OpCodes.Or] = ExpressionType.Or,
            //[OpCodes.Or] = ExpressionType.OrElse,
            [OpCodes.Xor] = ExpressionType.ExclusiveOr,
            [OpCodes.Shl] = ExpressionType.LeftShift,
            [OpCodes.Shr] = ExpressionType.RightShift,
            [OpCodes.Shr_Un] = ExpressionType.RightShift,
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
            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            IIEReturnValue retValue = next(baseExecutor, cur);

            bool lhsSymbolic = lhs.TryGetExpression(cur, out Expression lhsExpression);
            bool rhsSymbolic = rhs.TryGetExpression(cur, out Expression rhsExpression);

            if (!lhsSymbolic && !rhsSymbolic)
            {
                return retValue;
            }

            IDataElement result = cur.EvalStack.Peek();

            Expression resultExpression = Expression.MakeBinary(_operatorLookup[baseExecutor.Instruction.OpCode], lhsExpression ?? lhs.AsExpression(), rhsExpression ?? rhs.AsExpression());
            result.SetExpression(resultExpression, cur);
            
            return retValue;
        }
    }
}
