using dnlib.DotNet;
using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.Linq;

namespace dnWalker.NativePeers
{
    public class SystemIOTextWriter : NativePeer
    {
        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            var objectReference = (ObjectReference)args[0];
            var textWriterAlloc = cur.DynamicArea.Allocations[(ObjectReference)args[0]] as AllocatedObject;
            /*
            if (!cur.TryGetObjectAttribute<TextWriter>(textWriterAlloc, "TextWriter", out var tw))
            {
                throw new Exception();
            }*/

            switch (methodDef.Name)
            {
                case "WriteLine" when methodDef.Parameters.Count == 2:
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
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            throw new NotImplementedException(methodDef.FullName);
        }
    }

    internal class SystemIOTextWriterImpl
    {
        private string m_value;
    }
}
