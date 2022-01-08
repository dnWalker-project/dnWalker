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

        public static readonly MethodSignature Empty = new MethodSignature();

        public static MethodSignature Parse(string methodSignature)
        {
            if (!TryParse(methodSignature, out MethodSignature result))
            {
                throw new FormatException(methodSignature);
            }

            return result;
        }

        static readonly string retType2DeclType = " ";
        static readonly string declType2MethodName = "::";
        static readonly string[] methodName2Args = { "(", ")" };
        static readonly string arg2Arg = ",";
        public static bool TryParse(string methodSignatureString, out MethodSignature methodSignature)
        {

            methodSignature = MethodSignature.Empty;

            if (string.IsNullOrWhiteSpace(methodSignatureString))
            {
                return false;
            }

            //string[] parts = methodSignatureString.Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);


            // get ReturnType | Declaring.Type::MethodName(ArgTypes...);
            string[] parts = methodSignatureString.Split(retType2DeclType, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return false;
            string returnType = parts[0];

            // get Declaring.Type | MethodName(ArgTypes...);
            parts = parts[1].Split(declType2MethodName, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return false;
            string declType = parts[0];

            // get MethodName | ArgType1,ArgType2... |
            // ! ArgType N can have zero elements
            parts = parts[1].Split(methodName2Args, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2) return false;
            string methodName = parts[0];

            string[] args = parts.Length == 2 ? parts[1].Split(arg2Arg, StringSplitOptions.RemoveEmptyEntries) : Array.Empty<string>();

            methodSignature = new MethodSignature(returnType, declType, methodName, args);

            return true;
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
                   ArgumentTypeFullNames.SequenceEqual(other.ArgumentTypeFullNames);
        }

        public override int GetHashCode()
        {
            //return ToString().GetHashCode();
            HashCode hashCode = new HashCode();
            hashCode.Add(ReturnTypeFullName);
            hashCode.Add(DeclaringTypeFullName);
            hashCode.Add(MethodName);

            foreach (string arg in ArgumentTypeFullNames)
            {
                hashCode.Add(arg);
            }

            return hashCode.ToHashCode();
        }

        public static bool operator ==(MethodSignature left, MethodSignature right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MethodSignature left, MethodSignature right)
        {
            return !(left == right);
        }

        public static implicit operator MethodSignature(string str)
        {
            return Parse(str);
        }

        public static implicit operator string?(MethodSignature ms)
        {
            return ms.ToString();
        }
    }
}
