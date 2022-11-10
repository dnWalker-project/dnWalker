using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public interface ITestClassWriter
    {
        void WriteTestClass(TextWriter output, ITestClassContext context);
    }
}
