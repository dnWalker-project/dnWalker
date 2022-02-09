using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TestGenerator.Templates;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{

    public class SimpleDependency_ArrangeTemplateTests : TemplateTestBase
    {

        private class SimpleDependencyArrangeTemplate : TemplateBase
        {
            private readonly SimpleDependency _dependency;

            public SimpleDependencyArrangeTemplate(SimpleDependency dependency)
            {
                _dependency = dependency;
            }

            public override string TransformText()
            {
                WriteArrangeSimpleDepencency(_dependency);

                return base.TransformText().Trim();
            }
        }


        private readonly MethodSignature Primitive_NoArgs;
        private readonly MethodSignature Primitive_Args;
        private readonly MethodSignature Complex_NoArgs;
        private readonly MethodSignature Complex_Args;

        private readonly MethodSignature AbstractClass_Method;
        private readonly MethodSignature ConcreteClass_Method;

        private readonly TypeSignature TestInterfaceType;
        private readonly TypeSignature DoubleListType;
        private readonly TypeSignature DoubleType;
        
        private readonly TypeSignature AbstractClassType;
        private readonly TypeSignature ConcreteClassType;


        public SimpleDependency_ArrangeTemplateTests()
        {
            MethodTranslator methodTranslator = new MethodTranslator(DefinitionProvider);
            TypeTranslator typeTranslator = new TypeTranslator(DefinitionProvider);

            TestInterfaceType = typeTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ITestInterface");
            DoubleListType = typeTranslator.FromString("System.Collections.Generic.List`1<System.Double>");
            DoubleType = typeTranslator.FromString("System.Double");

            AbstractClassType = typeTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.AbstractClass");
            ConcreteClassType = typeTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ConcreteClass");

            Primitive_NoArgs = methodTranslator.FromString("System.Int32 dnWalker.TestGenerator.Tests.Templates.ITestInterface::PrimitiveValue_Method()");
            Primitive_Args = methodTranslator.FromString("System.Int32 dnWalker.TestGenerator.Tests.Templates.ITestInterface::PrimitiveValue_Method(System.Double,System.String)");

            Complex_NoArgs = methodTranslator.FromString("System.Collections.Generic.List`1<System.String> dnWalker.TestGenerator.Tests.Templates.ITestInterface::ComplexValue_Method()");
            Complex_Args =   methodTranslator.FromString("System.Collections.Generic.List`1<System.String> dnWalker.TestGenerator.Tests.Templates.ITestInterface::ComplexValue_Method(System.Collections.Generic.List`1<System.Double>)");

            AbstractClass_Method = methodTranslator.FromString("System.Int32 dnWalker.TestGenerator.Tests.Templates.AbstractClass::Method()");
            ConcreteClass_Method = methodTranslator.FromString("System.Int32 dnWalker.TestGenerator.Tests.Templates.ConcreteClass::Method()");
        }

        [Theory]
        [InlineData(null, "// Arrange variable: var_0x00000001\r\nint var_0x00000001 = 0;")]
        [InlineData(10, "// Arrange variable: var_0x00000001\r\nint var_0x00000001 = 10;")]
        [InlineData(-1, "// Arrange variable: var_0x00000001\r\nint var_0x00000001 = -1;")]
        [InlineData(0, "// Arrange variable: var_0x00000001\r\nint var_0x00000001 = 0;")]
        public void Test_PrimitiveValueParameter(int? value, string expected)
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            var i1 = set.CreateInt32Parameter(0x00000001);
            i1.Value = value;

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(i1)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "// Arrange variable: var_0x00000001\r\ndouble[] var_0x00000001 = null;")]
        [InlineData(new double[0], "// Arrange variable: var_0x00000001\r\ndouble[] var_0x00000001 = new double[0];")]
        [InlineData(new double[] { -5, 0, 10 }, "// Arrange variable: var_0x00000001\r\ndouble[] var_0x00000001 = new double[3] { -5, 0, 10 };")]
        public void Test_ArrayOfPrimitiveValueParameters(double[] srcArray, string expected)
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            var ap = set.CreateArrayParameter(DoubleType, 0x00000001, srcArray == null, srcArray?.Length);
            if (srcArray != null)
            {
                for (int i = 0; i < srcArray.Length; ++i)
                {
                    var dp = set.CreateDoubleParameter(i + 16);
                    dp.Value = srcArray[i];
                    ap.SetItem(i, dp);
                }
            }

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(ap)).TransformText();
            result.Should().Be(expected);
        }

        [Fact]
        public void Test_InterfaceParameter_Null()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 0x00000001, isNull: true);
            string expected = "// Arrange variable: var_0x00000001\r\nITestInterface var_0x00000001 = null;";

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Fact]
        public void Test_InterfaceParameter_NoMethods()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 0x00000001, isNull: false);
            
            string expected =
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;";

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[0],
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333 },
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method())
    .Returns(333);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333, 444 },
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method())
    .Returns(333)
    .Returns(444);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        public void Test_InterfaceParameter_PrimitiveReturn_NoArgs(int[] results, string expected)
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 1, isNull: false);

            for (int i = 0; i < results.Length; ++i)
            {
                IInt32Parameter ip = set.CreateInt32Parameter(i + 10);
                ip.Value = results[i];

                op.SetMethodResult(Primitive_NoArgs, i, ip);
            }


            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 333 },
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method(It.Any<double>(), It.Any<string>()))
    .Returns(333);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333, 444 },
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method(It.Any<double>(), It.Any<string>()))
    .Returns(333)
    .Returns(444);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        public void Test_InterfaceParameter_PrimitiveReturn_WithArgs(int[] results, string expected)
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 1, isNull: false);

            for (int i = 0; i < results.Length; ++i)
            {
                IInt32Parameter ip = set.CreateInt32Parameter(i + 10);
                ip.Value = results[i];

                op.SetMethodResult(Primitive_Args, i, ip);
            }


            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Fact]
        public void Test_InterfaceParameter_ComplexReturn_NoArgs()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 1, isNull: false);

            int[] refs = new int[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 };

            for (int i = 0; i < refs.Length; ++i)
            {
                IObjectParameter mr = set.CreateObjectParameter(DoubleListType, refs[i], isNull: false);
                op.SetMethodResult(Complex_NoArgs, i, mr);
            }

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.ComplexValue_Method())
    .Returns(var_0x00000010)
    .Returns(var_0x00000011)
    .Returns(var_0x00000012)
    .Returns(var_0x00000013)
    .Returns(var_0x00000014)
    .Returns(var_0x00000015);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;");
        }

        [Fact]
        public void Test_InterfaceParameter_ComplexReturn_WithArgs()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(TestInterfaceType, 1, isNull: false);

            int[] refs = new int[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 };

            for (int i = 0; i < refs.Length; ++i)
            {
                IObjectParameter mr = set.CreateObjectParameter(DoubleListType, refs[i], isNull: false);
                op.SetMethodResult(Complex_Args, i, mr);
            }

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
Mock<ITestInterface> mock_var_0x00000001 = new Mock<ITestInterface>();
mock_var_0x00000001.SetupSequence(o => o.ComplexValue_Method(It.Any<List<double>>()))
    .Returns(var_0x00000010)
    .Returns(var_0x00000011)
    .Returns(var_0x00000012)
    .Returns(var_0x00000013)
    .Returns(var_0x00000014)
    .Returns(var_0x00000015);
