using dnWalker.Explorations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas
{
    public interface ITestSchemaProvider
    {
        IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration explorationIteration);
    }

    public static class TestSchemaProviderExtensions
    {
        public static IEnumerable<ITestSchema> GetSchemas(this ITestSchemaProvider schemaProvider, ConcolicExploration exploration) 
        { 
            return exploration.Iterations.SelectMany(it => schemaProvider.GetSchemas(it));
        }
    }
}
