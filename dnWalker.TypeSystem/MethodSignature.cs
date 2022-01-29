using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public readonly struct MethodSignature : IEquatable<MethodSignature>
    {
        private readonly IMethod _method;

        public static readonly MethodSignature Empty = default(MethodSignature);

        public IMethod Method
        {
            get
            {
                return _method;
            }
        }

        public MethodSignature(IMethod method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public override bool Equals(object? obj)
        {
            return obj is MethodSignature signature && Equals(signature);
        }

        public bool Equals(MethodSignature other)
        {
            SigComparer comparer = new SigComparer();
            return comparer.Equals(_method, other._method);
        }

        public override int GetHashCode()
        {
            SigComparer comparer = new SigComparer();

            return comparer.GetHashCode(_method);
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
