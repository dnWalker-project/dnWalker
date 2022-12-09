using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestWriters
{
    public interface ITestProjectWriter : IDisposable
    {
        void WriteTestClasses(TestProject testProject);
        void WriteTestProject(TestProject testProject);
    }
}
