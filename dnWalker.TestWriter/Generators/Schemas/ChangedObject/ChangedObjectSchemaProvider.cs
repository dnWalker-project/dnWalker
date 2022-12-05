using dnWalker.Explorations;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas.ChangedObject
{
    internal class ChangedObjectSchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration iteration)
        {
            if (iteration.Exception != null)
            {
                return TestSchema.NoSchemas;
            }

            List<ChangedObjectSchema> changes = new List<ChangedObjectSchema>();
            foreach (IReadOnlyObjectHeapNode changedArray in GetChangedObjects(iteration.InputModel, iteration.OutputModel))
            {
                changes.Add(new ChangedObjectSchema(changedArray.Location, new TestContext(iteration)));
            }
            return changes;
        }
        private static IEnumerable<IReadOnlyHeapNode> GetChangedObjects(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            return outputModel.HeapInfo.Nodes.OfType<IReadOnlyObjectHeapNode>().Where(static n => n.IsDirty);
        }
    }
}
