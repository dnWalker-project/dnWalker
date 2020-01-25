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
    public class MethodBypass
    {
        public static MethodBypass Get(MethodDef meth)
        {
            if (meth.DeclaringType.FullName == typeof(System.Environment).FullName)
            {
                switch (meth.Name)
                {
                    case "GetResourceString":
                        return new MethodBypass(meth);
                    default:
                        throw new NotImplementedException(meth.FullName);
                }
            }

            return null;
        }

        private readonly MethodDef _meth;

        public MethodBypass(MethodDef meth)
        {
            _meth = meth;
        }

        public bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            dataElement = args[0];
            return true;
        }
    }
}
