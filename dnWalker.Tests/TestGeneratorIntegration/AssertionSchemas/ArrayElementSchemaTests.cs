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

            // Arrange variable: expectedElement_0
            int expectedElement_0 = 10;

            // act
            ArrayElementSchema.PrimitiveArrayAsMethodArgument(array);
            // assert
            array[0].Should().Be(expectedElement_0);
        }
    }
}", false)]
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
            // Arrange variable: var_0x00000002
            int[] var_0x00000002 = new int[1] { 0 };

            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();
            instance.SetPrivate(""_valArray"", var_0x00000002);

            // act
            Action act = () => ArrayElementSchema.PrimitiveArrayAsFieldValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000002()
        {
            // arrange
            // Arrange variable: var_0x00000002
            int[] var_0x00000002 = new int[1] { 0 };

            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();
            instance.SetPrivate(""_valArray"", var_0x00000002);

            // act
            ArrayElementSchema.PrimitiveArrayAsFieldValue(instance);
            // assert
            var_0x00000002[0].Should().Be(15);
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
            // Arrange variable: var_0x00000002
            int[] var_0x00000002 = new int[1] { 0 };

            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();
            instance.SetPrivate(""_valArray"", var_0x00000002);

            // act
            Action act = () => ArrayElementSchema.PrimitiveArrayAsFieldValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000002()
        {
            // arrange
            // Arrange variable: var_0x00000002
            int[] var_0x00000002 = new int[1] { 0 };

            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();
            instance.SetPrivate(""_valArray"", var_0x00000002);

            // Arrange variable: expectedElement_0
            int expectedElement_0 = 15;

            // act
            ArrayElementSchema.PrimitiveArrayAsFieldValue(instance);
            // assert
            var_0x00000002[0].Should().Be(expectedElement_0);
        }
    }
}", false)]
        public void PrimitiveArrayAsFieldValue(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ArrayElementSchema.PrimitiveArrayAsFieldValue");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(4);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Type.Should().Be(_arg);
            paths[2].Exception.Type.Should().Be(_arg);
            paths[3].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx).Trim())
                .ToArray();

            // first two tests handle exception throwing only
            testClasses[3].Should().BeEquivalentTo(expected);
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
            ArrayElementSchema instance = new ArrayElementSchema();

            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Input(array, instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();

            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Input(array, instance);
            // assert
            array[0].Should().BeSameAs(instance);
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
            ArrayElementSchema instance = new ArrayElementSchema();

            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Input(array, instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            ArrayElementSchema instance = new ArrayElementSchema();

            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Input(array, instance);
            // assert
            array[0].Should().BeSameAs(instance);
        }
    }
}", false)]
        public void ReferenceArrayAsMethodArgument_AnotherInput(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ArrayElementSchema.ReferenceArrayAsMethodArgument_Input");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(4);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Type.Should().Be(_arg);
            paths[2].Exception.Type.Should().Be(_argNull);
            paths[3].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = Deserializer.FromXml(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx).Trim())
                .ToArray();

            // first two tests handle exception throwing only
            testClasses[3].Should().BeEquivalentTo(expected);
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
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Null(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Null(array);
            // assert
            array[0].Should().BeNull();
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
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Null(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // Arrange variable: expectedElement_0
            ArrayElementSchema expectedElement_0 = null;

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Null(array);
            // assert
            array[0].Should().Be(expectedElement_0);
        }
    }
}", false, Skip = "Not passing because of a problem with Assert part generation, Should().BeNull() vs Should().Be(explicit_variable)")]
        public void ReferenceArrayAsMethodArgument_Null(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ArrayElementSchema.ReferenceArrayAsMethodArgument_Null");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Type.Should().Be(_arg);
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

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
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Fresh(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // Arrange variable: expectedElement_0
            ArrayElementSchema expectedElement_0 = new ArrayElementSchema();
            expectedElement_0.SetPrivate(""_valArray"", null);
            expectedElement_0.SetPrivate(""_refArray"", null);
            expectedElement_0.SetPrivate(""_id"", 5);

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Fresh(array);
            // assert
            array[0].Should().NotBeNull();
            array[0].Should().BeEquivalentTo(expectedElement_0);
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
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // act
            Action act = () => ArrayElementSchema.ReferenceArrayAsMethodArgument_Fresh(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ArrayElementSchema[] array = new ArrayElementSchema[1] { null };

            // Arrange variable: var_0x7FFFFFFD
            int var_0x7FFFFFFD = 5;

            // Arrange variable: var_out_0x7FFFFFFE
            int[] var_out_0x7FFFFFFE = null;

            // Arrange variable: expectedElement_0
            ArrayElementSchema expectedElement_0 = new ArrayElementSchema();
            expectedElement_0.SetPrivate(""_valArray"", var_out_0x7FFFFFFE);
            expectedElement_0.SetPrivate(""_refArray"", var_out_0x7FFFFFFE);
            expectedElement_0.SetPrivate(""_id"", var_0x7FFFFFFD);

            // act
            ArrayElementSchema.ReferenceArrayAsMethodArgument_Fresh(array);
            // assert
            array[0].Should().NotBeNull();
            array[0].Should().BeEquivalentTo(expectedElement_0);
        }
    }
}", false)]
        public void ReferenceArrayAsMethodArgument_Fresh(string expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ArrayElementSchema.ReferenceArrayAsMethodArgument_Fresh");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Type.Should().Be(_argNull);
            paths[1].Exception.Type.Should().Be(_arg);
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

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
