using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct ElementVar : IVariable, IEquatable<IVariable>, IEquatable<ElementVar>
    {
        public ElementVar(int index, TypeSig type, Location array)
        {
            Debug.Assert(array != Location.Null);

            Index = index;
            Type = type;
            Array = array;
            VariableType = VariableTypeExtensions.ToVariableType(type);
        }

        public int Index { get; }
        public TypeSig Type { get; }
        public Location Array { get; }
        public VariableType VariableType { get; }

        public override bool Equals(object obj)
        {
            return obj is ElementVar var && Equals(var);
        }

        public bool Equals(ElementVar other)
        {
            return Index == other.Index &&
                   TypeEqualityComparer.Instance.Equals(Type, other.Type) &&
                   Array.Equals(other.Array) &&
                   VariableType == other.VariableType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index, Type, Array, VariableType);
        }

        public static bool operator ==(ElementVar left, ElementVar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ElementVar left, ElementVar right)
        {
            return !(left == right);
        }

        public bool Equals(IVariable other)
        {
            return other is ElementVar ev && Equals(ev);
        }

        public override string ToString()
        {
            return $"{Array}[{Index}]";
        }
    }
}
