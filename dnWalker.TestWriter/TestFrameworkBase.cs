using dnWalker.Explorations;
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
        protected abstract void InitializeTestProject(TestProject testProject, IEnumerable<ConcolicExploration> explorations);
        protected abstract void InitializeTestClass(TestClass testClass, TestGroup testGroup, ConcolicExploration exploration);
        protected abstract void InitializeTestMethod(TestMethod testMethod, TestClass testClass, ConcolicExplorationIteration explorationIteration, ITestSchema testSchema);

        public TestProject CreateTestProject(string name, IEnumerable<ConcolicExploration> explorations)
        {
            TestProject newProject = new TestProject() { Name = name };
            InitializeTestProject(newProject, explorations);
            return newProject;
        }

        public TestClass CreateTestClass(TestProject testProject, TestGroup testGroup, ConcolicExploration exploration)
        {
            TestClass newClass = new TestClass();
            testGroup.TestClasses.Add(newClass);
            InitializeTestClass(newClass, testGroup, exploration);
            return newClass;
        }

        public TestMethod CreateTestMethod(TestClass testClass, ConcolicExplorationIteration explorationIteration, ITestSchema testSchema)
        {
            TestMethod newMethod = new TestMethod();
            testClass.Methods.Add(newMethod);
            InitializeTestMethod(newMethod, testClass, explorationIteration, testSchema);
            return newMethod;
        }
    }
}
