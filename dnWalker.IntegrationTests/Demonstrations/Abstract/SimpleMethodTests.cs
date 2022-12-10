using dnWalker.Concolic.Traversal;
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
    public class SimpleMethodTests : IntegrationTestBase
    {
        public SimpleMethodTests(ITestOutputHelper output) : base(output)
        {
        }

        [IntegrationTest]
        public void FooNoPrecondition(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore<AllPathsCoverage>("Examples.Demonstrations.Abstract.SimpleMethod.Foo");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
