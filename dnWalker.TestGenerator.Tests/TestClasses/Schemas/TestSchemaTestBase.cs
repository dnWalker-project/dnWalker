using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestClasses.Schemas;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.TestClasses.Schemas
{
    public abstract class TestSchemaTestBase
    {
        protected readonly IMethod StaticMethod;
        protected readonly IMethod InstanceMethod;
        protected readonly TypeSig TestClassSig;
        protected readonly TypeDef TestClassTD;

        protected readonly TypeSig ObjectSig;
        protected readonly TypeSig DataClassSig;
        protected readonly TypeDef DataClassTD;

        protected readonly IDefinitionProvider DefinitionProvider;

        protected TestSchemaTestBase()
        {
            DefinitionProvider = TestUtils.DefinitionProvider;

            TestClassTD = DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.TestClasses.Schemas.ClassUnderTest");
            TestClassSig = TestClassTD.ToTypeSig();

            StaticMethod = TestClassTD.FindMethods("StaticMethod").First();
            InstanceMethod = TestClassTD.FindMethods("InstanceMethod").First();

            ObjectSig = DefinitionProvider.BaseTypes.Object;
            DataClassTD = DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.TestClasses.Schemas.DataClass");
            DataClassSig = DataClassTD.ToTypeSig();
        }

        protected string GenerateTestClass(TestSchema schema)
        {
            TestWriter output = new TestWriter();

            schema.WriteTestMethodBody(output, TestUtils.Templates);

            return output.ToString().Trim();
        }

        protected IMethod GetMethod(string methodName)
        {
            return TestClassTD.FindMethods(methodName).First();
        }

        protected ITestClassContext CreateContext(IReadOnlyModel inputModel, IMethod testedMethod)
        {
            return CreateContext(inputModel, inputModel.Clone(), testedMethod);
        }

        protected ITestClassContext CreateContext(IReadOnlyModel inputModel, IReadOnlyModel outputModel, IMethod testedMethod)
        {
            return new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = testedMethod
            }.Build();
        }
    }
}
