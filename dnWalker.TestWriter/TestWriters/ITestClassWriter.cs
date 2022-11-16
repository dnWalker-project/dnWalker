using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestWriters
{
    public interface ITestClassWriter : IDisposable
    {
        void Write(TestClass testClass);
    }
}

