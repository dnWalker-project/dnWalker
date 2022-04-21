using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct NamedVar : IVariable, IEquatable<NamedVar>
    {
        public NamedVar(TypeSig type, string name)
        {
            Type = type;
            VariableType = VariableTypeExtensions.ToVariableType(type);
            Name = name;
        }

        public VariableType VariableType { get; }
        public TypeSig Type { get; }
        public string Name { get; }

        public bool Equals(IVariable other)
        {
            return other is NamedVar nv && Equals(nv);
        }

        public bool Equals(NamedVar other)
        {
            return VariableType == other.VariableType &&
                   TypeEqualityComparer.Instance.Equals(Type, other.Type) &&
                   Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is NamedVar nv && Equals(nv);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeEqualityComparer.Instance.GetHashCode(Type), Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(NamedVar left, NamedVar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NamedVar left, NamedVar right)
        {
            return !(left == right);
        }

    }
}
