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

namespace dnWalker.IntegrationTests.Demonstrations.Concrete
{
    public class SegmentTests : IntegrationTestBase
    {
        public SegmentTests(ITestOutputHelper output) : base(output)
        {
        }

        [IntegrationTest]
        public void NoPreconditions_Foo(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore<AllPathsCoverage>("Examples.Demonstrations.Concrete.Segment.Foo");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Append(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore<AllEdgesCoverage>("Examples.Demonstrations.Concrete.Segment.Append");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }


        [IntegrationTest]
        public void NoPreconditions_Insert(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore<AllEdgesCoverage>("Examples.Demonstrations.Concrete.Segment.Insert");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Count(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore<AllEdgesCoverage>("Examples.Demonstrations.Concrete.Segment.Count");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
