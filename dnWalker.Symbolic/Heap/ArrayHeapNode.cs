using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public class ArrayHeapNode : HeapNode, IArrayHeapNode
    {
        private readonly Dictionary<int, IValue> _elements = new Dictionary<int, IValue>();

        private ArrayHeapNode(ArrayHeapNode other) : base(other.Location, other.Type, other.IsDirty)
        {
            Length = other.Length;
            _elements = new Dictionary<int, IValue>(other._elements);
        }

        public ArrayHeapNode(Location location, TypeSig elementType, int length, bool isDirty = false) : base(location, new SZArraySig(elementType), isDirty)
        {
            Length = length;
        }

        public override HeapNode Clone()
        {
            return new ArrayHeapNode(this);
        }

        IReadOnlyHeapNode IReadOnlyHeapNode.Clone() => Clone();

        public IValue GetElementOrDefault(int index)
        {
            return GetValueOrDefault(_elements, index, ElementType);
        }

        public void SetElement(int index, IValue value)
        {
            _elements[index] = value;

            SetDirty();
        }

        public int Length
        {
            get;
            set;
        }

        public IEnumerable<int> Indeces => _elements.Keys;

        public TypeSig ElementType => Type.Next;

        public bool HasElements => _elements.Count > 0;

        public bool TryGetElement(int index, [NotNullWhen(true)] out IValue? value)
        {
            return _elements.TryGetValue(index, out value);
        }
    }
}
