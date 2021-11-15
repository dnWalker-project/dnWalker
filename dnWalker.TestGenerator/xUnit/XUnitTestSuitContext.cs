using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.CodeDom;

namespace dnWalker.TestGenerator.xUnit
{
    public class XUnitTestSuitContext : ITestSuitContext
    {
        private static readonly string[] XUnitUsings = { "Xunit" };
        //private static readonly string[] BasicUsings = { "System" };

        public void WriteTest(CodeWriter codeWriter, TestData testData)
        {
            IEnumerable<string> ns = testData.GetNamespaces();

            foreach (string namespaceName in ns.Concat(XUnitUsings))
            {
                codeWriter.WriteUsing(namespaceName);
            }

            codeWriter.WriteEmptyLine();

            string testClassName = testData.MethodName!.Replace('.', '_');

            using (IDisposable testNamespace = codeWriter.BeginNamespace("TestNamespace"))
            {
                codeWriter.WriteEmptyLine();
                using (IDisposable testClass = codeWriter.BeginClass(MemberModifiers.Public, testClassName))
                {
                    codeWriter.WriteEmptyLine();
                    codeWriter.WriteAttribute("Fact");
                    using (IDisposable testMethod = codeWriter.BeginMethod(MemberModifiers.Public, "void", "TestCase_1"))
                    {
                        // initialize input parameters
                        codeWriter.WriteComment("Initialize method arguments");
                        foreach (Parameter p in testData.MethodArguments)
                        {
                            p.WriteInitialization(codeWriter);
                        }

                        codeWriter.WriteEmptyLine();
                        // initialize expected result parameter
                        if (testData.Result != null)
                        {
                            codeWriter.WriteComment("Initialize expected method result");
                            testData.Result.WriteInitialization(codeWriter);
                        }

                        codeWriter.WriteEmptyLine();
                        // invoke the method
                        if (testData.Result != null)
                        {
                            // store the result to a new variable
                            codeWriter.WriteComment("Run tested method");
                            codeWriter.WriteMethodInvocation("var", "result", testData.MethodName, testData.MethodArguments.Select(p => p.Name).ToArray());
                        }
                        else
                        {
                            // dont care for the result, just invoke the method
                            codeWriter.WriteComment("Run tested method");
                            codeWriter.WriteMethodInvocation(testData.MethodName, testData.MethodArguments.Select(p => p.Name).ToArray());
                        }

                        if (testData.Result != null)
                        {
                            codeWriter.WriteEmptyLine();
                            codeWriter.WriteComment("Assert method result equals expected result");
                            codeWriter.WriteStatement($"Assert.Equal({testData.Result.Name}, result)");
                        }
                    }
                }
            }
        }

        public void CreateProject(string projectName, string? directory = null)
        {
            throw new NotImplementedException();
        }
    }
}
