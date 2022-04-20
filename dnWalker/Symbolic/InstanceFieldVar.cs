using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct InstanceFieldVar : IVariable, IEquatable<InstanceFieldVar>
    {
        public InstanceFieldVar(string name, TypeSig type, Location instance)
        {
            Debug.Assert(instance != Location.Null);

            Name = name;
            Type = type;
            Instance = instance;
            VariableType = VariableTypeExtensions.ToVariableType(type);
        }

        public string Name { get; }
        public TypeSig Type { get; }
        public Location Instance { get; }

        public VariableType VariableType { get; }

        public bool Equals(IVariable other)
        {
            return other is InstanceFieldVar fld &&
                fld.Name == Name &&
                fld.Instance == Instance &&
                TypeEqualityComparer.Instance.Equals(fld.Type, Type);
        }

        public bool Equals(InstanceFieldVar other)
        {
            return Name == other.Name &&
                   TypeEqualityComparer.Instance.Equals(Type, other.Type) &&
                   Instance.Equals(other.Instance) &&
                   VariableType == other.VariableType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type, Instance, VariableType);
        }

        public static bool operator ==(InstanceFieldVar left, InstanceFieldVar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InstanceFieldVar left, InstanceFieldVar right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is InstanceFieldVar && Equals((InstanceFieldVar)obj);
        }
    }
}
