using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct ReturnValueVariable : IRootVariable, IEquatable<ReturnValueVariable>
    {
        private readonly IMethod _method;
        public ReturnValueVariable(IMethod method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public bool Equals(ReturnValueVariable other)
        {
            return MethodEqualityComparer.CompareDeclaringTypes.Equals(_method, other._method);
        }

        public override int GetHashCode()
        {
            return MethodEqualityComparer.CompareDeclaringTypes.GetHashCode(_method);
        }

        public IMethod Method => _method;

        public static bool operator ==(ReturnValueVariable left, ReturnValueVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReturnValueVariable left, ReturnValueVariable right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is ReturnValueVariable rvv && Equals(rvv);
        }

        public override string ToString()
        {
            return Name;
        }

        public TypeSig Type => _method.MethodSig.RetType;

        public string Name => $"Return Value: {_method}";

        public bool Equals(IVariable? other)
        {
            return other is ReturnValueVariable rvv && Equals(rvv);
        }
    }
}
