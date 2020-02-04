using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemDouble : NativePeer
    {
        public SystemDouble(MethodDef method) : base(method)
        {
        }

        internal static NativePeer GetMethodBypass(MethodDef method)
        {
            return new SystemDouble(method);
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            if (_method.FullName == "System.Boolean System.Double::IsNaN(System.Double)")
            {
                dataElement = new Int4(double.IsNaN(((Float8)args[0]).Value) ? 1 : 0);
                return true;
            }

            if (_method.FullName == "System.Boolean System.Double::Equals(System.Double)")
            {
                var left = args[0];
                if (left is IManagedPointer lp)
                {
                    left = lp.Value;
                }

                var right = args[1];
                if (right is IManagedPointer rp)
                {
                    right = rp.Value;
                }

                dataElement = new Int4(left.CompareTo(right) == 0 ? 1 : 0);
                return true;
            }

            dataElement = null;
            return false;
        }
    }
}
