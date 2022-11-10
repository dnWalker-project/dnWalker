using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct StaticFieldVariable : IRootVariable, IEquatable<StaticFieldVariable>
    {
        private readonly IField _field;

        public string Name => _field.FullName;
        public TypeSig Type => _field.FieldSig.Type;

        public IField Field => _field;

        public StaticFieldVariable(IField field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
            if (!field.ResolveFieldDefThrow().IsStatic) throw new ArgumentException("Field must be static.", nameof(field));
        }

        public bool Equals(IVariable? other)
        {
            return other is StaticFieldVariable sfv && Equals(sfv);
        }

        public bool Equals(StaticFieldVariable other)
        {
            return FieldEqualityComparer.CompareDeclaringTypes.Equals(_field, other._field);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is StaticFieldVariable sfv && Equals(sfv);
        }

        public override int GetHashCode()
        {
            return FieldEqualityComparer.CompareDeclaringTypes.GetHashCode(_field);
        }

        public override string ToString()
        {
            return _field.FullName;
        }

        public static bool operator ==(StaticFieldVariable left, StaticFieldVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StaticFieldVariable left, StaticFieldVariable right)
        {
            return !(left == right);
        }
    }
}
