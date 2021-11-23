using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.Concolic.DataExport
{
    public class XmlExplorationExporterTest : SymbolicExamplesTestBase2
    {
        public XmlExplorationExporterTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void NoBranching_ShouldEnd_Within_1_Iteration()
        {
            const string outputFile = "test.xml";

            Explore("Examples.Concolic.Simple.Branches.NoBranching",
                (cfg) =>
                {
                    cfg.MaxIterations = 1;
                    cfg.AssemblyToCheckFileName = AssemblyFile;
                    cfg.ExplorationInfoOutputFile = outputFile;
                    cfg.ExportIterationInfo = true;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(1);

                    XElement exportXml = XElement.Load(outputFile);
                });
        }
    }
}
