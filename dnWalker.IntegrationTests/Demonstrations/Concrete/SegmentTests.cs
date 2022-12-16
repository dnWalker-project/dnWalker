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
using dnWalker.Input.Xml;
using dnWalker.Input;
using System.Xml.Linq;

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

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Segment.Foo");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Append(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Segment.Append");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }


        [IntegrationTest]
        public void NoPreconditions_Insert(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Segment.Insert");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Count(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Segment.Count");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        [IntegrationTest]
        public void NoPreconditions_Delete(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            ExplorationResult exploration = Explore("Examples.Demonstrations.ConcreteData.Segment.Delete");
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        //[IntegrationTest]
        //public void ArrayLengthBound_Delete(BuildInfo buildInfo)
        //{
        //    Initialize(buildInfo);

        //    const string ModelXml =
        //    """
        //    <UserModels>
        //        <SharedData>
        //            <Object Type="Examples.Demonstrations.Concrete.Segment" Id="length2">
        //                <Next>
        //                    <Object>
        //                        <Next>null</Next>
        //                    </Object>
        //                </Next>
        //            </Object>
        //        </SharedData>
        //        <UserModel EntryPoint="Examples.Demonstrations.Concrete.Segment.Delete">
        //            <m-this>
        //                <Reference>length2</Reference>
        //            </m-this>
        //            <data>
        //                <Array  Length="2"/>
        //            </data>
        //        </UserModel>
        //        <UserModel EntryPoint="Examples.Demonstrations.Concrete.Segment.Delete">
        //            <m-this>
        //                <Reference>length2</Reference>
        //            </m-this>
        //            <data>null</data>
        //        </UserModel>
        //    </UserModels>
        //    """;

        //    IEnumerable<UserModel> userModels = new XmlUserModelParser(DefinitionProvider).ParseModelCollection(XElement.Parse(ModelXml));

        //    ExplorationResult exploration = Explore<AllEdgesCoverage>("Examples.Demonstrations.Concrete.Segment.Delete", userModels);
        //    TestProject testProject = GenerateTests(exploration);
        //    IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        //}
    }
}
