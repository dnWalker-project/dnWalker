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

            if (!cur.TryGetObjectAttribute<TextWriter>(textWriterAlloc, "TextWriter", out var tw))
            {
                throw new Exception();
            }
            
            switch (methodDef.Name)
            {
                case "WriteLine" when methodDef.Parameters.Count == 2:
                    tw.WriteLine(((IConvertible)args[1]).ToString(System.Globalization.CultureInfo.CurrentCulture));
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            throw new NotImplementedException(methodDef.FullName);
        }
    }
}
