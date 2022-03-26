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
    public class CONV : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, Type> _resultTypes = new Dictionary<OpCode, Type>()
        {
            // this should be based on the architecture... int vs long
            [OpCodes.Conv_I] = typeof(int),
            [OpCodes.Conv_I1] = typeof(sbyte),
            [OpCodes.Conv_I2] = typeof(short),
            [OpCodes.Conv_I4] = typeof(int),
            [OpCodes.Conv_I8] = typeof(long),

            // this should be based on the architecture... int vs long
            [OpCodes.Conv_Ovf_I] = typeof(int),
            [OpCodes.Conv_Ovf_I_Un] = typeof(int),
            [OpCodes.Conv_Ovf_I1] = typeof(sbyte),
            [OpCodes.Conv_Ovf_I1_Un] = typeof(sbyte),
            [OpCodes.Conv_Ovf_I2] = typeof(short),
            [OpCodes.Conv_Ovf_I2_Un] = typeof(short),
            [OpCodes.Conv_Ovf_I4] = typeof(int),
            [OpCodes.Conv_Ovf_I4_Un] = typeof(int),
            [OpCodes.Conv_Ovf_I8] = typeof(long),
            [OpCodes.Conv_Ovf_I8_Un] = typeof(long),

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_U] = typeof(uint),
            [OpCodes.Conv_U1] = typeof(byte),
            [OpCodes.Conv_U2] = typeof(ushort),
            [OpCodes.Conv_U4] = typeof(uint),
            [OpCodes.Conv_U8] = typeof(ulong),

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_Ovf_U] = typeof(uint),
            [OpCodes.Conv_Ovf_U_Un] = typeof(uint),
            [OpCodes.Conv_Ovf_U1] = typeof(byte),
            [OpCodes.Conv_Ovf_U1_Un] = typeof(byte),
            [OpCodes.Conv_Ovf_U2] = typeof(ushort),
            [OpCodes.Conv_Ovf_U2_Un] = typeof(ushort),
            [OpCodes.Conv_Ovf_U4] = typeof(uint),
            [OpCodes.Conv_Ovf_U4_Un] = typeof(uint),
            [OpCodes.Conv_Ovf_U8] = typeof(ulong),
            [OpCodes.Conv_Ovf_U8_Un] = typeof(ulong),

            [OpCodes.Conv_R_Un] = typeof(double),
            [OpCodes.Conv_R4] = typeof(float),
            [OpCodes.Conv_R8] = typeof(double),
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
                Type outType = _resultTypes[baseExecutor.Instruction.OpCode];
                if (expression.Type == outType)
                {
                    result.SetExpression(expression, cur);
                }
                else
                {
                    result.SetExpression(baseExecutor.CheckOverflow ? Expression.ConvertChecked(expression, outType) : Expression.Convert(expression, outType), cur);
                }
            }

            return returnValue;
        }
    }
}
