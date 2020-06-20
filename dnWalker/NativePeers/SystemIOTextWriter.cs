using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.IO;
using System.Text;

namespace dnWalker.NativePeers
{
    public class SystemIOTextWriter : NativePeer
    {
        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            var textWriterAlloc = cur.DynamicArea.Allocations[(ObjectReference)args[0]];

            if (!cur.TryGetObjectAttribute<TextWriter>(textWriterAlloc, out var tw))
            {
                tw = cur.SetObjectAttribute<TextWriter>(textWriterAlloc, new StringWriter());
            }
            
            switch (methodDef.Name)
            {
                case "WriteLine":
                    tw.WriteLine("X");
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            throw new NotImplementedException(methodDef.FullName);
        }
    }
}
