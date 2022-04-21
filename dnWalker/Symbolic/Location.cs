using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct Location : IValue, IEquatable<Location>
    {
        public static readonly Location Null = new Location(0);

        public Location(uint value)
        {
            Value = value;
        }
        public Location(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            this = new Location((uint)value);
        }

        public uint Value { get; }

        public bool Equals(IValue other)
        {
            return other is Location loc && loc.Value == Value;
        }

        public bool Equals(Location other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(Location left, Location right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Location && Equals((Location)obj);
        }

        IValue IValue.Clone() => Clone();

        public Location Clone()
        {
            return new Location(Value);
        }

        public override string ToString()
        {
            return $"@{Value:X8}";
        }
    }
}
