
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a heap node.
    /// </summary>
    public interface IHeapNode : IReadOnlyHeapNode
    {
        /// <summary>
        /// Clones the heap node.
        /// </summary>
        /// <returns></returns>
        new IHeapNode Clone();

        void SetDirty(bool value = true);
    }
}
