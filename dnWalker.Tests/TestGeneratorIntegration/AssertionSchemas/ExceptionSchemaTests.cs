using dnWalker.Concolic;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
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
    public class ExceptionSchemaTests : IntegrationTestBase
    {
        public ExceptionSchemaTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void Test_ExceptionAssertSchema()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .ExportXmlData("exception-method")
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.TestGeneration.ExceptionSchema.ThrowIfNull");

            var paths = explorer.PathStore.Paths;

            paths.Should().HaveCount(2);

            paths[0].Exception.Type.Should().Be(DefinitionProvider.GetTypeDefinition("System.NullReferenceException"));

            string xmlData = System.IO.File.ReadAllText("exception-method.xml");

            ConcolicExploration exploration = new XmlExplorationDeserializer().GetExploration(XElement.Parse(xmlData));

            IReadOnlyList<ITestClassContext> testContexts = TestClassContext.FromExplorationData(exploration);

            XUnitTestClassGenerator testClassGenerator = new XUnitTestClassGenerator();

            string[] testClasses = testContexts
                .Select(ctx => testClassGenerator.GenerateTestClassFileContent(ctx))
                .ToArray();

        }
    }
}
