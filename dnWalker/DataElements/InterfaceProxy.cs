using dnlib.DotNet;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.DataElements
{

    public struct InterfaceProxy : IDataElement
    {
        private readonly TypeDef _proxyType;
        private readonly int _hashCode;

        public InterfaceProxy(TypeDef proxyType)
        {
            _proxyType = proxyType;
            _hashCode = 1;
            _hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        public bool Equals(IDataElement other)
        {
            return (other is InterfaceProxy proxy) && proxy._hashCode == _hashCode;
        }

        public bool ToBool()
        {
            return false;
        }

        public string WrapperName
        {
            get
            {
                return _proxyType.FullName;
            }
        }

        public int HashCode
        {
            get
            {
                return _hashCode;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is InterfaceProxy proxy)
            {
                return _hashCode.CompareTo(proxy._hashCode);
            }
            return -1;
        }

        public TypeDef WrappedType
        {
            get
            {
                return _proxyType;
            }
        }

        public void SetMethodResolvers(ExplicitActiveState cur, IDictionary<string, Func<IDataElement>> resolvers)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(this, "method_resolvers", resolvers);
        }
        public bool TryGetMethodResolvers(ExplicitActiveState cur, out IDictionary<string, Func<IDataElement>> resolvers)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute(this, "method_resolvers", out resolvers);
        }

        public bool TryResolveMethod(MethodDef method, ExplicitActiveState cur, out IDataElement result)
        {
            return TryResolveMethod(method, cur, new DataElementList(0), out result);
        }
        public bool TryResolveMethod(MethodDef method, ExplicitActiveState cur, DataElementList args, out IDataElement result)
        {
            if (_proxyType.Methods.Contains(method))
            {
                var methodName = method.FullName;

                if (TryGetMethodResolvers(cur, out var resolvers))
                {
                    if (resolvers.TryGetValue(methodName, out var resultFactory))
                    {
                        result = resultFactory();

                        // result has already pinned its own expression from the initialization

                        return true;
                    }
                }

                //if (cur.PathStore.CurrentPath.TryGetObjectAttribute(this, methodName, out result))
                //{
                //    return true;
                //}
            }
            result = ObjectReference.Null; //GetDefaultValue(method.ReturnType);
            return false;
        }

        public override string ToString()
        {
            return "InterfaceProxy for " + _proxyType.FullName;
        }
    }
}
