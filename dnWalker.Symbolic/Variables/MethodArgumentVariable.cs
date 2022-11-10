using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct MethodArgumentVariable : IRootVariable, IEquatable<MethodArgumentVariable>
    {
        private readonly Parameter _parameter;

        public MethodArgumentVariable(Parameter parameter)
        {
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        public string Name => _parameter.IsHiddenThisParameter ? "$this$" : _parameter.Name;
        public TypeSig Type => _parameter.Type;
        public Parameter Parameter => _parameter;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is MethodArgumentVariable mav && Equals(mav);
        }

        public bool Equals(MethodArgumentVariable other)
        {
            return Name == other.Name &&
                TypeEqualityComparer.Instance.Equals(Type, other.Type);
        }

        public bool Equals(IVariable? other)
        {
            return other is MethodArgumentVariable mav && Equals(mav);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TypeEqualityComparer.Instance.GetHashCode(Type));
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public static bool operator ==(MethodArgumentVariable left, MethodArgumentVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MethodArgumentVariable left, MethodArgumentVariable right)
        {
            return !(left == right);
        }
    }
}
