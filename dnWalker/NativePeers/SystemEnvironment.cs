using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemEnvironment : NativePeer
    {
        public SystemEnvironment(MethodDef method) : base(method)
        {
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            switch (_method.Name)
            {
                case "GetResourceString":
                    dataElement = args[0];
                    return true;
            }

            dataElement = null;
            return false;
        }
    }
}
