using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.TypeSystem;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class CONV_OVF : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, TypeCode> _resultTypes = new Dictionary<OpCode, TypeCode>()
        {
            // this should be based on the architecture... int vs long
            [OpCodes.Conv_Ovf_I] = TypeCode.Int32,
            [OpCodes.Conv_Ovf_I_Un] = TypeCode.Int32,
            [OpCodes.Conv_Ovf_I1] = TypeCode.SByte,
            [OpCodes.Conv_Ovf_I1_Un] = TypeCode.SByte,
            [OpCodes.Conv_Ovf_I2] = TypeCode.Int16,
            [OpCodes.Conv_Ovf_I2_Un] = TypeCode.Int16,
            [OpCodes.Conv_Ovf_I4] = TypeCode.Int32,
            [OpCodes.Conv_Ovf_I4_Un] = TypeCode.Int32,
            [OpCodes.Conv_Ovf_I8] = TypeCode.Int64,
            [OpCodes.Conv_Ovf_I8_Un] = TypeCode.Int64,

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_Ovf_U] = TypeCode.UInt32,
            [OpCodes.Conv_Ovf_U_Un] = TypeCode.UInt32,
            [OpCodes.Conv_Ovf_U1] = TypeCode.Byte,
            [OpCodes.Conv_Ovf_U1_Un] = TypeCode.Byte,
            [OpCodes.Conv_Ovf_U2] = TypeCode.UInt16,
            [OpCodes.Conv_Ovf_U2_Un] = TypeCode.UInt16,
            [OpCodes.Conv_Ovf_U4] = TypeCode.UInt32,
            [OpCodes.Conv_Ovf_U4_Un] = TypeCode.UInt32,
            [OpCodes.Conv_Ovf_U8] = TypeCode.UInt64,
            [OpCodes.Conv_Ovf_U8_Un] = TypeCode.UInt64,
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
                TypeCode outType = _resultTypes[baseExecutor.Instruction.OpCode];
                TypeCode inType = expression.Type.GetTypeCode();

                if (outType != inType)
                {
                    TypeSig targetType = cur.DefinitionProvider.GetTypeSig(outType);
                    expression = Expression.MakeConvert(targetType, expression);
                }

                // TODO: add exception constraint 
                // i.e. - get limits based on the OpCode and enforce the in expression be within them
                // choices:
                // - normal - the expression is within the limits
                // - underflow - the expression is less than value than the target minimum
                //   - will NOT happen with target type = Int64 - no other number may be less than long.MinValue
                // - overflow - the expression is greater than value than the target maximum
                //   - will NOT happen with target type = UInt64 - no other number may be greater than ulong.MinValue
                // => number of choices is dependent on the target type!!!!

                if (outType == TypeCode.Int64)
                {
                    // check which choice
                }


                result.SetExpression(cur, expression);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the minimal value.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private static long GetMin(TypeCode tc)
        {
            return tc switch
            {
                TypeCode.SByte => sbyte.MinValue, 
                TypeCode.Byte => byte.MinValue,
                TypeCode.Int16 => short.MinValue,
                TypeCode.UInt16 => ushort.MinValue,
                TypeCode.Int32 => int.MinValue,
                TypeCode.UInt32 => uint.MinValue,
                TypeCode.Int64 => long.MinValue,
                TypeCode.UInt64 => 0,
                _ => long.MinValue
            };
        }

        /// <summary>
        /// Gets the maximal value.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private static long GetMax(TypeCode tc)
        {
            return tc switch
            {
                TypeCode.SByte => sbyte.MaxValue,
                TypeCode.Byte => byte.MaxValue,
                TypeCode.Int16 => short.MaxValue,
                TypeCode.UInt16 => ushort.MaxValue,
                TypeCode.Int32 => int.MaxValue,
                TypeCode.UInt32 => uint.MaxValue,
                TypeCode.Int64 => long.MaxValue,
                TypeCode.UInt64 => long.MaxValue, // should not be ever used 
                _ => long.MaxValue
            };
        }
    }
}
