using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct StaticFieldVar : IVariable, IEquatable<StaticFieldVar>
    {
        public StaticFieldVar(string name, TypeSig type, TypeSig declaringType)
        {
            Name = name;
            Type = type;
            DeclaringType = declaringType;
            VariableType = VariableTypeExtensions.ToVariableType(type);
        }

        public string Name { get; }
        public TypeSig Type { get; }
        public TypeSig DeclaringType { get; }

        public VariableType VariableType { get; }

        public bool Equals(IVariable other)
        {
            return other is StaticFieldVar sf && 
                sf.Name == Name && 
                TypeEqualityComparer.Instance.Equals(sf.Type, Type) && 
                TypeEqualityComparer.Instance.Equals(sf.DeclaringType, DeclaringType);
        }

        public bool Equals(StaticFieldVar other)
        {
            return Name == other.Name &&
                   TypeEqualityComparer.Instance.Equals(Type, other.Type) &&
                   TypeEqualityComparer.Instance.Equals(DeclaringType, other.DeclaringType) &&
                   VariableType == other.VariableType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, DeclaringType, VariableType);
        }

        public static bool operator ==(StaticFieldVar left, StaticFieldVar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StaticFieldVar left, StaticFieldVar right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is StaticFieldVar && Equals((StaticFieldVar)obj);
        }
    }
}
