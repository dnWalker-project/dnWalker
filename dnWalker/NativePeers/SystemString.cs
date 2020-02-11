using dnlib.DotNet;
using MMC.Data;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemString : NativePeer
    {
        public SystemString(MethodDef method) : base(method)
        {
        }

        public override bool TryGetValue(DataElementList args, ExplicitActiveState cur, out IDataElement dataElement)
        {
            if (_method.FullName == "System.Boolean System.String::op_Equality(System.String,System.String)")
            {
                dataElement = new Int4(args[0].CompareTo(args[1]) == 0 ? 1 : 0);
                return true;
            }

            if (_method.FullName == "System.String System.String::Trim()")
            {
                var s = (ConstantString)args[0];
                dataElement = new ConstantString(s.Value.Trim());
                return true;
            }

            if (_method.FullName == "System.Int32 System.String::get_Length()")
            {
                var s = (ConstantString)args[0];
                dataElement = new Int4(s.Value.Length);
                return true;
            }

            dataElement = null;
            return false;
        }
    }
}
