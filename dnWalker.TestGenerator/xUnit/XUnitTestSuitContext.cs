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
        private String _directory;
        private String _projectDirectory;

        //private static CodeTypeDeclaration InitializeTestFixture(CodeCompileUnit codeCompileUnit, String ns, String fixtureName)
        //{
        //    CodeNamespace codeNamespace = new CodeNamespace(ns);

        //    codeCompileUnit.Namespaces.Add(codeNamespace);
        //}


        public void CreateProject(String directory, String name)
        {
            _directory = directory;
            _projectDirectory = System.IO.Path.Combine(directory, name);

            System.IO.Directory.CreateDirectory(_projectDirectory);
        }


        public void WriteAsTheory(String methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases)
        {
            
        }

        public void WriteAsFacts(String methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases)
        {

        }
    }
}
