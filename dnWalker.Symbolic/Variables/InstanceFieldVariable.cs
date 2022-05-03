using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct InstanceFieldVariable : IMemberVariable, IEquatable<InstanceFieldVariable>
    {
        private readonly IVariable _parent;
        private readonly IField _field;

        public InstanceFieldVariable(IVariable parent, IField field)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _field = field ?? throw new ArgumentNullException(nameof(field));

            if (_field.ResolveFieldDefThrow().IsStatic) throw new ArgumentException("Field must be an instance field.", nameof(field));

            // check that the this variable makes sense
            ITypeDefOrRef parentType = parent.Type.ToTypeDefOrRef();
            ITypeDefOrRef declType = field.DeclaringType;

            if (!parentType.IsAssignableTo(declType))
            {
                throw new InvalidOperationException("Cannot create this instance field variable. The field declaring type is incompatible with the parent type.");
            }
        }
        public string Name => $"{_parent.Name}::{_field.FullName}";
        public TypeSig Type => _field.FieldSig.Type;
        public IVariable Parent => _parent;
        public IField Field => _field;

        public bool Equals(IVariable? other)
        {
            return other is InstanceFieldVariable ifv && Equals(ifv);
        }
        public override bool Equals(object? obj)
        {
            return obj is InstanceFieldVariable ifv && Equals(ifv);
        }
        public bool Equals(InstanceFieldVariable ifv)
        {
            return FieldEqualityComparer.CompareDeclaringTypes.Equals(_field, ifv._field) &&
                _parent.Equals(ifv.Parent);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(FieldEqualityComparer.CompareDeclaringTypes.GetHashCode(_field), _parent);
        }
        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(InstanceFieldVariable left, InstanceFieldVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InstanceFieldVariable left, InstanceFieldVariable right)
        {
            return !(left == right);
        }
        public bool IsSameMemberAs(IMemberVariable other)
        {
            return other is InstanceFieldVariable ifv && 
                FieldEqualityComparer.CompareDeclaringTypes.Equals(_field, ifv._field);
        }
    }
}
