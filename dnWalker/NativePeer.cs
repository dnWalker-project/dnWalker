using dnlib.DotNet;
using MMC.Data;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public class NativePeer
    {
        public static NativePeer Get(MethodDef meth)
        {
            if (meth.DeclaringType.FullName == typeof(Environment).FullName)
            {
                switch (meth.Name)
                {
                    case "GetResourceString":
                        return new NativePeer(meth);
                    default:
                        throw new NotImplementedException(meth.FullName);
                }
            }

            if (meth.DeclaringType.FullName == typeof(double).FullName)
            {
                return new NativePeers.SystemDouble(meth);
            }

            if (meth.DeclaringType.FullName == typeof(float).FullName)
            {
                return new NativePeers.SystemSingle(meth);
            }

            if (meth.DeclaringType.FullName == typeof(IntPointer).FullName)
            {
                return new NativePeers.SystemSingle(meth);
            }

            if (meth.DeclaringType.FullName == typeof(RuntimeTypeHandle).FullName)
            {
                return new NativePeers.SystemRuntimeTypeHandle(meth);
            }

            if (meth.DeclaringType.FullName == typeof(RuntimeMethodHandle).FullName)
            {
                return new NativePeers.SystemRuntimeMethodHandle(meth);
            }

            if (meth.DeclaringType.FullName == typeof(RuntimeFieldHandle).FullName)
            {
                return new NativePeers.SystemRuntimeFieldHandle(meth);
            }

            return null;
        }

        protected readonly MethodDef _method;

        public NativePeer(MethodDef method)
        {
            _method = method;
        }

        public virtual bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            dataElement = args[0];
            return true;
        }
    }
}
