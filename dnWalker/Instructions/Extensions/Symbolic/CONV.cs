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
    public class CONV : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, TypeCode> _resultTypes = new Dictionary<OpCode, TypeCode>()
        {
            // this should be based on the architecture... int vs long
            [OpCodes.Conv_I] = TypeCode.Int32,
            [OpCodes.Conv_I1] = TypeCode.SByte,
            [OpCodes.Conv_I2] = TypeCode.Int16,
            [OpCodes.Conv_I4] = TypeCode.Int32,
            [OpCodes.Conv_I8] = TypeCode.Int64,

            // this should be based on the architecture... uint vs ulong
            [OpCodes.Conv_U] = TypeCode.UInt32,
            [OpCodes.Conv_U1] = TypeCode.Byte,
            [OpCodes.Conv_U2] = TypeCode.UInt16,
            [OpCodes.Conv_U4] = TypeCode.UInt32,
            [OpCodes.Conv_U8] = TypeCode.UInt64,

            [OpCodes.Conv_R_Un] = TypeCode.Double,
            [OpCodes.Conv_R4] = TypeCode.Single,
            [OpCodes.Conv_R8] = TypeCode.Double,
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

                result.SetExpression(cur, expression);
            }

            return returnValue;
        }
    }
}
