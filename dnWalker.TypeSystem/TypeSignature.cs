using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public readonly struct TypeSignature : IEquatable<TypeSignature>
    {
        private readonly ITypeDefOrRef _type;

        public static readonly TypeSignature Empty = default(TypeSignature);

        public ITypeDefOrRef Type
        {
            get
            {
                return _type;
            }
        }

        public TypeSignature(ITypeDefOrRef type)
        {
            _type = type;
        }

        public override bool Equals(object? obj)
        {
            return obj is TypeSignature signature && Equals(signature);
        }

        public bool Equals(TypeSignature other)
        {
            SigComparer sigComparer = new SigComparer();

            return sigComparer.Equals(_type, other._type);
        }

        public override int GetHashCode()
        {
            SigComparer sigComparer = new SigComparer();
            return sigComparer.GetHashCode(_type);
        }

        public static bool operator ==(TypeSignature left, TypeSignature right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeSignature left, TypeSignature right)
        {
            return !(left == right);
        }

        public TypeSignature MakeArray()
        {
            SZArraySig sig = new SZArraySig(_type.ToTypeSig());
            return new TypeSignature(sig.ToTypeDefOrRef());
        }

        public override string ToString()
        {
            return _type.FullName;
        }
    }
}
