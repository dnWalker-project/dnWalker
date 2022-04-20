using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnlib.DotNet;

namespace dnWalker.Symbolic
{
    public readonly struct MethodArgVar: IVariable, IEquatable<MethodArgVar>
    {
        public MethodArgVar(string name, TypeSig type)
        {
            Name = name;
            Type = type;
            VariableType = VariableTypeExtensions.ToVariableType(type);
        }

        public string Name { get; }
        public TypeSig Type {get; }

        public VariableType VariableType { get; }

        public bool Equals(IVariable other)
        {
            return other is MethodArgVar ma && ma.Name == Name && TypeEqualityComparer.Instance.Equals(ma.Type, Type);
        }

        public bool Equals(MethodArgVar other)
        {
            return Name == other.Name &&
                  TypeEqualityComparer.Instance.Equals(Type, other.Type) &&
                   VariableType == other.VariableType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, VariableType);
        }

        public static bool operator ==(MethodArgVar left, MethodArgVar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MethodArgVar left, MethodArgVar right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is MethodArgVar ma && Equals(ma);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
