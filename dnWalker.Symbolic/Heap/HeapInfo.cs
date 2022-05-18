using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public IReadOnlyCollection<Location> Locations => _nodes.Keys;
        public IReadOnlyCollection<IHeapNode> Nodes => _nodes.Values;

        public bool TryGetNode(Location location, [NotNullWhen(true)]out IHeapNode? node)
        {
            return _nodes.TryGetValue(location, out node);
        }

        public IHeapNode GetNode(Location location)
        {
            return _nodes[location];
        }

        IReadOnlyHeapNode IReadOnlyHeapInfo.GetNode(Location location)
        {
            return GetNode(location);
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

        public bool AddNode(IHeapNode node)
        {
            Location location = node.Location;
            if (_nodes.ContainsKey(location)) return false; // already occupied => do nothing...

            uint intLoc = location.Value;
            if (intLoc > _freeLocation)
            {
                _freeLocation = intLoc + 1;
            }

            _nodes[location] = node;

            return true;
        }
    }
}
