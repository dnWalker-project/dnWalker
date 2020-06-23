using dnlib.DotNet;
using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.IO;
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
                    AllocatedObject theObject = cur.DynamicArea.Allocations[objectReference] as AllocatedObject;
                    int offset = 0;// GetFieldOffset(theObject.Type);
                    var values = new[] { args[1] };
                    if (!theObject.Fields[offset].Equals(ObjectReference.Null))
                    {
                        values = new[] { theObject.Fields[offset], args[1] };
                    }

                    var val = new ConstantString(
                        string.Join(
                            Environment.NewLine, 
                            values.Select(v => ((IConvertible)v).ToString(System.Globalization.CultureInfo.CurrentCulture))));
                    cur.ParentWatcher.RemoveParentFromChild(objectReference, theObject.Fields[offset], cur.Configuration.MemoisedGC);
                    ObjectEscapePOR.UpdateReachability(theObject.ThreadShared, theObject.Fields[offset], val, cur);
                    theObject.Fields[offset] = val;
                    cur.ParentWatcher.AddParentToChild(objectReference, val, cur.Configuration.MemoisedGC);
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            throw new NotImplementedException(methodDef.FullName);
        }
    }

    internal class SystemIOTextWriterImpl
    {
        private string _buffer;
    }
}
