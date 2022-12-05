using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ChangedArray
{
    internal class ChangedArraySchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration iteration)
        {
            if (iteration.Exception != null) 
            {
                return TestSchema.NoSchemas;
            }

            List<ChangedArraySchema> changes = new List<ChangedArraySchema>();
            foreach (IReadOnlyArrayHeapNode changedArray in GetChangedArrays(iteration.InputModel, iteration.OutputModel))
            {
                changes.Add(new ChangedArraySchema(changedArray.Location, new TestContext(iteration)));
            }
            return changes;
        }


        private static IEnumerable<IReadOnlyArrayHeapNode> GetChangedArrays(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            return outputModel.HeapInfo.Nodes.OfType<IReadOnlyArrayHeapNode>().Where(static n => n.IsDirty);
        }
    }
}
