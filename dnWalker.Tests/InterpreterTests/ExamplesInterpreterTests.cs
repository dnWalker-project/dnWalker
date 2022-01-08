using dnWalker.Tests.ExampleTests;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests
{
    [Trait("Category", "Interpreter")]
    public class ExamplesInterpreterTests : InterpreterTestBase
    {
        //protected const string ExamplesAssemblyFileFormat = @"..\..\..\..\Examples\bin\{0}\net5.0\Examples.dll";
        protected const string AssemblyFilePath = @"..\..\..\..\Examples\bin\Release\framework\Examples.Framework.exe";

        protected static Lazy<DefinitionProvider> LazyDefinitionProvider = new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(AssemblyFilePath)));

        public ExamplesInterpreterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper, LazyDefinitionProvider.Value)
        {
        }

        protected override void TestAndCompare(string methodName, params object[] args)
        {
            methodName = "Examples.Interpreter.IntepreterTests." + methodName;
            base.TestAndCompare(methodName, args);
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(-1.0d)]
        [InlineData(1.0d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(2.0d)]
        [InlineData(3.0d)]
        [InlineData(4.0d)]
        [InlineData(5.0d)]
        [InlineData(6.0d)]
        public void Test_SWITCH__6_Double(object arg0) { TestAndCompare("Test_SWITCH__6_Double", arg0); }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(-1.0f)]
        [InlineData(1.0f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NaN)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(2.0f)]
        [InlineData(3.0f)]
        [InlineData(4.0f)]
        [InlineData(5.0f)]
        [InlineData(6.0f)]
        public void Test_SWITCH__6_Single(object arg0) { TestAndCompare("Test_SWITCH__6_Single", arg0); }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(1L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(2L)]
        [InlineData(3L)]
        [InlineData(4L)]
        [InlineData(5L)]
        [InlineData(6L)]
        public void Test_SWITCH__6_Int64(object arg0) { TestAndCompare("Test_SWITCH__6_Int64", arg0); }
    }
}
