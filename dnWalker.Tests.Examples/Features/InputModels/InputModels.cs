using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Configuration;
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
    public class InputModelTests : ExamplesTestBase, IDisposable
    {
        private string _inputModelsFile;

        public InputModelTests(ITestOutputHelper output) : base(output)
        {
            _inputModelsFile = Random.Shared.Next().ToString() + ".xml";

        }

        protected override void SetupConfiguration(IConfiguration configuration)
        {
            base.SetupConfiguration(configuration);

            configuration.SetInputModelsFile(_inputModelsFile);
        }

        public void Dispose()
        {
            System.IO.File.Delete(_inputModelsFile);
        }

        private void WriteInputModels(MethodDef method, params IReadOnlyModel[] models)
        {
            TypeParser tp = new TypeParser(DefinitionProvider);
            MethodParser mp = new MethodParser(DefinitionProvider, tp);
            XmlModelSerializer xmlModelSerializer = new XmlModelSerializer(tp, mp);

            XElement xml = new XElement("InputModels", models.Select(m =>
            {
                XElement modelXml = xmlModelSerializer.ToXml(m, "InputModel");
                modelXml.SetAttributeValue("Method", method.FullName);
                return modelXml;
            }));

            xml.Save(_inputModelsFile);
        }

        private void WriteInputModels(params string[] models)
        {
            using (StreamWriter writer = new StreamWriter(_inputModelsFile))
            {
                writer.WriteLine("<InputModels>");
                foreach (string model in models)
                {
                    writer.WriteLine(model);
                }
                writer.WriteLine("</InputModels>");
            }
        }


        [ExamplesTest]
        public void BranchIfNullForceNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNullModel =
@"<InputModel Method=""Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull"">
    <Variables>
        <MethodArgument Name=""instance"" Value=""null"" />
    </Variables>
    <Heap />
</InputModel>";
            WriteInputModels(InstanceIsNullModel);

            ExplorationResult result = CreateExplorer(buildInfo).Run(MethodName);

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance is null");
        }

        [ExamplesTest]
        public void BranchIfNullForceNotNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNotNullModel =
@"<InputModel Method=""Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull"">
    <Variables>
        <MethodArgument Name=""instance"" Value=""@0000001"" />
    </Variables>
    <Heap>
        <ObjectNode IsDirty=""False"" Location=""@0000001"" Type=""Examples.Concolic.Features.Objects.TestClass""/>
    </Heap>
</InputModel>";
            WriteInputModels(InstanceIsNotNullModel);

            ExplorationResult result = CreateExplorer(buildInfo).Run(MethodName);

            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance is not null");
        }

        [ExamplesTest]
        public void BranchIfNullForceNotNullThenNull(BuildInfo buildInfo)
        {
            const string MethodName = "Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull";
            const string InstanceIsNullModel =
@"<InputModel Method=""Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull"">
    <Variables>
        <MethodArgument Name=""instance"" Value=""null"" />
    </Variables>
    <Heap />
</InputModel>";
            const string InstanceIsNotNullModel =
@"<InputModel Method=""Examples.Concolic.Features.Objects.MethodsWithObjectParameter.BranchIfNull"">
    <Variables>
        <MethodArgument Name=""instance"" Value=""@0000001"" />
    </Variables>
    <Heap>
        <ObjectNode IsDirty=""False"" Location=""@0000001"" Type=""Examples.Concolic.Features.Objects.TestClass""/>
    </Heap>
</InputModel>";

            // the AllPaths strategy stores the constraints on the stack, so the order of the models in the configuration is inverse to the order of the iterations
            WriteInputModels(InstanceIsNullModel, InstanceIsNotNullModel);

            ExplorationResult result = CreateExplorer(buildInfo).Run(MethodName);

            result.Iterations.Should().HaveCount(2);
            result.Iterations[0].Output.Trim().Should().Be("instance is not null");
            result.Iterations[1].Output.Trim().Should().Be("instance is null");
        }
    }
}
