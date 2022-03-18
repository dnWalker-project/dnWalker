using dnWalker.Concolic;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Parameters;
using dnWalker.TestGenerator;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.XUnitFramework;

using FluentAssertions;

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
    public class ReturnValueSchemaTests : IntegrationTestBase
    {
        public ReturnValueSchemaTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Theory]
        [InlineData(new string[]
        {
@"using FluentAssertions;
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
            // act
            Action act = () => ReturnValueSchema.PrimitiveValue(0);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // act
            var result = ReturnValueSchema.PrimitiveValue(0);
            // assert
            result.Should().Be(10);
        }
    }
}",
@"using FluentAssertions;
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
            // act
            Action act = () => ReturnValueSchema.PrimitiveValue(-1);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // act
            var result = ReturnValueSchema.PrimitiveValue(-1);
            // assert
            result.Should().Be(-1);
        }
    }
}"
        }, true)]
        [InlineData(new string[]
        {
@"using FluentAssertions;
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
            // Arrange variable: input
            int input = 0;

            // act
            Action act = () => ReturnValueSchema.PrimitiveValue(input);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: input
            int input = 0;

            // Arrange variable: expectedResult
            int expectedResult = 10;

            // act
            var result = ReturnValueSchema.PrimitiveValue(input);
            // assert
            result.Should().Be(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            // Arrange variable: input
            int input = -1;

            // act
            Action act = () => ReturnValueSchema.PrimitiveValue(input);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: input
            int input = -1;

            // act
            var result = ReturnValueSchema.PrimitiveValue(input);
            // assert
            result.Should().Be(input);
        }
    }
}"
        }, false)]
        public void PrimitiveValueReturn(string[] expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ReturnValueSchema.PrimitiveValue");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Should().BeNull();
            paths[1].Exception.Should().BeNull();

            // path 0 - return 10
            // path 1 - return -1

            explorer.ParameterStore.ExecutionSets[0].TryGetReturnValue(out IInt32Parameter? rv0).Should().BeTrue();
            explorer.ParameterStore.ExecutionSets[1].TryGetReturnValue(out IInt32Parameter? rv1).Should().BeTrue();

            rv0.Value.Should().Be(10);
            rv1.Value.Should().Be(-1);

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx).Trim())
                .ToArray();

            testClasses.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(new string[] {
@"using FluentAssertions;
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
            // act
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(null);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: expectedResult
            int[] expectedResult = new int[6] { 0, 1, 2, 3, 4, 5 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(null);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            int[] array = new int[0];

            // act
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[0];

            // Arrange variable: expectedResult
            int[] expectedResult = new int[6] { 0, 1, 2, 3, 4, 5 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(array);
        }
        
        [Fact]
        public void Test_3_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // act
            ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            array[0].Should().Be(10);
        }
    }
}"}, true)]
        [InlineData(new string[] {
@"using FluentAssertions;
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
            int[] array = null;

            // act
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            int[] array = null;

            // Arrange variable: var_0x7FFFFFF9
            int var_0x7FFFFFF9 = 5;

            // Arrange variable: var_0x7FFFFFFA
            int var_0x7FFFFFFA = 4;

            // Arrange variable: var_0x7FFFFFFB
            int var_0x7FFFFFFB = 3;

            // Arrange variable: var_0x7FFFFFFC
            int var_0x7FFFFFFC = 2;

            // Arrange variable: var_0x7FFFFFFD
            int var_0x7FFFFFFD = 1;

            // Arrange variable: var_0x7FFFFFFE
            int var_0x7FFFFFFE = 0;

            // Arrange variable: expectedResult
            int[] expectedResult = new int[6] { var_0x7FFFFFFE, var_0x7FFFFFFD, var_0x7FFFFFFC, var_0x7FFFFFFB, var_0x7FFFFFFA, var_0x7FFFFFF9 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            int[] array = new int[0];

            // act
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[0];

            // Arrange variable: var_0x7FFFFFF9
            int var_0x7FFFFFF9 = 5;

            // Arrange variable: var_0x7FFFFFFA
            int var_0x7FFFFFFA = 4;

            // Arrange variable: var_0x7FFFFFFB
            int var_0x7FFFFFFB = 3;

            // Arrange variable: var_0x7FFFFFFC
            int var_0x7FFFFFFC = 2;

            // Arrange variable: var_0x7FFFFFFD
            int var_0x7FFFFFFD = 1;

            // Arrange variable: var_0x7FFFFFFE
            int var_0x7FFFFFFE = 0;

            // Arrange variable: expectedResult
            int[] expectedResult = new int[6] { var_0x7FFFFFFE, var_0x7FFFFFFD, var_0x7FFFFFFC, var_0x7FFFFFFB, var_0x7FFFFFFA, var_0x7FFFFFF9 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            Action act = () => ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // act
            var result = ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(array);
        }
        
        [Fact]
        public void Test_3_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            int[] array = new int[1] { 0 };

            // Arrange variable: expectedElement_0
            int expectedElement_0 = 10;

            // act
            ReturnValueSchema.ArrayOfPrimitives(array);
            // assert
            array[0].Should().Be(expectedElement_0);
        }
    }
}"}, false)]
        public void ArrayOfPrimitiveValuesReturn(string[] expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ReturnValueSchema.ArrayOfPrimitives");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Should().BeNull();
            paths[1].Exception.Should().BeNull();
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx =>
                {
                    return testClassGenerator.GenerateTestClassFileContent(ctx).Trim();
                })
                .ToArray();

            testClasses.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(new string[] {
@"using FluentAssertions;
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
            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(null);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: expectedResult
            ReturnValueSchema expectedResult = new ReturnValueSchema();
            expectedResult.SetPrivate(""ValueField"", 10);
            expectedResult.SetPrivate(""RefField"", null);

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(null);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", 0);

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", 0);

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(instance);
        }
        
        [Fact]
        public void Test_3_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", 0);

            // act
            ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            instance.GetPrivate(""ValueField"").Should().Be(-1);
            instance.GetPrivate(""RefField"").Should().BeSameAs(instance);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", 5);

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", 5);

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            result.Should().BeNull();
        }
    }
}" }, true)]
        [InlineData(new string[] {
@"using FluentAssertions;
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
            ReturnValueSchema instance = null;

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: instance
            ReturnValueSchema instance = null;

            // Arrange variable: var_out_0x7FFFFFFD
            ReturnValueSchema var_out_0x7FFFFFFD = null;

            // Arrange variable: var_0x7FFFFFFE
            int var_0x7FFFFFFE = 10;

            // Arrange variable: expectedResult
            ReturnValueSchema expectedResult = new ReturnValueSchema();
            expectedResult.SetPrivate(""ValueField"", var_0x7FFFFFFE);
            expectedResult.SetPrivate(""RefField"", var_out_0x7FFFFFFD);

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            int var_0x00000002 = 0;

            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", var_0x00000002);

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: var_0x00000002
            int var_0x00000002 = 0;

            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", var_0x00000002);

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(instance);
        }
        
        [Fact]
        public void Test_3_FieldValues_0x00000001()
        {
            // arrange
            // Arrange variable: expectedField_ValueField
            int expectedField_ValueField = 0;

            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", expectedField_ValueField);

            // Arrange variable: expectedField_ValueField
            int expectedField_ValueField = -1;

            // act
            ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            instance.GetPrivate(""ValueField"").Should().Be(expectedField_ValueField);
            instance.GetPrivate(""RefField"").Should().BeSameAs(instance);
        }
    }
}",
@"using FluentAssertions;
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
            int var_0x00000002 = 5;

            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", var_0x00000002);

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: var_0x00000002
            int var_0x00000002 = 5;

            // Arrange variable: instance
            ReturnValueSchema instance = new ReturnValueSchema();
            instance.SetPrivate(""ValueField"", var_0x00000002);

            // Arrange variable: expectedResult
            ReturnValueSchema expectedResult = null;

            // act
            var result = ReturnValueSchema.ReferenceTypeValue(instance);
            // assert
            result.Should().BeNull();
        }
    }
}" }, false)]
        public void ReferenceTypeValuesReturn(string[] expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ReturnValueSchema.ReferenceTypeValue");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Should().BeNull();
            paths[1].Exception.Should().BeNull();
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx =>
                {
                    return testClassGenerator.GenerateTestClassFileContent(ctx).Trim();
                })
                .ToArray();

            testClasses.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(new string[] 
{ 
@"using FluentAssertions;
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
            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(null);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: var_out_0x7FFFFFFB
            ReturnValueSchema var_out_0x7FFFFFFB = new ReturnValueSchema();
            var_out_0x7FFFFFFB.SetPrivate(""ValueField"", 5);
            var_out_0x7FFFFFFB.SetPrivate(""RefField"", null);

            // Arrange variable: var_out_0x7FFFFFFE
            ReturnValueSchema var_out_0x7FFFFFFE = new ReturnValueSchema();
            var_out_0x7FFFFFFE.SetPrivate(""ValueField"", 10);
            var_out_0x7FFFFFFE.SetPrivate(""RefField"", null);

            // Arrange variable: expectedResult
            ReturnValueSchema[] expectedResult = new ReturnValueSchema[4] { var_out_0x7FFFFFFE, null, var_out_0x7FFFFFFB, null };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(null);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[0];

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[0];

            // Arrange variable: var_out_0x7FFFFFFB
            ReturnValueSchema var_out_0x7FFFFFFB = new ReturnValueSchema();
            var_out_0x7FFFFFFB.SetPrivate(""ValueField"", 5);
            var_out_0x7FFFFFFB.SetPrivate(""RefField"", null);

            // Arrange variable: var_out_0x7FFFFFFE
            ReturnValueSchema var_out_0x7FFFFFFE = new ReturnValueSchema();
            var_out_0x7FFFFFFE.SetPrivate(""ValueField"", 10);
            var_out_0x7FFFFFFE.SetPrivate(""RefField"", null);

            // Arrange variable: expectedResult
            ReturnValueSchema[] expectedResult = new ReturnValueSchema[4] { var_out_0x7FFFFFFE, null, var_out_0x7FFFFFFB, null };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(array);
        }
        
        [Fact]
        public void Test_3_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // Arrange variable: expectedElement_0
            ReturnValueSchema expectedElement_0 = new ReturnValueSchema();
            expectedElement_0.SetPrivate(""ValueField"", -1);
            expectedElement_0.SetPrivate(""RefField"", null);

            // act
            ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            array[0].Should().NotBeNull();
            array[0].Should().BeEquivalentTo(expectedElement_0);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[2] { null, null };

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[2] { null, null };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().BeNull();
        }
    }
}"
}, true)]
        [InlineData(new string[] 
{
@"using FluentAssertions;
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
            ReturnValueSchema[] array = null;

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = null;

            // Arrange variable: var_0x7FFFFFFD
            int var_0x7FFFFFFD = 10;

            // Arrange variable: var_0x7FFFFFFA
            int var_0x7FFFFFFA = 5;

            // Arrange variable: var_out_0x7FFFFFFC
            ReturnValueSchema var_out_0x7FFFFFFC = null;

            // Arrange variable: var_out_0x7FFFFFFB
            ReturnValueSchema var_out_0x7FFFFFFB = new ReturnValueSchema();
            var_out_0x7FFFFFFB.SetPrivate(""ValueField"", var_0x7FFFFFFA);
            var_out_0x7FFFFFFB.SetPrivate(""RefField"", var_out_0x7FFFFFFC);

            // Arrange variable: var_out_0x7FFFFFFE
            ReturnValueSchema var_out_0x7FFFFFFE = new ReturnValueSchema();
            var_out_0x7FFFFFFE.SetPrivate(""ValueField"", var_0x7FFFFFFD);
            var_out_0x7FFFFFFE.SetPrivate(""RefField"", var_out_0x7FFFFFFC);

            // Arrange variable: expectedResult
            ReturnValueSchema[] expectedResult = new ReturnValueSchema[4] { var_out_0x7FFFFFFE, var_out_0x7FFFFFFC, var_out_0x7FFFFFFB, var_out_0x7FFFFFFC };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[0];

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[0];

            // Arrange variable: var_0x7FFFFFFD
            int var_0x7FFFFFFD = 10;

            // Arrange variable: var_0x7FFFFFFA
            int var_0x7FFFFFFA = 5;

            // Arrange variable: var_out_0x7FFFFFFC
            ReturnValueSchema var_out_0x7FFFFFFC = null;

            // Arrange variable: var_out_0x7FFFFFFB
            ReturnValueSchema var_out_0x7FFFFFFB = new ReturnValueSchema();
            var_out_0x7FFFFFFB.SetPrivate(""ValueField"", var_0x7FFFFFFA);
            var_out_0x7FFFFFFB.SetPrivate(""RefField"", var_out_0x7FFFFFFC);

            // Arrange variable: var_out_0x7FFFFFFE
            ReturnValueSchema var_out_0x7FFFFFFE = new ReturnValueSchema();
            var_out_0x7FFFFFFE.SetPrivate(""ValueField"", var_0x7FFFFFFD);
            var_out_0x7FFFFFFE.SetPrivate(""RefField"", var_out_0x7FFFFFFC);

            // Arrange variable: expectedResult
            ReturnValueSchema[] expectedResult = new ReturnValueSchema[4] { var_out_0x7FFFFFFE, var_out_0x7FFFFFFC, var_out_0x7FFFFFFB, var_out_0x7FFFFFFC };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(array);
        }
        
        [Fact]
        public void Test_3_ArrayElements_0x00000001()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[1] { null };

            // Arrange variable: var_out_0x7FFFFFFD
            ReturnValueSchema var_out_0x7FFFFFFD = null;

            // Arrange variable: var_0x7FFFFFFE
            int var_0x7FFFFFFE = -1;

            // Arrange variable: expectedElement_0
            ReturnValueSchema expectedElement_0 = new ReturnValueSchema();
            expectedElement_0.SetPrivate(""ValueField"", var_0x7FFFFFFE);
            expectedElement_0.SetPrivate(""RefField"", var_out_0x7FFFFFFD);

            // act
            ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            array[0].Should().NotBeNull();
            array[0].Should().BeEquivalentTo(expectedElement_0);
        }
    }
}",
@"using FluentAssertions;
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
            ReturnValueSchema[] array = new ReturnValueSchema[2] { null, null };

            // act
            Action act = () => ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            act.Should().NotThrow();
        }
        
        [Fact]
        public void Test_2_ReturnValue()
        {
            // arrange
            // Arrange variable: array
            ReturnValueSchema[] array = new ReturnValueSchema[2] { null, null };

            // Arrange variable: expectedResult
            ReturnValueSchema[] expectedResult = null;

            // act
            var result = ReturnValueSchema.ReferenceTypeArrayValue(array);
            // assert
            result.Should().BeNull();
        }
    }
}"
}, false)]
        public void ReferenceTypeArrayValuesReturn(string[] expected, bool preferLiteralsOverVariables)
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("test")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ReturnValueSchema.ReferenceTypeArrayValue");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(4);

            paths[0].Exception.Should().BeNull();
            paths[1].Exception.Should().BeNull();
            paths[2].Exception.Should().BeNull();
            paths[3].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("test.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            ITestGeneratorConfiguration configuration = GetConfiguration();
            configuration.PreferLiteralsOverVariables = preferLiteralsOverVariables;

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(configuration, exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx =>
                {
                    return testClassGenerator.GenerateTestClassFileContent(ctx).Trim();
                })
                .ToArray();

            testClasses.Should().BeEquivalentTo(expected);
        }
    }
}
