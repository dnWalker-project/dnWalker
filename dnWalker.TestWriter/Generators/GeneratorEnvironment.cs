using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class GeneratorEnvironment
    {
        private readonly Writer _writer = new Writer();

        private readonly ITestTemplate _testTemplate;
        private readonly ITestSchemaProvider _testSchemaProvider;

        // TODO inject these objects
        private readonly ITestMethodNamingStrategy _testMethodNamingStrategy = new BasicTestMethodNamingStrategy();
        private readonly ITestClassNamingStrategy _testClassNamingStrategy = new BasicTestClassNamingStrategy();
        private readonly ITestProjectNamingStrategy _testProjectNamingStrategy = new BasicTestProjectNamingStrategy();

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

        private static IEnumerable<string> GetNamespaces(TypeSig ts)
        {
            // type may be an array => get namespaces of the "next"
            if (ts.IsArray || ts.IsSZArray)
            {
                return GetNamespaces(ts.Next);
            }

            // type may be a generic instance => get namespacec of all of the arguments
            if (ts.IsGenericInstanceType)
            {
                GenericInstSig genInstSig = ts.ToGenericInstSig();

                // create list of the namespaces & add the generic type namespace
                List<string> ns = new List<string>() { genInstSig.ToTypeDefOrRef().ResolveTypeDefThrow().GetNamespace() };

                foreach(TypeSig genParam in genInstSig.GetGenericParameters())
                {
                    ns.AddRange(GetNamespaces(genParam));
                }

                return ns;
            }

            // type is just "normal type"
            return new[] { ts.ToTypeDefOrRef().ResolveTypeDefThrow().GetNamespace() };
        }


        public TestProject GenerateTestProject(ITestFramework framework, ConcolicExploration concolicExploration)
        {
            string testProjectName = _testProjectNamingStrategy.GetProjectName(concolicExploration.MethodUnderTest.Module);

            TestProject testProject = framework.CreateTestProject(testProjectName, new[] { concolicExploration });

            // add packages from the test templates
            foreach (PackageReference pr in TestTemplate.GetPackages())
            {
                testProject.Packages.Add(pr);
            }

            // add packages from the test framework
            foreach (PackageReference pr in framework.GetPackages())
            {
                testProject.Packages.Add(pr);
            }

            return testProject;

        }

        public TestClass GenerateTestClass(ITestFramework framework, TestProject testProject, ConcolicExploration exploration)
        {
            // setup test group
            MethodDef method = exploration.MethodUnderTest.ResolveMethodDefThrow();

            // test class namespace and name
            string testClassNamespace = _testClassNamingStrategy.GetNamespaceName(method);
            string testClassName = _testClassNamingStrategy.GetClassName(method);

            // test group name
            string testGroupName = testClassNamespace.Substring(testProject.Name!.Length + 1).Replace('.','\\');
            if (string.IsNullOrWhiteSpace(testGroupName))
            {
                testGroupName = ".";
            }

            if (!testProject.TestGroups.TryGetValue(testGroupName, out TestGroup? testGroup))
            {
                testGroup = new TestGroup() { Name = testGroupName };
                testProject.TestGroups.Add(testGroupName, testGroup);
            }

            TestClass testClass = framework.CreateTestClass(testProject, testGroup, exploration);
            testClass.Name = testClassName;
            testClass.Namespace = testClassNamespace;
            foreach (string ns in TestTemplate.GahterNamspaces())
            {
                testClass.Usings.Add(ns);
            }
            foreach (string ns in GatherNamespaces(exploration))
            {
                testClass.Usings.Add(ns);
            }


            int methodIndex = 1;
            foreach (ConcolicExplorationIteration explorationIteration in exploration.Iterations)
            {
                foreach (ITestSchema testSchema in _testSchemaProvider.GetSchemas(explorationIteration))
                {
                    string testMethodName = _testMethodNamingStrategy.GetMethodName(method, testSchema);

                    TestMethod testMethod = framework.CreateTestMethod(testClass, explorationIteration, testSchema);
                    testMethod.Body = GenerateMethodBody(testSchema);

                    testMethod.Name ??= $"{testMethodName}_{methodIndex++}";
                }
            }

            return testClass;
        }

        private static IEnumerable<string> GatherNamespaces(ConcolicExploration concolicExploration)
        {
            return concolicExploration.Iterations.SelectMany(it => GatherNamespaces(it)).Concat(GetNamespaces(concolicExploration.MethodUnderTest.DeclaringType.ToTypeSig()));
        }

        private static IEnumerable<string> GatherNamespaces(ConcolicExplorationIteration it)
        {
            List<string> ns = new List<string>();

            foreach (IReadOnlyHeapNode n in it.InputModel.HeapInfo.Nodes)
            {
                TypeSig t = n is IReadOnlyArrayHeapNode ? n.Type.Next : n.Type;
                ns.AddRange(GetNamespaces(t));
            }

            foreach (IReadOnlyHeapNode n in it.OutputModel.HeapInfo.Nodes)
            {
                TypeSig t = n is IReadOnlyArrayHeapNode ? n.Type.Next : n.Type;
                ns.AddRange(GetNamespaces(t));
            }
            return ns;
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
