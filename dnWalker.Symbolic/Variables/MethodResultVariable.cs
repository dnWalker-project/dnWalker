using dnlib.DotNet;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct MethodResultVariable : IMemberVariable, IEquatable<MethodResultVariable>
    {
        private readonly IVariable _parent;
        private readonly IMethod _method;
        private readonly int _invocation;

        public MethodResultVariable(IVariable parent, IMethod method, int invocation)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _invocation = invocation;
            if (_method.ResolveMethodDefThrow().IsStatic) throw new ArgumentException("method must be an instance method.", nameof(method));

            // check that the this variable makes sense
            ITypeDefOrRef parentType = parent.Type.ToTypeDefOrRef();
            ITypeDefOrRef declType = method.DeclaringType;

            if (!parentType.IsAssignableTo(declType))
            {
                throw new InvalidOperationException("Cannot create this instance method result variable. The method declaring type is incompatible with the parent type.");
            }
        }
        public string Name => $"{_parent.Name}.{_method.Name}({_invocation})";
        public TypeSig Type => _method.MethodSig.RetType;
        public IVariable Parent => _parent;
        public IMethod Method => _method;

        public int Invocation => _invocation;

        public bool Equals(IVariable? other)
        {
            return other is MethodResultVariable ifv && Equals(ifv);
        }
        public override bool Equals(object? obj)
        {
            return obj is MethodResultVariable ifv && Equals(ifv);
        }
        public bool Equals(MethodResultVariable ifv)
        {
            return _invocation == ifv._invocation &&
                MethodEqualityComparer.CompareDeclaringTypes.Equals(_method, ifv._method) &&
                _parent.Equals(ifv.Parent);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(MethodEqualityComparer.CompareDeclaringTypes.GetHashCode(_method), _parent, _invocation);
        }
        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(MethodResultVariable left, MethodResultVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MethodResultVariable left, MethodResultVariable right)
        {
            return !(left == right);
        }
        public bool IsSameMemberAs(IMemberVariable other)
        {
            return other is MethodResultVariable mrv && 
                mrv._invocation == _invocation && 
                MethodEqualityComparer.CompareDeclaringTypes.Equals(_method, mrv._method);
        }

        public IVariable Substitute(IVariable from, IVariable to)
        {
            if (from is MethodResultVariable met &&
                MethodEqualityComparer.CompareDeclaringTypes.Equals(_method, met._method) &&
                _invocation == met._invocation &&
                met.Parent.Equals(_parent))
            {
                // we are substituting me
                return to;
            }

            return new MethodResultVariable(_parent.Substitute(from, to), _method, _invocation);
        }
    }
}
