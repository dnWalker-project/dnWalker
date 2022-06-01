using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public class ObjectHeapNode : HeapNode, IObjectHeapNode
    {
        private class MethodInvocationComparer : IEqualityComparer<(IMethod m, int i)>
        {
            // we need to compare declaring types as same method signature may be used by multiple interfaces...
            private static readonly IEqualityComparer<IMethod> _methodComparer = MethodEqualityComparer.CaseInsensitiveCompareDeclaringTypes;

            public bool Equals((IMethod m, int i) x, (IMethod m, int i) y)
            {
                return x.i == y.i && // first the super fast and simple part
                    _methodComparer.Equals(x.m, y.m);
            }

            public int GetHashCode([DisallowNull] (IMethod m, int i) obj)
            {
                return HashCode.Combine(obj.m, obj.i);
            }
        }

        // we need to compare declaring types as same field signature may be used by base and descendant class
        private static readonly IEqualityComparer<IField> _fieldComaparer = FieldEqualityComparer.CompareDeclaringTypes;
        private static readonly IEqualityComparer<(IMethod, int)> _methodInvocationComparer = new MethodInvocationComparer();

        public ObjectHeapNode(Location location, TypeSig type) : base(location, type)
        {
        }

        private ObjectHeapNode(ObjectHeapNode other) : base(other.Location, other.Type)
        {
            _fields = new Dictionary<IField, IValue>(other._fields, _fieldComaparer);
            _methods = new Dictionary<(IMethod, int), IValue>(other._methods, _methodInvocationComparer);
        }

        private readonly Dictionary<IField, IValue> _fields = new Dictionary<IField, IValue>(_fieldComaparer);
        private readonly Dictionary<(IMethod, int), IValue> _methods = new Dictionary<(IMethod, int), IValue>(_methodInvocationComparer);

        public override ObjectHeapNode Clone()
        {
            return new ObjectHeapNode(this);
        }
        IReadOnlyHeapNode IReadOnlyHeapNode.Clone() => Clone();

        public IValue GetField(IField field)
        {
            return GetValueOrDefault(_fields, field, field.FieldSig.GetFieldType());
        }

        public void SetField(IField field, IValue value)
        {
            System.Diagnostics.Debug.Assert(field != null);

            _fields[field] = value;
        }

        public IValue GetMethodResult(IMethod method, int invocation)
        {
            System.Diagnostics.Debug.Assert(method != null);
            System.Diagnostics.Debug.Assert(invocation >= 0);

            return GetValueOrDefault(_methods, (method, invocation), method.MethodSig.RetType);
        }

        public void SetMethodResult(IMethod method, int invocation, IValue value)
        {
            System.Diagnostics.Debug.Assert(method != null);
            System.Diagnostics.Debug.Assert(invocation >= 0);

            _methods[(method, invocation)] = value;
        }

        public IEnumerable<IField> Fields => _fields.Keys;

        public IEnumerable<(IMethod method, int invocation)> MethodInvocations => _methods.Keys;

        public bool HasFields => _fields.Count > 0;

        public bool HasMethodInvocations => _methods.Count > 0;
    }
}
