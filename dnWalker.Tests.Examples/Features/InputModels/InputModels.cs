using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Configuration;
using dnWalker.Input;
using dnWalker.Input.Xml;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Tests.Examples.Features.InputModels
{
    public class InputModelTests : ExamplesTestBase
    {
        private XmlUserModelParser? _userModelParser;

        public InputModelTests(ITestOutputHelper output) : base(output)
        {
        }

        public XmlUserModelParser UserModelParser
        {
            get
            {
                _userModelParser ??= new XmlUserModelParser(DefinitionProvider);
                return _userModelParser;
            }
        }

        [ExamplesTest]
        public void BranchIfNullForceNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNullModel =
            """
            <UserModels>
                <UserModel EntryPoint="Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull">
                    <instance>null</instance>
                </UserModel>
            </UserModels>
            """;

            ExplorationResult result = CreateExplorer(buildInfo).Run(MethodName, UserModelParser.ParseModelCollection(XElement.Parse(InstanceIsNullModel)));

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
        }

        [ExamplesTest]
        public void BranchIfNullForceNotNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNotNullModel =
            """
            <UserModels>
                <UserModel EntryPoint="Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull">
                    <instance>
                        <Object/>
                    </instance>
                </UserModel>
            </UserModels>
            """;

            IExplorer explorer = CreateExplorer(buildInfo);

            IEnumerable<UserModel> models = UserModelParser.ParseModelCollection(XElement.Parse(InstanceIsNotNullModel));

            ExplorationResult result = explorer.Run(MethodName, models);

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance is not null");
        }

        [ExamplesTest]
        public void BranchIfNullForceNotNullThenNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNullAndIsNotNullModels =
            """
            <UserModels>
                <UserModel EntryPoint="Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull">
                    <instance>null</instance>
                </UserModel>
                <UserModel EntryPoint="Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull">
                    <instance>
                        <Object/>
                    </instance>
                </UserModel>
            </UserModels>
            """;

            ExplorationResult result = CreateExplorer(buildInfo).Run(MethodName, UserModelParser.ParseModelCollection(XElement.Parse(InstanceIsNullAndIsNotNullModels)));

            result.Iterations.Should().HaveCount(2);
            result.Iterations.Select(it => it.Output.Trim())
                .Should().Contain(new[]
                {
                    "instance is not null",
                    "instance is null"
                });
        }
    }
}