ITestInterface var_0x00000001 = mock_var_0x00000001.Object;");
        }

        [Fact]
        public void Test_AbstractTypeUsesMock()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(AbstractClassType, 1, isNull: false);

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
Mock<AbstractClass> mock_var_0x00000001 = new Mock<AbstractClass>();
AbstractClass var_0x00000001 = mock_var_0x00000001.Object;");
        }

        [Fact]
        public void Test_ConreteTypeWithoutMethodsIsCreatedDirectly()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(ConcreteClassType, 1, isNull: false);

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be("// Arrange variable: var_0x00000001\r\nConcreteClass var_0x00000001 = new ConcreteClass();");
        }

        [Fact]
        public void Test_ConreteTypeWithMethodsUsesMock()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(ConcreteClassType, 1, isNull: false);

            op.SetMethodResult(ConcreteClass_Method, 0, set.CreateInt32Parameter(0x10));

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
Mock<ConcreteClass> mock_var_0x00000001 = new Mock<ConcreteClass>();
mock_var_0x00000001.SetupSequence(o => o.Method())
    .Returns(0);
ConcreteClass var_0x00000001 = mock_var_0x00000001.Object;");
        }

        [Fact]
        public void Test_ArrangeFields_ConcreteType()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(ConcreteClassType, 1, isNull: false);

            IInt32Parameter i1 = set.CreateInt32Parameter(0x11);
            i1.Value = 55;

            IInt32Parameter i2 = set.CreateInt32Parameter(0x12);
            i2.Value = 65;

            op.SetField("_field1", i1);
            op.SetField("_field2", i2);

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
ConcreteClass var_0x00000001 = new ConcreteClass();
var_0x00000001.SetPrivate(""_field1"", 55);
var_0x00000001.SetPrivate(""_field2"", 65);");
        }


        [Fact]
        public void Test_ArrangeFields_AbstractType()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter op = set.CreateObjectParameter(AbstractClassType, 1, isNull: false);

            IInt32Parameter i1 = set.CreateInt32Parameter(0x11);
            i1.Value = 55;

            IInt32Parameter i2 = set.CreateInt32Parameter(0x12);
            i2.Value = 65;

            op.SetField("_field1", i1);
            op.SetField("_field2", i2);

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(
@"// Arrange variable: var_0x00000001
Mock<AbstractClass> mock_var_0x00000001 = new Mock<AbstractClass>();
AbstractClass var_0x00000001 = mock_var_0x00000001.Object;
var_0x00000001.SetPrivate(""_field1"", 55);
var_0x00000001.SetPrivate(""_field2"", 65);");
        }
    }
}
