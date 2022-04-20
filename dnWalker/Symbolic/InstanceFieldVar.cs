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
        public InstanceFieldVar(IField field, Location instance)
        {
            Debug.Assert(instance != Location.Null);

            Field = field;
            Instance = instance;
            VariableType = VariableTypeExtensions.ToVariableType(field.FieldSig.Type);
        }

        public IField Field { get; }
        public Location Instance { get; }

        public VariableType VariableType { get; }

        public bool Equals(IVariable other)
        {
            return other is InstanceFieldVar fld &&
                Equals(fld);
        }

        public bool Equals(InstanceFieldVar other)
        {
            return Instance == other.Instance &&
                FieldEqualityComparer.CaseInsensitiveCompareDeclaringTypes.Equals(other.Field, Field);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Instance, FieldEqualityComparer.CaseInsensitiveCompareDeclaringTypes.GetHashCode(Field));
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

        public override string ToString()
        {
            string name = Field.Name;
            return $"{Instance}->{name}";
        }
    }
}
