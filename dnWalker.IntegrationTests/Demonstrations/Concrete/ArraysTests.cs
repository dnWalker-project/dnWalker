using dnWalker.Concolic;
using dnWalker.Tests.Examples;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.IntegrationTests.Demonstrations.Concrete
{
    public class ArraysTests : IntegrationTestBase
    {
        public ArraysTests(ITestOutputHelper output) : base(output)
        {
        }

        [IntegrationTest]
        public void NoPreconditions_IndexOf(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Arrays.IndexOf");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
