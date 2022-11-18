using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Xunit.Tests
{
    file class DummyTestSchema : ITestSchema
    {
        public ITestContext TestContext
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Write(ITestTemplate testTemplate, IWriter output)
        {
            throw new NotImplementedException();
        }
    }

    public class XunitFrameworkTests
    {
        [Fact]
        public void TestClassHasUsings()
        {
            XunitFramework framework= new XunitFramework();
            TestClass testClass = framework.CreateTestClass(framework.CreateTestProject(string.Empty), new TestGroup());

            testClass.Usings.Should().Contain("Xunit");
        }

        [Fact]
        public void TestMethodIsFact()
        {
            XunitFramework framework = new XunitFramework();
            TestClass testClass = framework.CreateTestClass(framework.CreateTestProject(string.Empty), new TestGroup());
            TestMethod testMethod = framework.CreateTestMethod(testClass, new DummyTestSchema());

            testMethod.Attributes.Any(ai => ai.TypeName == "Fact").Should().BeTrue();
            testMethod.Attributes.Any(ai => ai.TypeName == "Trait" && ai.PositionalArguments[0] == "TestSchema").Should().BeTrue();
        }
    }
}
