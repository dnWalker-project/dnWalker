using System;
using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemConsole : NativePeer
    {
        private static ObjectReference _outTextWriterRef = ObjectReference.Null;

        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            switch (methodDef.Name)
            {
                case "get_Out":
                    if (_outTextWriterRef.Equals(ObjectReference.Null))
                    {
                        var textWriterType = cur.DefinitionProvider.GetTypeDefinition(typeof(System.IO.TextWriter).FullName);
                        _outTextWriterRef = cur.DynamicArea.AllocateObject(textWriterType);
                        cur.DynamicArea.SetPinnedAllocation(_outTextWriterRef, true);
                    }
                    
                    cur.EvalStack.Push(_outTextWriterRef);
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            iieReturnValue = InstructionExecBase.nextRetval;
            return false;
        }
    }
}
