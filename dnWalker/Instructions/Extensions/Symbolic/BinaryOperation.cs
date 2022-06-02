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

            [OpCodes.Rem] = Operator.Modulo,
            [OpCodes.Rem_Un] = Operator.Modulo,

            [OpCodes.Sub] = Operator.Subtract,
            [OpCodes.Sub_Ovf] = Operator.Subtract, //Checked?
            [OpCodes.Sub_Ovf_Un] = Operator.Subtract, //Checked?


            [OpCodes.And] = Operator.And,

            [OpCodes.Or] = Operator.Or,
            //TODO: add these operators
            [OpCodes.Xor] = Operator.Xor,
            [OpCodes.Shl] = Operator.ShiftLeft,
            [OpCodes.Shr] = Operator.ShiftRight,
            [OpCodes.Shr_Un] = Operator.ShiftRight,
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

            if (!ExpressionUtils.GetExpressions(cur, lhs, rhs, out Expression lhsExpression, out Expression rhsExpression))
            {
                return retValue;
            }

            IDataElement result = cur.EvalStack.Peek();

            Operator op = _operatorLookup[baseExecutor.Instruction.OpCode];

            PreprocessBooleanExpressions(ref lhsExpression, ref rhsExpression);
            if (lhs is IReferenceType)
                PreprocessReferenceComparison(ref op);

            Expression resultExpression = Expression.MakeBinary(op, lhsExpression, rhsExpression);
            result.SetExpression(cur, resultExpression);
            
            return retValue;
        }

        private static void PreprocessReferenceComparison(ref Operator op)
        {
            // if the result should be comparison between two references, we need to change 
            // <,> to !=
            switch (op)
            {
                case Operator.LessThan:
                case Operator.GreaterThan:
                    op = Operator.NotEqual;
                    break;
            }
        }

        private static void PreprocessBooleanExpressions(ref Expression lhs, ref Expression rhs)
        {
            // ensure both are the same 

            if (lhs.Type.IsBoolean())
            {
                // ensure rhs is also boolean
                rhs = rhs.AsBoolean();
            }
            else if (rhs.Type.IsBoolean())
            {
                // ensure rhs is also boolean
                lhs = lhs.AsBoolean();
            }
        }
    }
}
