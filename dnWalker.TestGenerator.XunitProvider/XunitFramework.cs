using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestFrameworks;
using dnWalker.TestGenerator.TestProjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XunitProvider
{
    /// <summary>
    /// Implementation of <see cref="ITestFramework"/> for xUnit.net.
    /// </summary>
    public class XunitFramework : ITestFramework
    {


        public void InitializeProjectContext(ITestProjectContext context)
        {
            throw new NotImplementedException();
        }

        public void InitializeClassContext(ITestClassContext context)
        {
            throw new NotImplementedException();
        }

        public ITestClassGenerator CreateClassGenerator()
        {
            throw new NotImplementedException();
        }

        public ITestProjectGenerator CreateProjectGenerator()
        {
            throw new NotImplementedException();
        }
    }
}
