using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a read only heap.
    /// </summary>
    public interface IReadOnlyHeapInfo
    {
        /// <summary>
        /// Clones the instance.
        /// </summary>
        /// <returns></returns>
        IReadOnlyHeapInfo Clone();

        /// <summary>
        /// Gets heap node associated with the specified location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        IReadOnlyHeapNode GetNode(Location location);
    }
}
