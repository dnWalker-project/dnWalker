using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct StringValue : IValue, IEquatable<StringValue>
    {
        public static readonly StringValue Null = new StringValue(null);

        public StringValue(string? content)
        {
            Content = content;
        }

        public string? Content { get; }

        public bool Equals(IValue? other)
        {
            return other is StringValue str && Equals(str);
        }

        public bool Equals(StringValue other)
        {
            return Content == other.Content;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Content);
        }

        public static bool operator ==(StringValue left, StringValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringValue left, StringValue right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is StringValue value && Equals(value);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Content) ? "\"\"" : $"\"{Content}\"";
        }
    }
}
