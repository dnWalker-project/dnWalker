using dnWalker.Explorations;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ReturnValue
{
    internal class ReturnValueSchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration explorationIteration)
        {
            if (explorationIteration.Exception != null &&
                explorationIteration.Exploration.MethodUnderTest.HasReturnValue())
            {
                return new ITestSchema[] { new ReturnValueSchema(new TestContext(explorationIteration)) };
            }
            return TestSchema.NoSchemas;
        }
    }
}
