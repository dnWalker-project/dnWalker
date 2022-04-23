using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Provides valuation of the heap.
    /// </summary>
    public interface IHeapInfo : IReadOnlyHeapInfo
    {
        new IHeapInfo Clone();

        IReadOnlyHeapInfo IReadOnlyHeapInfo.Clone()
        {
            return Clone();
        }

        bool TryGetNode(Location location, [NotNullWhen(true)] out IHeapNode node);

        bool IReadOnlyHeapInfo.TryGetNode(Location location, [NotNullWhen(true)] out IReadOnlyHeapNode node)
        {
            TryGetNode(location, out IHeapNode n);
            node = n;
            return true;
            //return GetNode(location);
        }

        IObjectHeapNode InitializeObject(TypeSig type);
        IArrayHeapNode InitializeArray(TypeSig elementType, int length);
        // IStringHeapNode InitializeString(TypeSig stringType, string content);
    }
}
