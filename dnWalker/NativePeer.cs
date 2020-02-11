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
    public abstract class NativePeer
    {
        public static NativePeer Get(MethodDef methodDef)
        {
            if (methodDef.DeclaringType.FullName == typeof(Environment).FullName)
            {
                return new NativePeers.SystemEnvironment(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(double).FullName)
            {
                return new NativePeers.SystemDouble(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(float).FullName)
            {
                return new NativePeers.SystemSingle(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(IntPointer).FullName)
            {
                return new NativePeers.SystemSingle(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(RuntimeTypeHandle).FullName)
            {
                return new NativePeers.SystemRuntimeTypeHandle(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(RuntimeMethodHandle).FullName)
            {
                return new NativePeers.SystemRuntimeMethodHandle(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(RuntimeFieldHandle).FullName)
            {
                return new NativePeers.SystemRuntimeFieldHandle(methodDef);
            }

            if (methodDef.DeclaringType.FullName == typeof(string).FullName)
            {
                return new NativePeers.SystemString(methodDef);
            }

            return null;
        }

        protected readonly MethodDef _method;

        public NativePeer(MethodDef method)
        {
            _method = method;
        }

        public abstract bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement);
    }
}
