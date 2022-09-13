using dnlib.DotNet;

using MMC;
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
    [NativePeer("System.IO.TextWriter", MatchMethods = true)]
    public class SystemIOTextWriter : CompiledMethodCallNativePeer<SystemIOTextWriter>
    {
        private static bool WriteLine(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var objectReference = (ObjectReference)args[0];

            if (method.Parameters.Count == 2)
            {
                var theObject = cur.DynamicArea.Allocations[objectReference] as AllocatedObject;
                var offset = 0;// GetFieldOffset(theObject.Type);
                var eol = new ConstantString(Environment.NewLine);
                var values = new[] { args[1], eol };
                if (!theObject.Fields[offset].Equals(ObjectReference.Null))
                {
                    values = new[] { theObject.Fields[offset], args[1], eol };
                }

                var val = new ConstantString(
                    string.Join(
                        string.Empty,
                        values.Select(v => ((IConvertible)v).ToString(System.Globalization.CultureInfo.CurrentCulture))));
                cur.ParentWatcher.RemoveParentFromChild(objectReference, theObject.Fields[offset], cur.Configuration.MemoisedGC());
                ObjectEscapePOR.UpdateReachability(theObject.ThreadShared, theObject.Fields[offset], val, cur);
                theObject.Fields[offset] = val;
                cur.ParentWatcher.AddParentToChild(objectReference, val, cur.Configuration.MemoisedGC());
                returnValue = InstructionExecBase.nextRetval;
                return true;
            }
            returnValue = null;
            return false;
        }
    }
}
