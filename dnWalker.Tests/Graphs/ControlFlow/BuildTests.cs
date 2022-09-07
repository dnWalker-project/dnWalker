using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit.Abstractions;
using dnlib.DotNet.Emit;

namespace dnWalker.Tests.Graphs.ControlFlow
{
    public class BuildTests : DnlibTestBase
    {
        private const string AssemblyFileName = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";
        private const string TestClassNamespace = "dnSpy.Debugger.DotNet.Interpreter.Tests";
        private const string TestClassType = "TestClass";

        private int _moduleCounter = 0;
        private int _typeCounter = 0;
        private int _methodCounter = 0;

        protected MethodDef CreateDynamicMethod(Action<CilBody> createAction)
        {
            return CreateDynamicMethod("DynamicModule" + ++_methodCounter, "DynamicType" + ++_typeCounter, "DynamicMethod" + ++_methodCounter, createAction);
        }

        protected MethodDef CreateDynamicMethod(string moduleName, string typeName, string methodName, Action<CilBody> createAction)
        {
            ModuleContext context = DefinitionProvider.Context.ModuleContext;

            AssemblyDefUser userAssembly = new AssemblyDefUser(moduleName);
            
            ModuleDefUser userModule = new ModuleDefUser(moduleName);
            userModule.Context = context;

            userAssembly.Modules.Add(userModule);

            TypeDefUser userType = new TypeDefUser(typeName);
            userModule.Types.Add(userType);

            MethodDefUser userMethod = new MethodDefUser(methodName) { Body = new CilBody() };

            createAction(userMethod.Body);

            FixOffsets(userMethod.Body);

            userType.Methods.Add(userMethod);

            return userMethod;

            static void FixOffsets(CilBody body)
            {
                uint offset = 0x00;
                foreach (Instruction i in body.Instructions)
                {
                    i.Offset = offset;
                    offset += (uint) i.GetSize();
                }
            }
        }

        public BuildTests(ITestOutputHelper textOutput) : base(textOutput, AssemblyFileName)
        {
        }

        [Theory]
        [InlineData("Test_ADD__Int32")]
        [InlineData("Test_AND__Int32")]
        [InlineData("Test_CEQ__Int32")]
        [InlineData("Test_CGT__Int32")]
        [InlineData("Test_CGT_UN__Int32")]
        [InlineData("Test_CLT__Int32")]
        [InlineData("Test_CLT_UN__Int32")]
        [InlineData("Test_MUL__Int32")]
        [InlineData("Test_OR__Int32")]
        [InlineData("Test_SHL__Int32")]
        [InlineData("Test_SHR__Int32")]
        [InlineData("Test_SHR_UN__Int32")]
        [InlineData("Test_SUB__Int32")]
        [InlineData("Test_XOR__Int32")]
        public void SingleNodeGraph(string methodName)
        {
            // IL_0000: ldarg.0
            // IL_0001: ldarg.1
            // IL_0002: add/sub/mul
            // IL_0003: ret
            MethodDef method = GetMethod(TestClassNamespace, TestClassType, methodName);

            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(1);
            ControlFlowNode node = graph.Nodes.First();

            node.Should().BeOfType<InstructionBlockNode>();

            InstructionBlockNode block = (InstructionBlockNode)node;

            block.Instructions.Should().HaveCount(4);
        }

        [Theory]
        [InlineData("Test_BEQ__Int32")]
        [InlineData("Test_BEQ_S__Int32")]
        [InlineData("Test_BGE__Int32")]
        [InlineData("Test_BGE_S__Int32")]
        [InlineData("Test_BGE_UN__Int32")]
        [InlineData("Test_BGE_UN_S__Int32")]
        [InlineData("Test_BGT__Int32")]
        [InlineData("Test_BGT_S__Int32")]
        [InlineData("Test_BGT_UN__Int32")]
        [InlineData("Test_BGT_UN_S__Int32")]
        [InlineData("Test_BNE_UN__Int32")]
        [InlineData("Test_BLE__Int32")]
        [InlineData("Test_BLE_S__Int32")]
        [InlineData("Test_BLE_UN__Int32")]
        [InlineData("Test_BLE_UN_S__Int32")]
        [InlineData("Test_BLT__Int32")]
        [InlineData("Test_BLT_S__Int32")]
        [InlineData("Test_BLT_UN__Int32")]
        [InlineData("Test_BLT_UN_S__Int32")]
        [InlineData("Test_BRFALSE__Int32")]
        [InlineData("Test_BRFALSE_S__Int32")]
        [InlineData("Test_BRTRUE__Int32")]
        [InlineData("Test_BRTRUE_S__Int32")]
        public void ThreeNodesNextJumpEdges(string methodName)
        {
            // IL_0000: ldarg.0
            // IL_0001: ldarg.1
            // IL_0002: beq/... IL_0009

            // IL_0007: ldc.i4.0
            // IL_0008: ret

            // IL_0009: ldc.i4.8
            // IL_000a: ret

            MethodDef method = GetMethod(TestClassNamespace, TestClassType, methodName);
            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(3);
            graph.Nodes.Should().ContainItemsAssignableTo<InstructionBlockNode>();
            graph.Edges.Should().HaveCount(2);

            graph.EntryPoint.OutEdges.Should().HaveCount(2);
            graph.EntryPoint.OutEdges.Should().Contain(e => e is NextEdge);
            graph.EntryPoint.OutEdges.Should().Contain(e => e is JumpEdge);

        }

