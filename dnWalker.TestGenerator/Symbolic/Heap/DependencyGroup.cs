using dnWalker.Symbolic.Heap;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Symbolic.Heap
{
    public class DependencyGroup : IReadOnlyCollection<IReadOnlyHeapNode>
    {
        private readonly List<IReadOnlyHeapNode> _heapNodes;

        public DependencyGroup(IEnumerable<IReadOnlyHeapNode> heapNodes)
        {
            _heapNodes = new List<IReadOnlyHeapNode>(heapNodes);
            
            // nodes within one group have proper ordering
            _heapNodes.Sort(static (n1, n2) => n1.Location.Value.CompareTo(n2.Location.Value));
        }

        public int Count
        {
            get
            {
                return ((IReadOnlyCollection<IReadOnlyHeapNode>)_heapNodes).Count;
            }
        }

        public IEnumerator<IReadOnlyHeapNode> GetEnumerator()
        {
            return ((IEnumerable<IReadOnlyHeapNode>)_heapNodes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_heapNodes).GetEnumerator();
        }
    }
}
