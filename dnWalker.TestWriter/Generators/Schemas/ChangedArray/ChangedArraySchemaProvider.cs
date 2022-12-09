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
    public class ChangedArraySchemaProvider : ITestSchemaProvider
    {
        public IEnumerable<ITestSchema> GetSchemas(ConcolicExplorationIteration iteration)
        {
            if (iteration.Exception != null) 
            {
                return TestSchema.NoSchemas;
            }

            List<ChangedArraySchema> changes = new List<ChangedArraySchema>();
            foreach (Location changedLocation in GetChangedArrays(iteration.InputModel, iteration.OutputModel))
            {
                changes.Add(new ChangedArraySchema(changedLocation, new TestContext(iteration)));
            }
            return changes;
        }


        private static IEnumerable<Location> GetChangedArrays(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            //return outputModel.HeapInfo.Nodes.OfType<IReadOnlyArrayHeapNode>().Where(static n => n.IsDirty);
            List<Location> changedArrays = new List<Location>();
            foreach (IReadOnlyArrayHeapNode inArr in inputModel.HeapInfo.Nodes.OfType<IReadOnlyArrayHeapNode>())
            {
                IReadOnlyArrayHeapNode outArr = (IReadOnlyArrayHeapNode) outputModel.HeapInfo.GetNode(inArr.Location);

                foreach (int index in outArr.Indeces)
                {
                    IValue inElem = inArr.GetElementOrDefault(index);
                    IValue outElem = outArr.GetElementOrDefault(index);

                    if (!outElem.Equals(inElem))
                    {
                        changedArrays.Add(outArr.Location);
                        break;
                    }
                }
            }
            return changedArrays;
        }
    }
}
