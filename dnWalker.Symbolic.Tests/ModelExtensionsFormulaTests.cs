using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Symbolic.Tests
{
    public class ModelExtensionsFormulaTests
    {
        public class TestClass
        {
            public void TestMethod(double dblArg, int intArg, TestClass other, String strArg)
            {

            }

            public Double PrimitiveField;
            public TestClass? RefField;
            public Double[]? PrimitiveArrField;
            public TestClass[]? RefArrField;
            public String? StringField;

            public Double MockedMethod()
            {
                return Math.Ceiling(PrimitiveField);
            }
        }



        private readonly ExpressionFactory _expressionFactory;
        private readonly ModuleDef _testModule;
        private readonly TypeDef _testType;
        private readonly MethodDef _testMethod;

        public ModelExtensionsFormulaTests()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            _testModule = ModuleDefMD.Load(typeof(ModelExtensionsFormulaTests).Module, ctx);
            _expressionFactory = new CustomModuleExpressionFactory(_testModule);
            _testType = _testModule.FindThrow("dnWalker.Symbolic.Tests.ModelExtensionsFormulaTests+TestClass", true);
            _testMethod = _testType.FindMethod("TestMethod");
        }

        [Fact]
        public void EmptyModelIsTrue()
        {
            IReadOnlyModel model = new Model();
            model.GetFormula(_expressionFactory).ToString().Should().Be("True");
        }

        [Fact]
        public void SetSinglePrimitiveMethodArgument()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[1]), ValueFactory.GetValue(5.0));

            model.GetFormula(_expressionFactory).ToString().Should().Be("(dblArg == 5)");
        }

        [Fact]
        public void SetMultiplePrimitiveMethodArguments()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[1]), ValueFactory.GetValue(5.0));
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[2]), ValueFactory.GetValue(-3));

            model.GetFormula(_expressionFactory).ToString().Should().Be("((dblArg == 5) & (intArg == -3))");
        }

        [Fact]
        public void SetStringMethodArgumentToNull()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), StringValue.Null);

            model.GetFormula(_expressionFactory).ToString().Should().Be("(strArg == null)");
        }

        [Fact]
        public void SetStringMethodArgumentToEmpty()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), new StringValue(""));

            model.GetFormula(_expressionFactory).ToString().Should().Be("(strArg == \"\")");
        }

        [Fact]
        public void SetStringMethodArgumentToNonEmpty()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), new StringValue("Hello world"));

            model.GetFormula(_expressionFactory).ToString().Should().Be("(strArg == \"Hello world\")");
        }

        [Fact]
        public void SetReferenceMethodArgumentToNull()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), Location.Null);

            model.GetFormula(_expressionFactory).ToString().Should().Be("(other == null)");
        }

        [Fact]
        public void SetReferenceMethodArgumentToNonNull()
        {
            MethodDef method = _testMethod;

            Model model = new Model();
            IObjectHeapNode node = model.HeapInfo.InitializeObject(_testType.ToTypeSig());

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), node.Location);

            model.GetFormula(_expressionFactory).ToString().Should().Be("(other != null)");
        }

        [Fact]
        public void SetReferenceMethodArgumentToNonNullWithInitializedFields()
        {
            MethodDef method = _testMethod;
            TypeDef type = _testType;

            Model model = new Model();
            IObjectHeapNode node = model.HeapInfo.InitializeObject(type.ToTypeSig());
            node.SetField(type.FindField("RefField"), Location.Null);
            node.SetField(type.FindField("StringField"), new StringValue("NotNullOrEmpty"));

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), node.Location);

            model.GetFormula(_expressionFactory).ToString().Should().Be("(((other != null) & (other.RefField == null)) & (other.StringField == \"NotNullOrEmpty\"))");
        }

        [Fact]
        public void HeapNodeWithMultipleReferences()
        {
            MethodDef method = _testMethod;
            TypeDef type = _testType;

            Model model = new Model();
            IObjectHeapNode thisNode = model.HeapInfo.InitializeObject(type.ToTypeSig());
            IObjectHeapNode refFieldNode = model.HeapInfo.InitializeObject(type.ToTypeSig());

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[0]), thisNode.Location);
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), thisNode.Location);

            thisNode.SetField(type.FindField("RefField"), refFieldNode.Location);

            // we do not care for the tree from 'other' (e.g. other.RefField) since it is covered by the tree from $this$ and other == $this$
            model.GetFormula(_expressionFactory).ToString().Should().Be("((($this$ != null) & ($this$.RefField != null)) & (other == $this$))");
        }

        [Fact]
        public void SetMockedMethod()
        {
            MethodDef method = _testMethod;
            TypeDef type = _testType;
            MethodDef mockedMethod = type.FindMethod("MockedMethod");

            Model model = new Model();
            IObjectHeapNode thisNode = model.HeapInfo.InitializeObject(type.ToTypeSig());
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[0]), thisNode.Location);

            thisNode.SetMethodResult(mockedMethod, 1, ValueFactory.GetValue(5.0));
            thisNode.SetMethodResult(mockedMethod, 2, ValueFactory.GetValue(-5.0));

            model.GetFormula(_expressionFactory).ToString().Should().Be("((($this$ != null) & ($this$.MockedMethod(1) == 5)) & ($this$.MockedMethod(2) == -5))");
        }
    }
}
