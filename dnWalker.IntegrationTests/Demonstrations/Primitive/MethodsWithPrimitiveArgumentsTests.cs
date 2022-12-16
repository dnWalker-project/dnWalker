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

namespace dnWalker.IntegrationTests.Demonstrations.Primitive
{
    public class MethodsWithPrimitiveArgumentsTests : IntegrationTestBase
    {
        public MethodsWithPrimitiveArgumentsTests(ITestOutputHelper output) : base(output)
        {
        }

        [IntegrationTest]
        public void NoPreconditions_Pow(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.Primitive.PrimitiveArguments.Pow");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Foo(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.Primitive.PrimitiveArguments.Foo");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
