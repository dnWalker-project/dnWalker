using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct ArrayElementVariable : IMemberVariable, IEquatable<ArrayElementVariable>
    {
        private readonly IVariable _parent;
        private readonly int _index;

        public ArrayElementVariable(IVariable parent, int index)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _index = index;

            TypeSig parentType = parent.Type;
            if (!parentType.IsArray && 
                !parentType.IsSZArray &&
                !parentType.IsValueArray)
            {
                throw new ArgumentException("The parent is not array type. Cannot make index access variable.");
            }
        }

        public TypeSig Type => _parent.Type.Next;
        public IVariable Parent => _parent;

        public string Name => $"{_parent.Name}[{_index}]";

        public int Index => _index;

        public bool Equals(IVariable? other)
        {
            return other is ArrayElementVariable ifv && Equals(ifv);
        }
        public override bool Equals(object? obj)
        {
            return obj is ArrayElementVariable ifv && Equals(ifv);
        }
        public bool Equals(ArrayElementVariable ev)
        {
            return _index == ev._index &&
                _parent.Equals(ev.Parent);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(_index, _parent);
        }
        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(ArrayElementVariable left, ArrayElementVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArrayElementVariable left, ArrayElementVariable right)
        {
            return !(left == right);
        }

        public bool IsSameMemberAs(IMemberVariable other)
        {
            return other is ArrayElementVariable aev && 
                aev._index == _index;
        }

        public IVariable Substitute(IVariable from, IVariable to)
        {
            if (from is ArrayElementVariable arr && 
                arr._index == _index && 
                arr.Parent.Equals(_parent))
            {
                // we are substituting me
                return to;
            }

            return new ArrayElementVariable(_parent.Substitute(from, to), _index);
        }
    }
}
