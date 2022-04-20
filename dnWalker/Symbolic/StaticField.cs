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
        public StaticFieldVar(IField field)
        {
            Field = field;
            VariableType = VariableTypeExtensions.ToVariableType(field.FieldSig.Type);
        }

        public IField Field { get; }

        public VariableType VariableType { get; }

        public bool Equals(IVariable other)
        {
            return other is StaticFieldVar sf &&
                Equals(sf);
        }

        public bool Equals(StaticFieldVar other)
        {
            return FieldEqualityComparer.CaseInsensitiveCompareDeclaringTypes.Equals(Field, other.Field);
        }

        public override int GetHashCode()
        {
            return FieldEqualityComparer.CaseInsensitiveCompareDeclaringTypes.GetHashCode(Field);
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

        public override string ToString()
        {
            ITypeDefOrRef declaringType = Field.DeclaringType;
            string name = Field.Name;
            return $"{declaringType.FullName}->{name}";
        }
    }
}
