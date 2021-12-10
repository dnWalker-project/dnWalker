using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public readonly struct ParameterReference : IEquatable<ParameterReference>
    {
        public ParameterReference(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public override string ToString()
        {
            return Id.ToString("x8");
        }

        public static readonly ParameterReference Empty = new ParameterReference(0);

        public static ParameterReference Parse(ReadOnlySpan<char> str)
        {
            return int.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }

        public static bool TryParse(ReadOnlySpan<char> str, out ParameterReference parameterReference)
        {
            if (int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out int id))
            {
                parameterReference = id;
                return true;
            }

            parameterReference = Empty;
            return false;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParameterReference reference && Equals(reference);
        }

        public bool Equals(ParameterReference other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public IParameter Resolve(ParameterStore parameterStore)
        {
            if (parameterStore.TryGetParameter(Id, out IParameter? resolved))
            {
                return resolved;
            }

            throw new Exception($"Cannot resolve parameter with id = {Id} using provided parameter store");
        }

        public bool TryResolve(ParameterStore parameterStore,[NotNullWhen(true)] out IParameter? resolvedParameter)
        {
            return parameterStore.TryGetParameter(Id,out resolvedParameter);
        }

        public static bool operator ==(ParameterReference left, ParameterReference right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ParameterReference left, ParameterReference right)
        {
            return !(left == right);
        }

        public static implicit operator ParameterReference(int id)
        {
            return new ParameterReference(id);
        }
    }
}
