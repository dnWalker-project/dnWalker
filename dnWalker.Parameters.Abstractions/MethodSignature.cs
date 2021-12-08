using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public readonly struct MethodSignature : IEquatable<MethodSignature>
    {
        public MethodSignature(string returnTypeFullName, string declaringTypeFullName, string methodName, string[] arguments)
        {
            if (string.IsNullOrWhiteSpace(returnTypeFullName))
            {
                throw new ArgumentException($"'{nameof(returnTypeFullName)}' cannot be null or whitespace.", nameof(returnTypeFullName));
            }

            if (string.IsNullOrWhiteSpace(declaringTypeFullName))
            {
                throw new ArgumentException($"'{nameof(declaringTypeFullName)}' cannot be null or whitespace.", nameof(declaringTypeFullName));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($"'{nameof(methodName)}' cannot be null or whitespace.", nameof(methodName));
            }

            ReturnTypeFullName = returnTypeFullName;
            DeclaringTypeFullName = declaringTypeFullName;
            MethodName = methodName;
            ArgumentTypeFullNames = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public string ReturnTypeFullName { get; }
        public string DeclaringTypeFullName { get; }
        public string MethodName { get; }
        public string[] ArgumentTypeFullNames { get; }

        public override string ToString()
        {
            return $"{ReturnTypeFullName} {DeclaringTypeFullName}::{MethodName}({string.Join(',', ArgumentTypeFullNames)})";
        }

        private static readonly char[] _splitChars = new char[] { ' ', ':', '(', ',', ')' };

        public static MethodSignature Parse(string methodSignature)
        {
            string[] parts = methodSignature.Split(_splitChars,StringSplitOptions.RemoveEmptyEntries);
            ref string returnType = ref parts[0];
            ref string declType = ref parts[1];
            ref string methodName = ref parts[2];

            string[] args = parts.Skip(3).ToArray();

            return new MethodSignature(returnType, declType, methodName, args);
        }

        public override bool Equals(object? obj)
        {
            return obj is MethodSignature signature && Equals(signature);
        }

        public bool Equals(MethodSignature other)
        {
            return ReturnTypeFullName == other.ReturnTypeFullName &&
                   DeclaringTypeFullName == other.DeclaringTypeFullName &&
                   MethodName == other.MethodName &&
                   EqualityComparer<string[]>.Default.Equals(ArgumentTypeFullNames, other.ArgumentTypeFullNames);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(MethodSignature left, MethodSignature right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MethodSignature left, MethodSignature right)
        {
            return !(left == right);
        }
    }
}
