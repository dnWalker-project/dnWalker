using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Heap
{
    public class HeapVertex
    {
        private readonly IReadOnlyHeapInfo _heap;
        private readonly IReadOnlyHeapNode _node;

        public HeapVertex(IReadOnlyHeapInfo heap, IReadOnlyHeapNode node)
        {
            _heap = heap ?? throw new ArgumentNullException(nameof(heap));
            _node = node ?? throw new ArgumentNullException(nameof(node));
        }


        public IReadOnlyHeapInfo Heap => _heap;
        public IReadOnlyHeapNode Node => _node;
    }
}
