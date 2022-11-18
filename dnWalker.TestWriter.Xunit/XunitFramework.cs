using dnWalker.TestWriter;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.Utils;

namespace dnWalker.TestWriter.Xunit
{
    public class XunitFramework : TestFrameworkBase
    {
        protected override void InitializeTestProject(TestProject testProject)
        {
            // TODO: add nuget packages
            //testProject.Packages.Add(new PackageReference { Name = "xunit", Version = "2.4.2" });
            //testProject.Packages.Add(new PackageReference { Name = "xunit.runner.visualstudio", Version = "2.4.5" });
        }

        protected override void InitializeTestClass(TestClass testClass, TestGroup testGroup)
        {
            testClass.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "dnWalkerGenerated", "SUT name" }
            });
            testClass.Usings.Add("Xunit");
        }

        protected override void InitializeTestMethod(TestMethod testMethod, TestClass testClass, ITestSchema testSchema)
        {
            testMethod.Attributes.Add(new AttributeInfo
            {
                TypeName = "Fact"
            });
            testClass.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "TestSchema", testSchema.GetType().Name }
            });
        }
    }
}