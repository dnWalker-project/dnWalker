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
    public class FieldSchemaTests : IntegrationTestBase
    {

        private readonly ITypeDefOrRef _argNull;
        private readonly ITypeDefOrRef _arg;

        public FieldSchemaTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            Action act = () => FieldSchema.PrimitiveValueChanged_Fresh(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            FieldSchema.PrimitiveValueChanged_Fresh(instance);
            // assert
            instance.GetPrivate(""_primitiveValueField"").Should().Be(15);
        }
    }
}", true)]
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
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            Action act = () => FieldSchema.PrimitiveValueChanged_Fresh(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // Arrange variable: expectedField__primitiveValueField
            int expectedField__primitiveValueField = 15;

            // act
            FieldSchema.PrimitiveValueChanged_Fresh(instance);
            // assert
            instance.GetPrivate(""_primitiveValueField"").Should().Be(expectedField__primitiveValueField);
        }
    }
}", false)]
        public void PrimitiveValueChanged_Fresh(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.PrimitiveValueChanged_Fresh");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

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
            testClasses[1].Should().BeEquivalentTo(expected);
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
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            Action act = () => FieldSchema.PrimitiveValueChanged_Input(instance, 0);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            FieldSchema.PrimitiveValueChanged_Input(instance, 0);
            // assert
            instance.GetPrivate(""_primitiveValueField"").Should().Be(0);
        }
    }
}", true)]
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
            // Arrange variable: value
            int value = 0;

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            Action act = () => FieldSchema.PrimitiveValueChanged_Input(instance, value);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: value
            int value = 0;

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // act
            FieldSchema.PrimitiveValueChanged_Input(instance, value);
            // assert
            instance.GetPrivate(""_primitiveValueField"").Should().Be(value);
        }
    }
}", false)]
        public void PrimitiveValueChanged_Input(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.PrimitiveValueChanged_Input");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

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
            testClasses[1].Should().BeEquivalentTo(expected);
        }
    }
}
