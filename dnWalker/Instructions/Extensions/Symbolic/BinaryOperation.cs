using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class BinaryOperation : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, Operator> _operatorLookup = new Dictionary<OpCode, Operator>()
        {
            [OpCodes.Ceq] = Operator.Equal,

            [OpCodes.Cgt] = Operator.GreaterThan,
            [OpCodes.Cgt_Un] = Operator.GreaterThan,

            [OpCodes.Clt] = Operator.LessThan,
            [OpCodes.Clt_Un] = Operator.LessThan,

            [OpCodes.Add] = Operator.Add,
            [OpCodes.Add_Ovf] = Operator.Add, //Checked?
            [OpCodes.Add_Ovf_Un] = Operator.Add, //Checked?

            [OpCodes.Div] = Operator.Divide,
            [OpCodes.Div_Un] = Operator.Divide,

            [OpCodes.Mul] = Operator.Multiply,
            [OpCodes.Mul_Ovf] = Operator.Multiply, //Checked?
            [OpCodes.Mul_Ovf_Un] = Operator.Multiply, //Checked?

            [OpCodes.Rem] = Operator.Remainder,
            [OpCodes.Rem_Un] = Operator.Remainder,

            [OpCodes.Sub] = Operator.Subtract,
            [OpCodes.Sub_Ovf] = Operator.Subtract, //Checked?
            [OpCodes.Sub_Ovf_Un] = Operator.Subtract, //Checked?


            [OpCodes.And] = Operator.And,

            [OpCodes.Or] = Operator.Or,
            //TODO: add these operators
            //[OpCodes.Xor] = Operator.ExclusiveOr,
            //[OpCodes.Shl] = Operator.LeftShift,
            //[OpCodes.Shr] = Operator.RightShift,
            //[OpCodes.Shr_Un] = Operator.RightShift,
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

            lhsExpression ??= lhs.AsExpression();
            rhsExpression ??= rhs.AsExpression();

            Operator op = _operatorLookup[baseExecutor.Instruction.OpCode];

            PreprocessBooleanExpressions(ref lhsExpression, ref rhsExpression);

            Expression resultExpression = Expression.MakeBinary(op, lhsExpression, rhsExpression);
            result.SetExpression(resultExpression, cur);
            
            return retValue;
        }

        private static void PreprocessBooleanExpressions(ref Expression lhs, ref Expression rhs)
        {
            // ensure both are the same 

            if (lhs.Type == ExpressionType.Boolean)
            {
                // ensure rhs is also boolean
                rhs = rhs.AsBoolean();
            }
            else if (lhs.Type == ExpressionType.Boolean)
            {
                // ensure rhs is also boolean
                lhs = lhs.AsBoolean();
            }
        }
    }
}
