using dnWalker.Concolic;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Parameters;
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

        [Fact]
        public void PrimitiveValueReturn()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("exception-method")
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

            string xmlData = System.IO.File.ReadAllText("exception-method.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx))
                .ToArray();
        }

        [Fact]
        public void ArrayOfPrimitiveValuesReturn()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("exception-method")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ReturnValueSchema.ArrayOfPrimitives");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(3);

            paths[0].Exception.Should().BeNull();
            paths[1].Exception.Should().BeNull();
            paths[2].Exception.Should().BeNull();

            string xmlData = System.IO.File.ReadAllText("exception-method.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx =>
                {
                    return testClassGenerator.GenerateTestClassFileContent(ctx);
                })
                .ToArray();
        }
    }
}
