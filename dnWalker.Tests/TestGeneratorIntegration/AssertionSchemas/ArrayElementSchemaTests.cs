using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Parameters;
using dnWalker.TestGenerator;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.XUnitFramework;

using FluentAssertions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.TestGeneratorIntegration.AssertionSchemas
{
    public class ArrayElementSchemaTests : IntegrationTestBase
    {
        private readonly ITypeDefOrRef _argNull;
        private readonly ITypeDefOrRef _arg;

        public ArrayElementSchemaTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _argNull = DefinitionProvider.GetTypeDefinition("System.ArgumentNullException");
            _arg = DefinitionProvider.GetTypeDefinition("System.ArgumentException");
        }

        [Theory]
        [InlineData(@"using FluentAssertions;
using Xunit;
using Moq;
using Examples.TestGeneration;
using System;

namespace TestNamespace
{
    public class TestClass
    {
        
        [Fact]
        public void Test_1_Exception()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // act
            Action act = () => ArrayElementSchema.PrimitiveArrayAsMethodArgument(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // act
            ArrayElementSchema.PrimitiveArrayAsMethodArgument(array);
            // assert
            array[0].Should().Be(10);
        }
    }
}", true)]
        public void PrimitiveArrayAsMethodArgument(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ArrayElementSchema.PrimitiveArrayAsMethodArgument");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Type.Should().Be(_arg);
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx).Trim())
                .ToArray();

            // first two tests handle exception throwing only
            testClasses[2].Should().BeEquivalentTo(expected);
        }
    }
}
