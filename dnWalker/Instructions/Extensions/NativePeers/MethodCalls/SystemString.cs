using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer("System.String", MatchMethods = true)]
    public class SystemStringNativePeer : CompiledMethodCallNativePeer<SystemStringNativePeer>
    {
#pragma warning disable IDE1006 // Naming Styles - method name must match native method names

        private static bool op_Equality(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var lhs = args[0];
            var rhs = args[1];

            return PushReturnValue(new Int4(lhs.CompareTo(rhs) == 0 ? 1 : 0), cur, out returnValue);

        }
#pragma warning restore IDE1006 // Naming Styles

        private static bool Format(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters.Count != 2)
            {
                returnValue = null;
                return false;
            }

            var format = (ConstantString)args[0];
            var allocatedObject = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)args[1]];
            //cur.DynamicArea.Allocations[args[1]];
            var value = allocatedObject.Fields[allocatedObject.ValueFieldOffset];
            var arg = ((IConvertible)value).ToString(System.Globalization.CultureInfo.CurrentCulture);
            return PushReturnValue(new ConstantString(string.Format(format.Value, arg)), cur, out returnValue);
        }

        private static bool Trim(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters.Count != 1)
            {
                returnValue = null;
                return false;
            }

            var s = (ConstantString)args[0];
            return PushReturnValue(new ConstantString(s.Value.Trim()),cur, out returnValue);
        }

        private static bool get_Length(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            return PushReturnValue(new Int4(((ConstantString)args[0]).Value.Length), cur, out returnValue);
        }

        private static bool Concat(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters.Any(p => !p.Type.IsString()))
            {
                returnValue = null;
                return false;
            }

            // if all parameters are strings, concat them
            StringBuilder sb = new StringBuilder();
            foreach (IDataElement arg in args)
            {
                if (arg is ConstantString cs)
                {
                    sb.Append(cs.Value);
                    continue;
                }
                sb.Append(arg.ToString());
            }

            return PushReturnValue(new ConstantString(sb.ToString()), cur, out returnValue);
        }

        private static bool IsNullOrEmptry(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            ConstantString arg = (ConstantString)args[0];
            return PushReturnValue(new Int4(string.IsNullOrEmpty(arg.Value) ? 1 : 0), cur, out returnValue);
        }
    }
}
