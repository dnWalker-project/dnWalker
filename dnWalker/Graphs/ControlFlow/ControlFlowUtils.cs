using dnlib.DotNet;
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
        internal enum SuccessorType
        {
            Next,
            Jump,
            Exception,
            AssertViolation
        }

        internal readonly struct SuccessorInfo : IEquatable<SuccessorInfo>
        {
            public static readonly SuccessorInfo NextInstruction = new SuccessorInfo(null, null, SuccessorType.Next);
            public static readonly SuccessorInfo AssertViolation = new SuccessorInfo(null, null, SuccessorType.AssertViolation);

            public static readonly SuccessorInfo[] NextInstructionArray = new SuccessorInfo[] { NextInstruction };
            public static readonly SuccessorInfo[] AssertArray = new SuccessorInfo[] { NextInstruction, AssertViolation };
            public static readonly SuccessorInfo[] EmptyArray = new SuccessorInfo[0];

            public static SuccessorInfo ExceptionHandler(TypeDef exception)
            {
                return new SuccessorInfo(null, exception, SuccessorType.Exception);
            }
            public static SuccessorInfo Jump(Instruction target)
            {
                return new SuccessorInfo(target, null, SuccessorType.Jump);
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

            public readonly SuccessorType Type;
            public readonly TypeDef ExceptionType;
            public readonly Instruction Instruction;


            private SuccessorInfo(Instruction instruction, TypeDef exception, SuccessorType type)
            {
                Instruction = instruction;
                ExceptionType = exception;
                Type = type;
            }

            public static bool operator ==(SuccessorInfo left, SuccessorInfo right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SuccessorInfo left, SuccessorInfo right)
            {
                return !(left == right);
            }

            public override string ToString()
            {
                if (Instruction == null && ExceptionType == null) return "Next Instruction";
                else if (Instruction == null && ExceptionType != null) return $"Virtual Exception Handler [{ExceptionType}]";
                else if (Instruction != null && ExceptionType == null) return $"Branch To [{Instruction}]";
                else return $"Concrete Exception Handler [{ExceptionType}] ad [{Instruction}]";
            }
        }

        public static bool EndsBlock(Instruction instruction)
        {
            return
                HasMultipleSuccessors(instruction) ||
                instruction.OpCode.Code == Code.Ret ||
                instruction.OpCode.Code == Code.Throw ||
                instruction.OpCode.Code == Code.Rethrow ||
                instruction.OpCode.Code == Code.Br ||
                instruction.OpCode.Code == Code.Br_S ||

                // all kinds of CALL instruction should end the block - may throw arbitrary exception...
                instruction.OpCode.Code == Code.Call ||
                instruction.OpCode.Code == Code.Calli ||
                instruction.OpCode.Code == Code.Callvirt;
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

                case Code.Div:
                case Code.Div_Un:
                case Code.Rem:
                case Code.Rem_Un:

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
                    return true;

                default:
                    return false;
            }
        }

        //public static int GetSuccessorCount(Instruction instruction)
        //{
        //    switch (instruction.OpCode.Code)
        //    {
        //        case Code.Beq:
        //        case Code.Beq_S:
        //        case Code.Bge:
        //        case Code.Bge_S:
        //        case Code.Bge_Un:
        //        case Code.Bge_Un_S:
        //        case Code.Bgt:
        //        case Code.Bgt_S:
        //        case Code.Bgt_Un:
        //        case Code.Bgt_Un_S:
        //        case Code.Ble:
        //        case Code.Ble_S:
        //        case Code.Ble_Un:
        //        case Code.Ble_Un_S:
        //        case Code.Blt:
        //        case Code.Blt_S:
        //        case Code.Blt_Un:
        //        case Code.Blt_Un_S:
        //        case Code.Bne_Un:
        //        case Code.Bne_Un_S:
        //        case Code.Brfalse:
        //        case Code.Brfalse_S:
        //        case Code.Brtrue:
        //        case Code.Brtrue_S:
        //            // next, branch
        //            return 2;

        //        case Code.Add_Ovf:
        //        case Code.Add_Ovf_Un:
        //        case Code.Sub_Ovf:
        //        case Code.Sub_Ovf_Un:
        //        case Code.Mul_Ovf:
        //        case Code.Mul_Ovf_Un:

        //        case Code.Conv_Ovf_I:
        //        case Code.Conv_Ovf_I_Un:
        //        case Code.Conv_Ovf_I1:
        //        case Code.Conv_Ovf_I1_Un:
        //        case Code.Conv_Ovf_I2:
        //        case Code.Conv_Ovf_I2_Un:
        //        case Code.Conv_Ovf_I4:
        //        case Code.Conv_Ovf_I4_Un:
        //        case Code.Conv_Ovf_I8:
        //        case Code.Conv_Ovf_I8_Un:
        //        case Code.Conv_Ovf_U:
        //        case Code.Conv_Ovf_U_Un:
        //        case Code.Conv_Ovf_U1:
        //        case Code.Conv_Ovf_U1_Un:
        //        case Code.Conv_Ovf_U2:
        //        case Code.Conv_Ovf_U2_Un:
        //        case Code.Conv_Ovf_U4:
        //        case Code.Conv_Ovf_U4_Un:
        //        case Code.Conv_Ovf_U8:
        //        case Code.Conv_Ovf_U8_Un:
        //            // next, overflow exception
        //            return 2;

        //        case Code.Div:
        //        case Code.Div_Un:
        //            // next, divide by zero exception
        //            // ?? when dividing floats, results in Non Finite, when dividing integers, results in exceptions
        //            return 2;

        //        case Code.Rem:
        //        case Code.Rem_Un:
        //            // next, divide by zero exception
        //            // ?? when dividing floats, results in Non Finite, when dividing integers, results in exceptions
        //            return 2;

        //        case Code.Ldelem:
        //        case Code.Ldelem_I:
        //        case Code.Ldelem_I1:
        //        case Code.Ldelem_I2:
        //        case Code.Ldelem_I4:
        //        case Code.Ldelem_I8:
        //        case Code.Ldelem_R4:
        //        case Code.Ldelem_R8:
        //        case Code.Ldelem_Ref:
        //        case Code.Ldelem_U1:
        //        case Code.Ldelem_U2:
        //        case Code.Ldelem_U4:
        //        case Code.Ldelema:

        //        case Code.Stelem:
        //        case Code.Stelem_I:
        //        case Code.Stelem_I1:
        //        case Code.Stelem_I2:
        //        case Code.Stelem_I4:
        //        case Code.Stelem_I8:
        //        case Code.Stelem_R4:
        //        case Code.Stelem_R8:
        //        case Code.Stelem_Ref:
        //            // next, null reference exception, index out of range exception
        //            return 3;

        //        case Code.Ldlen:
        //        case Code.Ldfld:
        //        case Code.Ldflda:
        //        case Code.Ldftn:
        //        case Code.Ldvirtftn:

        //        case Code.Stfld:
        //            // next, null reference exception
        //            return 2;

        //        case Code.Call:
        //        case Code.Callvirt:
        //            // if static => next only
        //            // if not static => next, null reference exception
        //            //{
        //            //    MethodDef md = ((IMethod)instruction.Operand).ResolveMethodDef();
        //            //    if (md != null)
        //            //    {
        //            //        return md.IsStatic ? 1 : 2;
        //            //    }
        //            //    throw new Exception("Could not resolve method!");
        //            //}
        //            return ((IMethod)instruction.Operand).ResolveMethodDefThrow().IsStatic ? 1 : 2;

        //        case Code.Ret:
        //            return 0;

        //        case Code.Switch:
        //            return ((Instruction[])instruction.Operand).Length;

        //        default:
        //            return 1;
        //    }
        //}

        internal static SuccessorInfo[] GetSuccessors(Instruction instruction, ModuleDef context, IDictionary<string, TypeDef> exceptionCache = null)
        {
            const string NullReference = "NullReferenceException";
            const string DivideByZero = "DivideByZeroException";
            const string IndexOutOfRange = "IndexOutOfRangeException";
            const string Overflow = "OverflowException";

            Debug.Assert(context != null);

            TypeDef GetException(string exceptionTypeName)
            {
                IResolver resolver = context.Context.Resolver;
                //AssemblyDef corAssembly = context.Context.AssemblyResolver.Resolve(context.CorLibTypes.AssemblyRef, null);
                if (exceptionCache != null)
                {
                    if (!exceptionCache.TryGetValue(exceptionTypeName, out TypeDef exceptionType))
                    {
                        exceptionType = resolver.Resolve(new TypeRefUser(context, "System", exceptionTypeName), context);

                        //exceptionType = context.Context.Resolver.Resolve(new TypeRefUser(null, "System", exceptionTypeName), null);
                        //TypeRef typeRef = new TypeRefUser(cm, "System", exceptionTypeName);
                        exceptionCache[exceptionTypeName] = exceptionType;
                    }
                    return exceptionType;
                }
                return resolver.Resolve(new TypeRefUser(context, exceptionTypeName));
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
                        SuccessorInfo.Jump((Instruction)instruction.Operand)
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

                case Code.Div:
                case Code.Div_Un:
                    // next, divide by zero exception
                    // ?? when dividing floats, results in Non Finite, when dividing integers, results in exceptions
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(DivideByZero))
                    };

                case Code.Rem:
                case Code.Rem_Un:
                    // next, divide by zero exception
                    // ?? when dividing floats, results in Non Finite, when dividing integers, results in exceptions
                    return new SuccessorInfo[]
                    {
                        SuccessorInfo.NextInstruction,
                        SuccessorInfo.ExceptionHandler(GetException(DivideByZero))
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
                    {
                        MethodDef m = ((IMethod)instruction.Operand).ResolveMethodDefThrow();
                        // if static => next only
                        // if not static => next, null reference exception
                        if (m.IsStatic)
                        {
                            if (m.DeclaringType.FullName == "System.Diagnostics.Debug" && m.Name == "Assert")
                            {
                                return SuccessorInfo.AssertArray;
                            }

                            return SuccessorInfo.NextInstructionArray;
                        }
                        else
                        {
                            return new SuccessorInfo[]
                            {
                                SuccessorInfo.NextInstruction,
                                SuccessorInfo.ExceptionHandler(GetException(NullReference))
                            };
                        }
                    }

                case Code.Ret:
                    return SuccessorInfo.EmptyArray;

                case Code.Switch:
                    {
                        // the switch may contain "explicit" default - one of the target instructions is the next instruction
                        // if that is the case, it should be 

                        SuccessorInfo[] successors = ((IList<Instruction>)instruction.Operand).Select(i => SuccessorInfo.Jump(i)).Append(SuccessorInfo.NextInstruction).ToArray();

                        return successors;
                    }

                case Code.Br:
                case Code.Br_S:
                    return new SuccessorInfo[] { SuccessorInfo.Jump((Instruction)instruction.Operand) };

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
                    h = m - 1;
                else // if (cmpResult > 0)
                    l = m + 1;
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

        //public static ControlFlowEdge CreateEdge(ControlFlowNode source, ControlFlowNode target, TypeDef exception = null)
        //{
        //    ControlFlowEdge edge;
        //    if (exception != null)
        //    {
        //        edge = new ExceptionEdge(exception, source, target);
        //    }
        //    else
        //    {
        //        InstructionBlockNode sourceBlock = source as InstructionBlockNode;
        //        InstructionBlockNode targetBlock = target as InstructionBlockNode;
        //        if (sourceBlock == null || targetBlock == null) throw new InvalidOperationException("Both source and target must be instruction block nodes.");

        //        if (sourceBlock.Footer.Offset + sourceBlock.Footer.GetSize() ==
        //            targetBlock.Header.Offset)
        //        {
        //            // target block is just after source block => next edge
        //            edge = new NextEdge(source, target);
        //        }
        //        else
        //        {
        //            edge = new JumpEdge(source, target);
        //        }
        //    }

        //    EnsureConnected(edge);
        //    return edge;
        //}

        public static NextEdge CreateNextEdge(ControlFlowNode source, ControlFlowNode target)
        {
            NextEdge edge = new NextEdge(source, target);
            EnsureConnected(edge);
            return edge;
        }

        public static JumpEdge CreateJumpEdge(ControlFlowNode source, ControlFlowNode target)
        {
            JumpEdge edge = new JumpEdge(source, target);
            EnsureConnected(edge);
            return edge;
        }

        public static ExceptionEdge CreateExceptionEdge(TypeDef exception, ControlFlowNode source, ControlFlowNode target)
        {
            ExceptionEdge edge = new ExceptionEdge(exception, source, target);
            EnsureConnected(edge);
            return edge;
        }

        public static AssertViolationEdge CreateAssertViolationEdge(ControlFlowNode src, ControlFlowNode trg)
        {
            AssertViolationEdge edge = new AssertViolationEdge(src, trg);
            EnsureConnected(edge);
            return edge;
        }

        //public static ControlFlowEdge GetOrCreateEdge(ControlFlowNode source, ControlFlowNode target)
        //{
        //    if (source.Successors.Contains(target))
        //    {
        //        return source.GetEdgesTo(target);
        //    }

        //    return CreateEdge(source, target);
        //}

        private static void EnsureConnected(ControlFlowEdge edge)
        {
            edge.Source.AddOutEdge(edge);
            edge.Target.AddInEdge(edge);
        }

        public static void MarkUnreachable(ControlFlowEdge edge)
        {
            edge.MarkUnreachable();

            ControlFlowNode target = edge.Target;
            if (target.InEdges.All(static e => !e.IsReachable))
            {
                // every in edge is marked as unreachable => we can marking every out edge in same manner
                foreach (ControlFlowEdge outEdge in target.OutEdges)
                {
                    MarkUnreachable(outEdge);
                }
            }
        }
    }
}
