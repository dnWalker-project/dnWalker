﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public static class ControlFlowUtils
    {
        internal readonly struct SuccessorInfo : IEquatable<SuccessorInfo>
        {
            public static readonly SuccessorInfo NextInstruction = new SuccessorInfo();

            public static readonly SuccessorInfo[] NextInstructionArray = new SuccessorInfo[] { NextInstruction };
            public static readonly SuccessorInfo[] EmptyArray = new SuccessorInfo[0];

            public static SuccessorInfo ExceptionHandler(TypeDef exception)
            {
                return new SuccessorInfo(exception);
            }
            public static SuccessorInfo Branch(Instruction target)
            {
                return new SuccessorInfo(target);
            }

            public override bool Equals(object obj)
            {
                return obj is SuccessorInfo info && Equals(info);
            }

            public bool Equals(SuccessorInfo other)
            {
                return TypeEqualityComparer.Instance.Equals(ExceptionType, other.ExceptionType) &&
                       EqualityComparer<Instruction>.Default.Equals(Instruction, other.Instruction);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ExceptionType, Instruction);
            }

            public readonly TypeDef ExceptionType;
            public readonly Instruction Instruction;


            private SuccessorInfo(TypeDef exceptionType)
            {
                ExceptionType = exceptionType ?? throw new ArgumentNullException(nameof(exceptionType));
                Instruction = null;
            }

            public SuccessorInfo(Instruction instruction)
            {
                ExceptionType = null;
                Instruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
            }

            public static bool operator ==(SuccessorInfo left, SuccessorInfo right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SuccessorInfo left, SuccessorInfo right)
            {
                return !(left == right);
            }
        }

        public static bool EndsBlock(Instruction instruction)
        {
            return
                instruction.OpCode.Code == Code.Ret ||
                instruction.OpCode.Code == Code.Throw ||
                instruction.OpCode.Code == Code.Rethrow;
        }

        public static bool HasMultipleSuccessors(Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:

                case Code.Beq:
                case Code.Beq_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:

                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:

                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelema:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldftn:
                case Code.Ldlen:
                case Code.Ldvirtftn: // null ref
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:

                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                case Code.Stfld:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                    return true;

                case Code.Call:
                case Code.Callvirt:
                    return !((IMethod)instruction.Operand).ResolveMethodDefThrow().IsStatic;

                case Code.Switch:
                    throw new NotSupportedException();

                default:
                    return false;
            }
        }

        public static int GetSuccessorCount(Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Beq:
                case Code.Beq_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                    // next, branch
                    return 2;

                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:

                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    // next, overflow exception
                    return 2;

                case Code.Div:
                case Code.Div_Un:
                    // next, divide by zero exception
                    // ?? when dividing floats, results in Non Finite, when dividing integers, results in exceptions
                    return 2;

                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelema:

                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                    // next, null reference exception, index out of range exception
                    return 3;

                case Code.Ldlen:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldftn:
                case Code.Ldvirtftn:

                case Code.Stfld:
                    // next, null reference exception
                    return 2;

                case Code.Call:
                case Code.Callvirt:
                    // if static => next only
                    // if not static => next, null reference exception
                    return ((IMethod)instruction.Operand).ResolveMethodDefThrow().IsStatic ? 2 : 1;

                case Code.Ret:
                    return 0;

                case Code.Switch:
                    throw new NotSupportedException();

                default:
                    return 1;
            }
        }

        internal static SuccessorInfo[] GetSuccessors(Instruction instruction, ModuleDef context, IDictionary<string, TypeDef> exceptionCache = null)
        {
            const string NullReference = nameof(NullReferenceException);
            const string DivideByZero = nameof(DivideByZeroException);
            const string IndexOutOfRange = nameof(IndexOutOfRangeException);
            const string Overflow = nameof(OverflowException);

            Debug.Assert(context != null);

            ModuleDef cm = context.CorLibTypes.Boolean.Module;

            TypeDef GetException(string exceptionTypeName)
            {
                if (exceptionCache != null)
                {
                    if (!exceptionCache.TryGetValue(exceptionTypeName, out TypeDef exceptionType))
                    {
                        exceptionType = cm.Types.First(t => t.Name == exceptionTypeName);
                        exceptionCache[exceptionTypeName] = exceptionType;
                    }
                    return exceptionType;
                }
                return cm.Types.First(t => t.Name == exceptionTypeName);
            }


            switch (instruction.OpCode.Code)
            {
                case Code.Beq:
                case Code.Beq_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                    // next, branch
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.Branch((Instruction)instruction.Operand)
                    };

                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:

                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    // next, overflow exception
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(Overflow))
                    };

                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelema:

                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                    // next, null reference exception, index out of range exception
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(NullReference)),
                        SuccessorInfo.ExceptionHandler(GetException(IndexOutOfRange))
                    };

                case Code.Ldlen:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldftn:
                case Code.Ldvirtftn:

                case Code.Stfld:
                    // next, null reference exception
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(NullReference))
                    };

                case Code.Call:
                case Code.Callvirt:
                    // if static => next only
                    // if not static => next, null reference exception
                    if (((IMethod)instruction.Operand).ResolveMethodDefThrow().IsStatic)
                    {
                        return SuccessorInfo.NextInstructionArray;
                    }
                    else return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(NullReference))
                    };

                case Code.Ret:
                    return SuccessorInfo.EmptyArray;

                case Code.Switch:
                    throw new NotSupportedException();

                default:
                    return SuccessorInfo.NextInstructionArray;
            }
        }

        public static InstructionBlockNode GetNode(IReadOnlyList<InstructionBlockNode> blocks, Instruction instruction)
        {
            int l = 0;
            int h = blocks.Count - 1;

            uint offset = instruction.Offset;

            while (l <= h)
            {
                int m = (l + h) >> 1; // equivalent to Math.Flor((l + h) / 2)

                InstructionBlockNode block = blocks[m];
                int cmpResult = Compare(block, offset);
                if (cmpResult == 0)
                    return block;
                else if (cmpResult < 0)
                    l = m + 1;
                else // if (cmpResult > 0)
                    h = m - 1;
            }

            return null;

            static int Compare(InstructionBlockNode block, uint offset)
            {
                uint refOffset = block.Header.Offset;
                if (offset < refOffset) return -1;

                refOffset = block.Footer.Offset;
                if (offset <= refOffset) return 0;

                return 1;
            }
        }

        public static bool TryGetHandler(Instruction instruction, TypeDef exceptionType, MethodDef method, out Instruction handlerHeader)
        {
            // TODO: search for the exception handlers within method.Body
            handlerHeader = null;
            return false;
        }
    }
}
