using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public class ArrayHeapNode : HeapNode, IArrayHeapNode
    {
        private readonly Dictionary<int, IValue> _elements = new Dictionary<int, IValue>();

        private ArrayHeapNode(ArrayHeapNode other) : base(other.Location, other.Type)
        {
            _elements = new Dictionary<int, IValue>(other._elements);
        }

        public ArrayHeapNode(Location location, TypeSig elementType, int lenght) : base(location, elementType)
        {
            Length = lenght;
        }

        public override HeapNode Clone()
        {
            return new ArrayHeapNode(this);
        }

        IReadOnlyHeapNode IReadOnlyHeapNode.Clone() => Clone();

        public IValue GetElement(int index)
        {
            return GetValueOrDefault(_elements, index, Type);
        }

        public void SetElement(int index, IValue value)
        {
            _elements[index] = value;
        }

        public int Length
        {
            get;
            set;
        }

        public IEnumerable<int> Indeces => _elements.Keys;
    }
}
