using dnWalker.Explorations;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace dnWalker.TestWriter.Generators.Schemas.ChangedObject
{
    public class ChangedObjectSchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration iteration)
        {
            if (iteration.Exception != null)
            {
                return TestSchema.NoSchemas;
            }

            List<ChangedObjectSchema> changes = new List<ChangedObjectSchema>();
            foreach (Location changedLocation in GetChangedObjects(iteration.InputModel, iteration.OutputModel))
            {
                changes.Add(new ChangedObjectSchema(changedLocation, new TestContext(iteration)));
            }
            return changes;
        }
        private static IEnumerable<Location> GetChangedObjects(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            //return outputModel.HeapInfo.Nodes.OfType<IReadOnlyObjectHeapNode>().Where(static n => n.IsDirty);
            List<Location> changedObjects = new List<Location>();
            foreach (IReadOnlyObjectHeapNode inObj in inputModel.HeapInfo.Nodes.OfType<IReadOnlyObjectHeapNode>())
            {
                IReadOnlyObjectHeapNode outObj = (IReadOnlyObjectHeapNode)outputModel.HeapInfo.GetNode(inObj.Location);

                foreach (IField field in outObj.Fields)
                {
                    IValue outFieldVal = outObj.GetFieldOrDefault(field);

                    if (!inObj.TryGetField(field, out IValue? inFieldVal) || !outFieldVal.Equals(inFieldVal))
                    {
                        changedObjects.Add(outObj.Location);
                        break;
                    }
                }
            }
            return changedObjects;
        }
    }
}
