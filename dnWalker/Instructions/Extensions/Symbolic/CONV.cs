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
    public class CONV : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, ExpressionType> _resultTypes = new Dictionary<OpCode, ExpressionType>()
        {
            // this should be based on the architecture... int vs long
            [OpCodes.Conv_I] = ExpressionType.Integer,
            [OpCodes.Conv_I1] = ExpressionType.Integer,
            [OpCodes.Conv_I2] = ExpressionType.Integer,
            [OpCodes.Conv_I4] = ExpressionType.Integer,
            [OpCodes.Conv_I8] = ExpressionType.Integer,

            // this should be based on the architecture... int vs long
            [OpCodes.Conv_Ovf_I] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I1] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I1_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I2] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I2_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I4] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I4_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I8] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_I8_Un] = ExpressionType.Integer,

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_U] = ExpressionType.Integer,
            [OpCodes.Conv_U1] = ExpressionType.Integer,
            [OpCodes.Conv_U2] = ExpressionType.Integer,
            [OpCodes.Conv_U4] = ExpressionType.Integer,
            [OpCodes.Conv_U8] = ExpressionType.Integer,

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_Ovf_U] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U1] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U1_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U2] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U2_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U4] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U4_Un] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U8] = ExpressionType.Integer,
            [OpCodes.Conv_Ovf_U8_Un] = ExpressionType.Integer,

            [OpCodes.Conv_R_Un] = ExpressionType.Real,
            [OpCodes.Conv_R4] = ExpressionType.Real,
            [OpCodes.Conv_R8] = ExpressionType.Real,
        };

        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _resultTypes.Keys;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement toPop = cur.EvalStack.Peek();
            INumericElement operand = (toPop is IManagedPointer) ? (toPop as IManagedPointer).ToInt4() : (INumericElement)toPop;

            IIEReturnValue returnValue = next(baseExecutor, cur);

            if (returnValue is ExceptionHandlerLookupReturnValue)
            {
                // an exception was thrown (probably overflow exception...)
                return returnValue;
            }

            IDataElement result = cur.EvalStack.Peek();

            if (operand.TryGetExpression(cur, out Expression expression))
            {
                ExpressionType outType = _resultTypes[baseExecutor.Instruction.OpCode];
                ExpressionType inType = expression.Type;

                if (outType != inType)
                {
                    expression = (inType, outType) switch
                    {
                        (ExpressionType.Integer, ExpressionType.Real) => Expression.IntegerToReal(expression),
                        (ExpressionType.Real, ExpressionType.Real) => Expression.RealToInteger(expression),
                        _ => throw new InvalidOperationException("Unexpected expression types. The CONV instruction should accept only integer and real values!!!")
                    };
                }

                if (baseExecutor.CheckOverflow)
                {
                    // TODO: add exception constraint 
                    // i.e. - get limits based on the OpCode and enforce the in expression be within them

                    result.SetExpression(expression, cur);
                }
                else
                {
                    result.SetExpression(expression, cur);
                }
            }

            return returnValue;
        }
    }
}
