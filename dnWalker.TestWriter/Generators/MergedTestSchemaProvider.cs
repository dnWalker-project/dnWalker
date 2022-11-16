using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    internal class MergedTestSchemaProvider : ITestSchemaProvider
    {
        private readonly IReadOnlyList<ITestSchemaProvider> _schemaProviders;

        public MergedTestSchemaProvider(IReadOnlyList<ITestSchemaProvider> schemaProviders)
        {
            _schemaProviders = schemaProviders;
        }

        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration explorationIteration)
        {
            return _schemaProviders.SelectMany(p => p.GetSchemas(explorationIteration));
        }
    }
}
