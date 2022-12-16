using dnlib.DotNet;

using dnWalker.TestWriter.Generators.Schemas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class BasicTestMethodNamingStrategy : ITestMethodNamingStrategy
    {
        public string GetMethodName(IMethod methodUnderTest, ITestSchema testSchema)
        {
            return $"{methodUnderTest.Name}{testSchema.GetType().Name}";
        }
    }
}
