using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public interface ITestClassGenerator
    {
        void GenerateTestClass(ITestClassContext context);
    }
}
