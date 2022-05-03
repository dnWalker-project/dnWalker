using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct PrimitiveValue<T> : IValue, IEquatable<PrimitiveValue<T>>
        where T : struct
    {
        public T Value { get; }

        public PrimitiveValue(T value)
        {
            Value = value;
        }

        public PrimitiveValue<T> Clone()
        {
            return new PrimitiveValue<T>(Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PrimitiveValue<T> value && Equals(value);
        }
        public bool Equals(IValue? other)
        {
            return other is PrimitiveValue<T> pv && Equals(pv);
        }
        public bool Equals(PrimitiveValue<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(PrimitiveValue<T> left, PrimitiveValue<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PrimitiveValue<T> left, PrimitiveValue<T> right)
        {
            return !(left == right);
        }

        public static implicit operator T(PrimitiveValue<T> value)
        {
            return value.Value;
        }

    }
}
