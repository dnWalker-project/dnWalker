using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    internal class GeneratorEnvironment
    {
        private readonly Writer _writer = new Writer();

        private readonly ITestTemplate _testTemplate;
        private readonly ITestSchemaProvider _testSchemaProvider;

        public GeneratorEnvironment(ITestTemplate testTemplate, ITestSchemaProvider testSchemaProvider)
        {
            _testTemplate = testTemplate;
            _testSchemaProvider = testSchemaProvider;
        }

        public ITestTemplate TestTemplate
        {
            get
            {
                return _testTemplate;
            }
        }

        public ITestSchemaProvider TestSchemaProvider
        {
            get
            {
                return _testSchemaProvider;
            }
        }

        public TestClass GenerateTestClass(ITestFramework framework, TestProject testProject, ConcolicExploration concolicExploration)
        {
            // setup test group
            MethodDef method = concolicExploration.MethodUnderTest.ResolveMethodDefThrow();

            string moduleName = method.Module.Name;
            string methodNamespace = method.DeclaringType.Namespace;

            string testGroupName = methodNamespace.Substring(moduleName.Length).Replace('.', '/');
            if (string.IsNullOrWhiteSpace(testGroupName))
            {
                testGroupName = ".";
            }

            if (!testProject.TestGroups.TryGetValue(testGroupName, out TestGroup? testGroup))
            {
                testGroup = new TestGroup() { Name = testGroupName };
                testProject.TestGroups.Add(testGroupName, testGroup);
            }

            TestClass testClass = framework.CreateTestClass(testProject, testGroup);

            foreach (ITestSchema testSchema in _testSchemaProvider.GetSchemas(concolicExploration))
            {
                GenerateTestMethod(testSchema, framework, testClass);
            }

            return testClass;
        }




        private TestMethod GenerateTestMethod(ITestSchema testSchema, ITestFramework framework, TestClass testClass)
        {
            TestMethod testMethod = framework.CreateTestMethod(testClass, testSchema);
            testMethod.Body = GenerateMethodBody(testSchema);

            return testMethod;
        }

        private string GenerateMethodBody(ITestSchema testSchema)
        {
            try
            {
                testSchema.Write(_testTemplate, _writer);
                return _writer.ToString();
            }
            finally
            {
                _writer.Clear();
            }
        }
    }
}