        [Theory]
        [InlineData("Test_ADD_OVF__Int32")]
        [InlineData("Test_ADD_OVF_UN__Int32")]
        [InlineData("Test_MUL_OVF__Int32")]
        [InlineData("Test_MUL_OVF_UN__Int32")]
        [InlineData("Test_SUB_OVF__Int32")]
        [InlineData("Test_SUB_OVF_UN__Int32")]
        public void OvfInstructionCreatesVirtualExceptionHandlerForOerflow(string methodName)
        {
            // IL_0000: ldarg.0
            // IL_0001: ldarg.1
            // IL_0002: add/.../.ovf
            // IL_0003: ret

            MethodDef method = GetMethod(TestClassNamespace, TestClassType, methodName);
            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(3);
            graph.Nodes.OfType<InstructionBlockNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().HaveCount(1);

            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Single().ExceptionType.Should().BeEquivalentTo(GetType("System", "OverflowException"));

            graph.Edges.Should().HaveCount(2);
            graph.Edges.OfType<NextEdge>().Should().HaveCount(1);
            graph.Edges.OfType<ExceptionEdge>().Should().HaveCount(1);
        }

        [Theory(Skip = "Divide by zero is thrown iff the operands are integer types, which is impossible to guess just from the CIL.")]
        [InlineData("Test_DIV__Int32")]
        [InlineData("Test_DIV_UN__Int32")]
        [InlineData("Test_REM__Int32")]
        [InlineData("Test_REM_UN__Int32")]
        public void DivideInstructionsCreatesVirtualExceptionHandlerForDivideByZero(string methodName)
        {
            // IL_0000: ldarg.0
            // IL_0001: ldarg.1
            // IL_0002: div/rem/
            // IL_0003: ret

            MethodDef method = GetMethod(TestClassNamespace, TestClassType, methodName);
            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(3);
            graph.Nodes.OfType<InstructionBlockNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().HaveCount(1);

            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Single().ExceptionType.Should().BeEquivalentTo(GetType("System", "DivideByZeroException"));

            graph.Edges.Should().HaveCount(2);
            graph.Edges.OfType<NextEdge>().Should().HaveCount(1);
            graph.Edges.OfType<ExceptionEdge>().Should().HaveCount(1);
        }

        [Fact]
        public void LDLENCreatesNullReferenceException()
        {
            MethodDef method = CreateDynamicMethod(b =>
            {
                // load 1
                b.Instructions.Add(OpCodes.Ldc_I4_1.ToInstruction());
                // create array of int with length 1
                b.Instructions.Add(OpCodes.Newarr.ToInstruction(DefinitionProvider.BaseTypes.Int32));
                b.Instructions.Add(OpCodes.Ldlen.ToInstruction());
                b.Instructions.Add(OpCodes.Ret.ToInstruction());
            });

            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(3);
            graph.Nodes.OfType<InstructionBlockNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().HaveCount(1);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Single().ExceptionType.Should().BeEquivalentTo(GetType("System", "NullReferenceException"));

            graph.Edges.Should().HaveCount(2);
            graph.Edges.OfType<NextEdge>().Should().HaveCount(1);
            graph.Edges.OfType<ExceptionEdge>().Should().HaveCount(1);
        }

        [Fact]
        public void LDELEMCreatesNullReferenceAndIndexOutOfRangeExceptions()
        {
            MethodDef method = CreateDynamicMethod(b =>
            {
                // load 1
                b.Instructions.Add(OpCodes.Ldc_I4_1.ToInstruction());
                // create array of int with length 1
                b.Instructions.Add(OpCodes.Newarr.ToInstruction(DefinitionProvider.BaseTypes.Int32));
                // of course once optimization kicks in, no bound check would be produced by the JIT - because the index is constant
                b.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
                b.Instructions.Add(OpCodes.Ldelem_I4.ToInstruction());
                b.Instructions.Add(OpCodes.Ret.ToInstruction());
            });


            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(4);
            graph.Nodes.OfType<InstructionBlockNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().Contain(e => e.ExceptionType.Equals(GetType("System", "NullReferenceException")));
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().Contain(e => e.ExceptionType.Equals(GetType("System", "IndexOutOfRangeException")));

            graph.Edges.Should().HaveCount(3);
            graph.Edges.OfType<NextEdge>().Should().HaveCount(1);
            graph.Edges.OfType<ExceptionEdge>().Should().HaveCount(2);
        }

        [Fact]
        public void STELEMCreatesNullReferenceAndIndexOutOfRangeExceptions()
        {
            MethodDef method = CreateDynamicMethod(b =>
            {
                // load 1
                b.Instructions.Add(OpCodes.Ldc_I4_1.ToInstruction());
                // create array of int with length 1
                b.Instructions.Add(OpCodes.Newarr.ToInstruction(DefinitionProvider.BaseTypes.Int32));
                // of course once optimization kicks in, no bound check would be produced by the JIT - because the index is constant
                b.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
                b.Instructions.Add(OpCodes.Ldc_I4_1.ToInstruction());
                b.Instructions.Add(OpCodes.Stelem_I4.ToInstruction());
                b.Instructions.Add(OpCodes.Ret.ToInstruction());
            });


            ControlFlowGraph graph = ControlFlowGraph.Build(method);

            graph.Nodes.Should().HaveCount(4);
            graph.Nodes.OfType<InstructionBlockNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().HaveCount(2);
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().Contain(e => e.ExceptionType.Equals(GetType("System", "NullReferenceException")));
            graph.Nodes.OfType<VirtualExceptionHandlerNode>().Should().Contain(e => e.ExceptionType.Equals(GetType("System", "IndexOutOfRangeException")));

            graph.Edges.Should().HaveCount(3);
            graph.Edges.OfType<NextEdge>().Should().HaveCount(1);
            graph.Edges.OfType<ExceptionEdge>().Should().HaveCount(2);
        }
    }
}
