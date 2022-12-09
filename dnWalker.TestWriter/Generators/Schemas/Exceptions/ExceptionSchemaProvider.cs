using dnWalker.Explorations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.Exceptions
{
    public class ExceptionSchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration explorationIteration)
        {
            if (explorationIteration.Exception != null) 
            {
                return new ITestSchema[] { new ExceptionSchema(new TestContext(explorationIteration)) };
            }
            else
            {
                return TestSchema.NoSchemas;
            }
        }
    }
}
