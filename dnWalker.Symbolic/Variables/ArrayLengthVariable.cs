using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct ArrayLengthVariable : IMemberVariable, IEquatable<ArrayLengthVariable>
    {
        private readonly IVariable _parent;
        private readonly TypeSig _type;

        public ArrayLengthVariable(IVariable parent)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            
            TypeSig parentType = parent.Type;
            if (!parentType.IsArray &&
                !parentType.IsSZArray &&
                !parentType.IsValueArray)
            {
                throw new ArgumentException("The parent is not array type. Cannot make index access variable.");
            }

            _type = parentType.Module.CorLibTypes.UInt32;
        }

        public TypeSig Type => _type;
        public IVariable Parent => _parent;

        public string Name => $"{_parent.Name}.Length";


        public bool Equals(IVariable? other)
        {
            return other is ArrayLengthVariable ifv && Equals(ifv);
        }
        public override bool Equals(object? obj)
        {
            return obj is ArrayLengthVariable ifv && Equals(ifv);
        }
        public bool Equals(ArrayLengthVariable ev)
        {
            return _parent.Equals(ev.Parent);
        }
        public override int GetHashCode()
        {
            return 23770493 ^ _parent.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(ArrayLengthVariable left, ArrayLengthVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArrayLengthVariable left, ArrayLengthVariable right)
        {
            return !(left == right);
        }
        public bool IsSameMemberAs(IMemberVariable other)
        {
            return other is ArrayLengthVariable;
        }

        public IVariable Substitute(IVariable from, IVariable to)
        {
            if (from is ArrayLengthVariable l &&
                l.Parent.Equals(_parent))
            {
                // we are substituting me
                return to;
            }

            return new ArrayLengthVariable(_parent.Substitute(from, to));
        }
    }
}
