using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    /// <summary>
    /// A dummy implementation of variable, used in symbolic tests.
    /// </summary>
    public readonly struct NamedVariable : IRootVariable, IEquatable<NamedVariable>
    {
        private readonly TypeSig _type;
        private readonly string _name;
        public NamedVariable(TypeSig type, string name)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool Equals(NamedVariable other)
        {
            return other._name == _name &&
                TypeEqualityComparer.Instance.Equals(_type, other._type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, TypeEqualityComparer.Instance.GetHashCode(_type));
        }

        public TypeSig Type => _type;
        public string Name => _name;

        public static bool operator ==(NamedVariable left, NamedVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NamedVariable left, NamedVariable right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is NamedVariable nv && Equals(nv);
        }

        public override string ToString()
        {
            return $"{_type.FullName} {_name}";
        }
    }
}
