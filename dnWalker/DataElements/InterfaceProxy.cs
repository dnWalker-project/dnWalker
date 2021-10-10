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
        private readonly Int32 _hashCode;

        public InterfaceProxy(TypeDef proxyType)
        {
            _proxyType = proxyType;
            _hashCode = 1;
            _hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        public Boolean Equals(IDataElement other)
        {
            return (other is InterfaceProxy proxy) && proxy._hashCode == _hashCode;
        }

        public Boolean ToBool()
        {
            return false;
        }

        public String WrapperName
        {
            get
            {
                return _proxyType.FullName;
            }
        }

        public Int32 HashCode
        {
            get
            {
                return _hashCode;
            }
        }

        public Int32 CompareTo(Object obj)
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

        public void SetMethodResolvers(ExplicitActiveState cur, IDictionary<String, Func<IDataElement>> resolvers)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(this, "method_resolvers", resolvers);
        }
        public Boolean TryGetMethodResolvers(ExplicitActiveState cur, out IDictionary<String, Func<IDataElement>> resolvers)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute(this, "method_resolvers", out resolvers);
        }

        public Boolean TryResolveMethod(MethodDef method, ExplicitActiveState cur, out IDataElement result)
        {
            return TryResolveMethod(method, cur, new DataElementList(0), out result);
        }
        public Boolean TryResolveMethod(MethodDef method, ExplicitActiveState cur, DataElementList args, out IDataElement result)
        {
            if (_proxyType.Methods.Contains(method))
            {
                String methodName = method.FullName;

                if (TryGetMethodResolvers(cur, out IDictionary<String, Func<IDataElement>> resolvers))
                {
                    if (resolvers.TryGetValue(methodName, out Func<IDataElement> resultFactory))
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

        public override String ToString()
        {
            return "InterfaceProxy for " + _proxyType.FullName;
        }
    }
}
