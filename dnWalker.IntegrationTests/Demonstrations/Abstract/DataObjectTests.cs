using dnWalker.Concolic;
using dnWalker.Tests.Examples;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.IntegrationTests.Demonstrations.Abstract
{
    public class DataObjectTests : IntegrationTestBase
    {
        public DataObjectTests(ITestOutputHelper output) : base(output)
        {
        }

        [IntegrationTest]
        public void ReadDataNoPrecondition(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.Abstract.DataObject.ReadData");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
