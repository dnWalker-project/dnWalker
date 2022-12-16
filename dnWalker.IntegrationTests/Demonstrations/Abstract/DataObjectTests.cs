using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Input;
using dnWalker.Input.Xml;
using dnWalker.Tests.Examples;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            ExplorationResult exploration = Explore("Examples.Demonstrations.AbstractData.DataObject.ReadData");

            TestProject testProject = GenerateTests(exploration);
            IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        }

        //[IntegrationTest]
        //public void ReadDataSomePreviousState(BuildInfo buildInfo)
        //{
        //    Initialize(buildInfo);

        //    const string ModelXml =
        //    """
        //    <UserModels>
        //        <UserModel EntryPoint="Examples.Demonstrations.Abstract.DataObject.ReadData">
        //            <m-this>
        //                <Object>
        //                    <Id>55</Id>
        //                    <Created>113</Created>
        //                    <LastAccess>225</LastAccess>
        //                    <Author>John Doe</Author>
        //                </Object>
        //            </m-this>
        //        </UserModel>
        //    </UserModels>
        //    """;

        //    IEnumerable<UserModel> userModels = new XmlUserModelParser(DefinitionProvider).ParseModelCollection(XElement.Parse(ModelXml));

        //    ExplorationResult exploration = Explore<AllEdgesCoverage>("Examples.Demonstrations.Abstract.DataObject.ReadData", userModels);
        //    TestProject testProject = GenerateTests(exploration);
        //    IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        //}

        //[IntegrationTest]
        //public void ReadDataEnsureMaxRecordsLength(BuildInfo buildInfo)
        //{
        //    Initialize(buildInfo);

        //    const string ModelXml =
        //    """
        //    <UserModels>
        //        <UserModel EntryPoint="Examples.Demonstrations.Abstract.DataObject.ReadData">
        //            <m-this><Object/></m-this>
        //            <database>
        //                <Object>
        //                    <GetRecords>
        //                        <Array Length="2"/>
        //                    </GetRecords>
        //                </Object>
        //            </database>
        //        </UserModel>
        //    </UserModels>
        //    """;

        //    IEnumerable<UserModel> userModels = new XmlUserModelParser(DefinitionProvider).ParseModelCollection(XElement.Parse(ModelXml));

        //    ExplorationResult exploration = Explore<AllPathsCoverage>("Examples.Demonstrations.Abstract.DataObject.ReadData", userModels);
        //    TestProject testProject = GenerateTests(exploration);
        //    IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        //}

        //[IntegrationTest]
        //public void ReadDataEnsureMaxRecordsLengthAndSomeState(BuildInfo buildInfo)
        //{
        //    Initialize(buildInfo);

        //    const string ModelXml =
        //    """
        //    <UserModels>
        //        <UserModel EntryPoint="Examples.Demonstrations.Abstract.DataObject.ReadData">
        //            <m-this>
        //                <Object>
        //                    <Id>55</Id>
        //                    <Created>113</Created>
        //                    <LastAccess>225</LastAccess>
        //                    <Author>John Doe</Author>
        //                </Object>
        //            </m-this>
        //            <database>
        //                <Object>
        //                    <GetRecords>
        //                        <Array Length="1"/>
        //                    </GetRecords>
        //                </Object>
        //            </database>
        //        </UserModel>
        //    </UserModels>
        //    """;

        //    IEnumerable<UserModel> userModels = new XmlUserModelParser(DefinitionProvider).ParseModelCollection(XElement.Parse(ModelXml));

        //    ExplorationResult exploration = Explore<AllPathsCoverage>("Examples.Demonstrations.Abstract.DataObject.ReadData", userModels);
        //    TestProject testProject = GenerateTests(exploration);
        //    IReadOnlyDictionary<string, string> files = WriteTests(testProject);
        //}
    }
}
