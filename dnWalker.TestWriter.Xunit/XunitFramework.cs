using dnWalker.Explorations;
using dnWalker.TestWriter;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.Utils;

namespace dnWalker.TestWriter.Xunit
{
    public class XunitFramework : TestFrameworkBase
    {
        protected override void InitializeTestProject(TestProject testProject, IEnumerable<ConcolicExploration> explorations)
        {
            // TODO: add nuget packages
            //testProject.Packages.Add(new PackageReference { Name = "xunit", Version = "2.4.2" });
            //testProject.Packages.Add(new PackageReference { Name = "xunit.runner.visualstudio", Version = "2.4.5" });
        }

        protected override void InitializeTestClass(TestClass testClass, TestGroup testGroup, ConcolicExploration exploration)
        {
            string classUnderTest = exploration.MethodUnderTest.DeclaringType.GetName();
            string methodUnderTest = exploration.MethodUnderTest.Name;

            testClass.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "\"dnWalkerGenerated\"", $"\"{classUnderTest}::{methodUnderTest}\"" }
            });
            testClass.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "\"ExplorationStrategy\"", $"\"{exploration.Strategy}\"" }
            });

            testClass.Usings.Add("Xunit");
        }

        protected override void InitializeTestMethod(TestMethod testMethod, TestClass testClass, ConcolicExplorationIteration explorationIteration, ITestSchema testSchema)
        {
            testMethod.Attributes.Add(new AttributeInfo
            {
                TypeName = "Fact"
            });
            testMethod.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "\"TestSchema\"", $"\"{testSchema.GetType().Name}\"" }
            });
            testMethod.Attributes.Add(new AttributeInfo
            {
                TypeName = "Trait",
                PositionalArguments = { "\"Iteration\"", $"\"{explorationIteration.IterationNumber}\"" }
            });
            //testMethod.Attributes.Add(new AttributeInfo
            //{
            //    TypeName = "Trait",
            //    PositionalArguments = { "\"Precondition\"", $"\"{explorationIteration.PathConstraint}\"" }
            //});
        }
    }
}