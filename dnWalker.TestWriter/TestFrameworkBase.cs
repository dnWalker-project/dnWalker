using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Utils
{
    public abstract class TestFrameworkBase : ITestFramework
    {
        protected abstract void InitializeTestProject(TestProject testProject);
        protected abstract void InitializeTestClass(TestClass testClass, TestGroup testGroup);
        protected abstract void InitializeTestMethod(TestMethod testMethod, TestClass testClass, ITestSchema testSchema);

        public TestProject CreateTestProject(string name)
        {
            TestProject newProject = new TestProject() { Name = name };
            InitializeTestProject(newProject);
            return newProject;
        }

        public TestClass CreateTestClass(TestProject testProject, TestGroup testGroup)
        {
            TestClass newClass = new TestClass();
            testGroup.TestClasses.Add(newClass);
            InitializeTestClass(newClass, testGroup);
            return newClass;
        }

        public TestMethod CreateTestMethod(TestClass testClass, ITestSchema testSchema)
        {
            TestMethod newMethod = new TestMethod();
            testClass.Methods.Add(newMethod);
            InitializeTestMethod(newMethod, testClass, testSchema);
            return newMethod;
        }
    }
}
