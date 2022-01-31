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

        public MethodSignature(IMethod method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public string Name
        {
            get
            {
                return _method.Name;
            }
        }

        public string FullName
        {
            get
            {
                return _method.FullName;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return _method.ResolveMethodDefThrow().IsAbstract;
            }
        }

        public bool IsGeneric
        {
            get
            {
                return _method.ResolveMethodDefThrow().HasGenericParameters;
            }
        }

        public bool IsGenericInstance
        {
            get
            {
                if (_method is MethodSpec methodSpec)
                {
                    return methodSpec.GenericInstMethodSig.IsGenericInst && methodSpec.GenericInstMethodSig.GenericArguments.Count > 0;
                }
                return false;
            }
        }

        public TypeSignature[] GetGenericParameters()
        {
            if (_method is MethodSpec methodSpec)
            {
                return methodSpec.GenericInstMethodSig.GenericArguments.Select(ts => new TypeSignature(ts.ToTypeDefOrRef())).ToArray();
            }

            throw new InvalidOperationException("The method is not a generic instance.");
        }

        public IMethod ToMethod()
        {
            return _method;
        }

        public IMethodDefOrRef ToMethodRefOrDef()
        {
            return _method as IMethodDefOrRef ?? (_method as MethodSpec)?.Method ?? throw new Exception("Cannot get the method definition or reference.");
        }

        public TypeSignature DeclaringType
        {
            get
            {
                return new TypeSignature(_method.DeclaringType);
            }
        }

        public MethodSignature CreateGenericInstance(TypeSignature declaringType, params TypeSignature[] genericParameters)
        {
            ITypeDefOrRef declType = declaringType.ToTypeDefOrRef();
            MethodDef md = _method.ResolveMethodDefThrow();
            MemberRef methodRef = new MemberRefUser(declType.Module, md.Name, md.MethodSig);
            MethodSpec methodSpec = new MethodSpecUser(methodRef, new GenericInstMethodSig(genericParameters.Select(p => p.ToTypeDefOrRef().ToTypeSig()).ToArray()));
            return new MethodSignature(methodSpec);
        }

        public TypeSignature ReturnType
        {
            get
            {
                return new TypeSignature(_method.MethodSig.RetType.ToTypeDefOrRef());
            }
        }

        public TypeSignature[] Parameters
        {
            get
            {
                return _method.GetParams().Select(ts => new TypeSignature(ts.ToTypeDefOrRef())).ToArray();
            }
        }

        #region Equality & HashCode
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

        public override string ToString()
        {
            return _method.FullName;
        }
        #endregion Equality & HashCode
    }
}
