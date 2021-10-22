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
        private string _directory;
        private string _projectDirectory;

        //private static CodeTypeDeclaration InitializeTestFixture(CodeCompileUnit codeCompileUnit, String ns, String fixtureName)
        //{
        //    CodeNamespace codeNamespace = new CodeNamespace(ns);

        //    codeCompileUnit.Namespaces.Add(codeNamespace);
        //}


        public void CreateProject(string directory, string name)
        {
            _directory = directory;
            _projectDirectory = System.IO.Path.Combine(directory, name);

            System.IO.Directory.CreateDirectory(_projectDirectory);
        }


        public void WriteAsTheory(string methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases)
        {
            
        }

        public void WriteAsFacts(string methodName, IReadOnlyList<dnWalker.Concolic.ExplorationIterationData> cases)
        {

        }
    }
}
