using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Graphs.ControlFlow
{
    public class SwitchTests
    {
        private const String AssemblyFileName = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";

        private readonly TypeDef _testClassTD;

        public SwitchTests()
        {
            ModuleDef testModule = ModuleDefMD.Load(AssemblyFileName);
            _testClassTD = testModule.Find("dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass", true);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_1()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__1");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_2()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__2");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_3()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__3");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_4()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__4");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_5()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__5");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }

        [Fact]
        public void MethodWithSwitchAndDefault_6()
        {
            MethodDef md = _testClassTD.FindMethod("Test_SWITCH__6");

            ControlFlowGraph cfg = ControlFlowGraph.Build(md);
        }
    }
}
