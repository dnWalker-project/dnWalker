using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas
{
    public interface ITestSchema
    {
        ITestContext TestContext { get; }

        void Write(ITestTemplate testTemplate, IWriter output);
    }
}
