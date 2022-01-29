using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TestGenerator.Templates;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public interface TestInterface
    {
        int PrimitiveValue_Method();
        int PrimitiveValue_Method(double dbl, string str);
        List<string> ComplexValue_Method(List<double> dbls);
        List<string> ComplexValue_Method();
    }

    public class ArrangeTemplateTests
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

        [Theory]
        [InlineData(null, "int var_0x00000001 = 0;")]
        [InlineData(10, "int var_0x00000001 = 10;")]
        [InlineData(-1, "int var_0x00000001 = -1;")]
        [InlineData(0, "int var_0x00000001 = 0;")]
        public void Test_PrimitiveValueParameter(int? value, string expected)
        {
            IParameterContext context = new ParameterContext();
            var i1 = context.CreateInt32Parameter(0x00000001);
            i1.Value = value;

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(i1)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "double[] var_0x00000001 = null;")]
        [InlineData(new double[0], "double[] var_0x00000001 = new double[0];")]
        [InlineData(new double[] { -5, 0, 10 }, "double[] var_0x00000001 = new double[3] { -5, 0, 10 };")]
        public void Test_ArrayOfPrimitiveValueParameters(double[] srcArray, string expected)
        {
            IParameterContext context = new ParameterContext();
            var ap = context.CreateArrayParameter(0x00000001, "System.Double", srcArray == null, srcArray?.Length);
            if (srcArray != null)
            {
                for (int i = 0; i < srcArray.Length; ++i)
                {
                    var dp = context.CreateDoubleParameter(i + 16);
                    dp.Value = srcArray[i];
                    ap.SetItem(i, dp);
                }
            }

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(ap)).TransformText();
            result.Should().Be(expected);
        }

        private static readonly MethodSignature Primitive_NoArgs = "System.Int32 dnWalker.TestGenerator.Tests.Templates.TestInterface::PrimitiveValue_Method()";
        private static readonly MethodSignature Primitive_Args = "System.Int32 dnWalker.TestGenerator.Tests.Templates.TestInterface::PrimitiveValue_Method(System.Double,System.String)";

        [Fact]
        public void Test_InterfaceParameter_Null()
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter op = context.CreateObjectParameter(0x00000001, "dnWalker.TestGenerator.Tests.Templates.TestInterface", isNull: true);
            string expected = "TestInterface var_0x00000001 = null;";

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Fact]
        public void Test_InterfaceParameter_NoMethods()
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter op = context.CreateObjectParameter(0x00000001, "dnWalker.TestGenerator.Tests.Templates.TestInterface", isNull: false);
            
            string expected =
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
TestInterface var_0x00000001 = mock_var_0x00000001.Object;";

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[0],
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
TestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333 },
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method())
    .Returns(333);
TestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333, 444 },
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method())
    .Returns(333)
    .Returns(444);
TestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        public void Test_InterfaceParameter_PrimitiveReturn_NoArgs(int[] results, string expected)
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter op = context.CreateObjectParameter(1, "dnWalker.TestGenerator.Tests.Templates.TestInterface", isNull: false);

            for (int i = 0; i < results.Length; ++i)
            {
                IInt32Parameter ip = context.CreateInt32Parameter(i + 10);
                ip.Value = results[i];

                op.SetMethodResult(Primitive_NoArgs, i, ip);
            }


            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 333 },
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method(It.Any<double>(), It.Any<string>()))
    .Returns(333);
TestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        [InlineData(new int[] { 333, 444 },
@"Mock<TestInterface> mock_var_0x00000001 = new Mock<TestInterface>();
mock_var_0x00000001.SetupSequence(o => o.PrimitiveValue_Method(It.Any<double>(), It.Any<string>()))
    .Returns(333)
    .Returns(444);
TestInterface var_0x00000001 = mock_var_0x00000001.Object;")]
        public void Test_InterfaceParameter_PrimitiveReturn_WithArgs(int[] results, string expected)
        {
            IParameterContext context = new ParameterContext();
            IObjectParameter op = context.CreateObjectParameter(1, "dnWalker.TestGenerator.Tests.Templates.TestInterface", isNull: false);

            for (int i = 0; i < results.Length; ++i)
            {
                IInt32Parameter ip = context.CreateInt32Parameter(i + 10);
                ip.Value = results[i];

                op.SetMethodResult(Primitive_Args, i, ip);
            }


            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(op)).TransformText();
            result.Should().Be(expected);
        }
    }
}
