using dnWalker.TypeSystem;

using MMC;

using System;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests
{
    [Trait("Category", "PatternMatching")]
    public class PatternMatchingTests : InterpreterTestBase
    {
        //protected const string ExamplesAssemblyFileFormat = @"..\..\..\..\Examples\bin\{0}\net5.0\Examples.dll";
        protected const string AssemblyFilePath = @"..\..\..\..\Examples\bin\Release\framework\Examples.Framework.exe";

        private static Lazy<DefinitionProvider> Lazy = new Lazy<DefinitionProvider>(() => new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilePath)));

        public PatternMatchingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper, Lazy.Value)
        {
        }

        protected override void TestAndCompare(string methodName, params object[] args)
        {
            methodName = "Examples.Interpreter.PatternMatching." + methodName;
            base.TestAndCompare(methodName, args);
        }

        [Theory]
        [InlineData("circle")]
        [InlineData("square")]
        [InlineData("large-circle")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("unknown")]
        public void CreateShape(string shapeName)
        {
            TestAndCompare("CreateShape", shapeName);
        }

        /*public void ComputeAreaModernIs()
        {

        }*/
    }
}
