using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public class HeapInfo : IHeapInfo
    {
        private readonly Dictionary<Location, IHeapNode> _nodes;

        private HeapInfo(HeapInfo heapInfo)
        {
            _nodes = new Dictionary<Location, IHeapNode>(heapInfo._nodes.Select(p => KeyValuePair.Create(p.Key, p.Value.Clone())));
            _freeLocation = heapInfo._freeLocation;
        }

        public HeapInfo()
        {
            _nodes = new Dictionary<Location, IHeapNode>();
        }

        public HeapInfo Clone()
        {
            return new HeapInfo(this);
        }

        IHeapInfo IHeapInfo.Clone() => Clone();
        IReadOnlyHeapInfo IReadOnlyHeapInfo.Clone() => Clone();

        public IHeapNode GetNode(Location location)
        {
            if (_nodes.TryGetValue(location, out IHeapNode node)) return node;

            throw new Exception("Invalid location.");
        }

        public IObjectHeapNode InitializeObject(TypeSig type)
        {
            ObjectHeapNode node = new ObjectHeapNode(NextLocation(), type);
            _nodes[node.Location] = node;
            return node;
        }

        public IArrayHeapNode InitializeArray(TypeSig elementType, int length)
        {
            ArrayHeapNode node = new ArrayHeapNode(NextLocation(), elementType, length);
            _nodes[node.Location] = node;
            return node;
        }

        private uint _freeLocation = 1;
        private Location NextLocation()
        {
            return new Location(_freeLocation++);
        }
    }
}
