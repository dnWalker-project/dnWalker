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

        [IntegrationTest]
        public void ProveConstrainedBehaviorNoPrecondition(BuildInfo buildInfo)
        {
            Initialize(buildInfo);

            const string ModelXml =
            """
            <UserModels>
                <UserModel EntryPoint="Examples.Demonstrations.Abstract.SimpleMethod.ProveConstrainedBehavior">
                    <m-this>
                        <Object />
                    </m-this>
                    <bar>
                        <Object>
                            <GetValueWithArgs Condition="x &#60; y">5</GetValueWithArgs>
                            <GetValueWithArgs Condition="x &#62;&#61; y">10</GetValueWithArgs>
                        </Object>
                    </bar>
                </UserModel>
            </UserModels>
            """;

            IEnumerable<UserModel> userModels = new XmlUserModelParser(DefinitionProvider).ParseModelCollection(XElement.Parse(ModelXml));

            ExplorationResult exploration = Explore<AllPathsCoverage>("Examples.Demonstrations.Abstract.SimpleMethod.ProveConstrainedBehavior", userModels);
            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }
    }
}
