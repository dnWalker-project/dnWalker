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
                .ExportXmlData("PrimitiveValueChanged_Fresh")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.PrimitiveValueChanged_Fresh");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("PrimitiveValueChanged_Fresh.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

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
                .ExportXmlData("PrimitiveValueChanged_Input")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.PrimitiveValueChanged_Input");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("PrimitiveValueChanged_Input.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

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
            Action act = () => FieldSchema.RefFieldChanged_Input(instance);
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
            FieldSchema.RefFieldChanged_Input(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().BeSameAs(instance);
        }
    }
}", true)]
        [InlineData(@"using FluentAssertions;
using Xunit;
using Moq;
using Examples.TestGeneration;

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
            Action act = () => FieldSchema.RefFieldChanged_Input(instance);
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
            FieldSchema.RefFieldChanged_Input(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().BeSameAs(instance);
        }
    }
}", false)]
        public void RefFieldChanged_Input(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("RefFieldChanged_Input")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.RefFieldChanged_Input");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("RefFieldChanged_Input.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

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
            Action act = () => FieldSchema.RefFieldChanged_Fresh(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // Arrange variable: expectedField__refField
            FieldSchema expectedField__refField = new FieldSchema();
            expectedField__refField.SetPrivate(""_primitiveValueField"", 0);
            expectedField__refField.SetPrivate(""_refField"", null);

            // act
            FieldSchema.RefFieldChanged_Fresh(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().NotBeNull();
            instance.GetPrivate(""_refField"").Should().BeEquivalentTo(expectedField__refField);
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
            Action act = () => FieldSchema.RefFieldChanged_Fresh(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();

            // Arrange variable: fieldSchema0
            FieldSchema fieldSchema0 = null;

            // Arrange variable: int0
            int int0 = 0;

            // Arrange variable: expectedField__refField
            FieldSchema expectedField__refField = new FieldSchema();
            expectedField__refField.SetPrivate(""_primitiveValueField"", int0);
            expectedField__refField.SetPrivate(""_refField"", fieldSchema0);

            // act
            FieldSchema.RefFieldChanged_Fresh(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().NotBeNull();
            instance.GetPrivate(""_refField"").Should().BeEquivalentTo(expectedField__refField);
        }
    }
}", false)]
        public void RefFieldChanged_Fresh(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("RefFieldChanged_Fresh")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.RefFieldChanged_Fresh");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("RefFieldChanged_Fresh.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

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

namespace TestNamespace
{
    public class TestClass
    {
        
        [Fact]
        public void Test_1_Exception()
        {
            // arrange
            // Arrange variable: fieldSchema0
            FieldSchema fieldSchema0 = new FieldSchema();

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();
            instance.SetPrivate(""_refField"", fieldSchema0);

            // act
            Action act = () => FieldSchema.RefFieldChanged_Null(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: fieldSchema0
            FieldSchema fieldSchema0 = new FieldSchema();

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();
            instance.SetPrivate(""_refField"", fieldSchema0);

            // act
            FieldSchema.RefFieldChanged_Null(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().BeNull();
        }
    }
}", true)]
        [InlineData(@"using FluentAssertions;
using Xunit;
using Moq;
using Examples.TestGeneration;

namespace TestNamespace
{
    public class TestClass
    {
        
        [Fact]
        public void Test_1_Exception()
        {
            // arrange
            // Arrange variable: fieldSchema0
            FieldSchema fieldSchema0 = new FieldSchema();

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();
            instance.SetPrivate(""_refField"", fieldSchema0);

            // act
            Action act = () => FieldSchema.RefFieldChanged_Null(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: fieldSchema0
            FieldSchema fieldSchema0 = new FieldSchema();

            // Arrange variable: instance
            FieldSchema instance = new FieldSchema();
            instance.SetPrivate(""_refField"", fieldSchema0);

            // Arrange variable: expectedField__refField
            FieldSchema expectedField__refField = null;

            // act
            FieldSchema.RefFieldChanged_Null(instance);
            // assert
            instance.GetPrivate(""_refField"").Should().BeEquivalentTo(expectedField__refField);
        }
    }
}", false)]
        public void RefFieldChanged_Null(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("RefFieldChanged_Null")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.FieldSchema.RefFieldChanged_Null");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Should().BeNull();
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("RefFieldChanged_Null.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

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
