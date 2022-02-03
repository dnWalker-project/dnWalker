using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public readonly partial struct TypeSignature : IEquatable<TypeSignature>
    {
        private readonly ITypeDefOrRef _type;

        public static readonly TypeSignature Empty = default(TypeSignature);

        public TypeSignature(ITypeDefOrRef type)
        {
            _type = type;
        }


        public string Namespace
        {
            get
            {
                return _type.Namespace;
            }
        }

        public string Name
        {
            get
            {
                return _type.Name;
            }
        }

        public string FullName
        {
            get
            {
                return _type.FullName;
            }
        }

        public TypeSignature CreateArray()
        {
            SZArraySig sig = new SZArraySig(_type.ToTypeSig());
            return new TypeSignature(sig.ToTypeDefOrRef());
        }

        public TypeSignature CreateArray(int rank)
        {
            ArraySig sig = new ArraySig(_type.ToTypeSig(), rank);
            return new TypeSignature(sig.ToTypeDefOrRef());
        }

        public TypeSignature CreateArray(int rank, IEnumerable<int> sizes, IEnumerable<int> lowerBounds)
        {
            ArraySig sig = new ArraySig(_type.ToTypeSig(), rank, sizes.Select(s => (uint)s), lowerBounds);
            return new TypeSignature(sig.ToTypeDefOrRef());
        }

        public bool IsInterface
        {
            get
            {
                return _type.ResolveTypeDefThrow().IsInterface;
            }
        }

        public bool IsClass
        {
            get
            {
                return _type.ResolveTypeDefThrow().IsClass;
            }
        }

        public bool IsValueType
        {
            get
            {
                return _type.ResolveTypeDef().IsValueType;
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return _type.ResolveTypeDef().IsPrimitive;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return _type.ResolveTypeDefThrow().IsAbstract;
            }
        }


        /// <summary>
        /// Determine whether this type is a generic type or generic instance.
        /// </summary>
        public bool IsGeneric
        {
            get
            {
                return _type.ResolveTypeDefThrow().HasGenericParameters;
            }
        }

        public bool IsGenericInstance
        {
            get
            {
                return _type.ToTypeSig().ToGenericInstSig()  != null;
            }
        }

        public TypeSignature[] GetGenericParameters()
        {
            TypeSig ts = _type.ToTypeSig();
            GenericInstSig genInstTS = ts.ToGenericInstSig();
            if (genInstTS != null)
            {
                var genArgs = genInstTS.GenericArguments;

                return genArgs.Select(t => new TypeSignature(t.ToTypeDefOrRef())).ToArray();
            }
            return Array.Empty<TypeSignature>();
        }

        public TypeSignature CreateGenericInstance(params TypeSignature[] genericParameters)
        {
            TypeSpec genericType = new TypeSpecUser(new GenericInstSig(_type.ToTypeSig().ToClassOrValueTypeSig(), genericParameters.Select(ts => ts._type.ToTypeSig()).ToArray()));
            return new TypeSignature(genericType);
        }

        public MethodSignature[] GetMethods()
        {
            TypeDef td = _type.ResolveTypeDefThrow();
            IList<MethodDef> methods = td.Methods;
            MethodSignature[] methodSigs = new MethodSignature[td.Methods.Count];
            for (int i = 0; i < methodSigs.Length; ++i)
            {
                MemberRef methodRef = new MemberRefUser(_type.Module, methods[i].Name, methods[i].MethodSig, _type);
                methodSigs[i] = new MethodSignature(methodRef);
            }
            return methodSigs;
        }

        public MethodSignature[] GetMethods(string methodName)
        {
            ITypeDefOrRef typeRef = _type;

            TypeDef td = typeRef.ResolveTypeDefThrow();
            IList<MethodDef> methods = td.Methods;
            MethodSignature[] methodSigs = td.Methods
                .Where(md => md.Name == methodName)
                .Select(md => new MethodSignature(new MemberRefUser(typeRef.Module, md.Name, md.MethodSig, typeRef)))
                .ToArray();

            return methodSigs;
        }

        public bool IsNested
        {
            get
            {
                return _type.DeclaringType != null;
            }
        }

        public bool IsArray
        {
            get
            {
                return _type.ToTypeSig().IsArray;
            }
        }

        public bool IsSZArray
        {
            get
            {
                return _type.ToTypeSig().IsSZArray;
            }
        }

        public TypeSignature DeclaringType
        {
            get
            {
                return new TypeSignature(_type.DeclaringType);
            }
        }

        public TypeSignature ElementType
        {
            get
            {
                TypeSig ts = _type.ToTypeSig();
                if (ts.IsSZArray)
                {
                    return new TypeSignature(ts.ToSZArraySig().Next.ToTypeDefOrRef());
                }
                else if (ts.IsArray)
                {
                    return new TypeSignature(ts.ToArraySig().Next.ToTypeDefOrRef());
                }
                throw new InvalidOperationException("The type is not an array.");
            }
        }

        #region Equality & HashCode
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

        public ITypeDefOrRef ToTypeDefOrRef()
        {
            return _type;
        }

        public override string ToString()
        {
            return _type.FullName;
        }
        #endregion Equality & HashCode
    }
}
