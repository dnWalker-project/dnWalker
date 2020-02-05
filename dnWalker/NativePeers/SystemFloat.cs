using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemSingle : NativePeer
    {
        public SystemSingle(MethodDef method) : base(method)
        {
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            if (_method.FullName == "System.Boolean System.Single::IsNaN(System.Single)")
            {
                dataElement = new Int4(double.IsNaN(((Float4)args[0]).Value) ? 1 : 0);
                return true;
            }

            if (_method.FullName == "System.Boolean System.Single::Equals(System.Single)")
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
