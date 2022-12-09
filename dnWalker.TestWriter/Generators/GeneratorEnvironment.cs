using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class GeneratorEnvironment
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

        private static string GetNamespace(TypeDef td)
        {
            while (td.DeclaringType != null)
            {
                td = td.DeclaringType;
            }
            return td.Namespace;
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
                List<string> ns = new List<string>() { GetNamespace(genInstSig.ToTypeDefOrRef().ResolveTypeDefThrow()) };

                foreach(TypeSig genParam in genInstSig.GetGenericParameters())
                {
                    ns.AddRange(GetNamespaces(genParam));
                }

                return ns;
            }

            // type is just "normal type"
            return new[] { GetNamespace(ts.ToTypeDefOrRef().ResolveTypeDefThrow()) };
        }

        private static string GetName(MethodDef method)
        {
            // Nest1_Nest2_..._NestN_Name
            Stack<TypeDef> declChain = new Stack<TypeDef>();
            
            TypeDef td = method.DeclaringType;

            while (td != null)
            {
                declChain.Push(td);
                td = td.DeclaringType;
            }

            StringBuilder sb = new StringBuilder();
            while (declChain.TryPop(out td!))
            {
                sb.Append(td.Name);
                sb.Append('_');
            }
            sb.Append(method.Name);
            return sb.ToString();
        }


        public TestClass GenerateTestClass(ITestFramework framework, TestProject testProject, ConcolicExploration concolicExploration)
        {
            // setup test group
            MethodDef method = concolicExploration.MethodUnderTest.ResolveMethodDefThrow();

            string methodNamespace = GetNamespace(method.DeclaringType);

            string testGroupName = methodNamespace.Replace('.', '/');
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
            foreach (string ns in TestTemplate.GahterNamspaces())
            {
                testClass.Usings.Add(ns);
            }
            foreach (string ns in GatherNamespaces(concolicExploration))
            {
                testClass.Usings.Add(ns);
            }
            testClass.Name = GetName(method) + "Tests";
            testClass.Namespace = methodNamespace + ".Tests";


            int methodIndex = 1;
            foreach (ITestSchema testSchema in _testSchemaProvider.GetSchemas(concolicExploration))
            {
                TestMethod testMethod = GenerateTestMethod(testSchema, framework, testClass);
                testMethod.Name = $"{testMethod.Name}_{methodIndex++}";

            }

            return testClass;
        }

        private static IEnumerable<string> GatherNamespaces(ConcolicExploration concolicExploration)
        {
            return concolicExploration.Iterations.SelectMany(it => GatherNamespaces(it));
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

        private TestMethod GenerateTestMethod(ITestSchema testSchema, ITestFramework framework, TestClass testClass)
        {
            TestMethod testMethod = framework.CreateTestMethod(testClass, testSchema);
            testMethod.Body = GenerateMethodBody(testSchema);
            testMethod.Name ??= testSchema.GetTestMethodName();

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
