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
                return NativePeers.SystemDouble.GetMethodBypass(meth);
            }

            return null;
        }

        protected readonly MethodDef _method;

        public NativePeer(MethodDef meth)
        {
            _method = meth;
        }

        public virtual bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            dataElement = args[0];
            return true;
        }
    }
}
