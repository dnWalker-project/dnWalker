using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// <param name="node"></param>
        /// <returns></returns>
        bool TryGetNode(Location location, [NotNullWhen(true)] out IReadOnlyHeapNode? node);
        IReadOnlyHeapNode GetNode(Location location);

        /// <summary>
        /// Gets the locations.
        /// </summary>
        IReadOnlyCollection<Location> Locations { get; }
        
        /// <summary>
        /// Gets the heap nodes.
        /// </summary>
        IReadOnlyCollection<IHeapNode> Nodes { get; }
    }

    public static class HeapInfoExtensions
    {
        public static bool IsEmpty(this IReadOnlyHeapInfo self)
        {
            return self.Locations.Count == 0;
        }
    }
}
