using System.IO;
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
            var name = methodDef.Name.Replace("get_", "");
            
            switch (name)
            {
                case "Out":
                    if (_outTextWriterRef.Equals(ObjectReference.Null))
                    {
                        var textWriterType = cur.DefinitionProvider.GetTypeDefinition(typeof(TextWriter).FullName);
                        _outTextWriterRef = cur.DynamicArea.AllocateObject(textWriterType);
                        cur.DynamicArea.SetPinnedAllocation(_outTextWriterRef, true);

                        var textWriterAlloc = cur.DynamicArea.Allocations[_outTextWriterRef];
                        var stringWriter = new StringWriter();
                        // "static" attribute
                        cur.SetObjectAttribute<TextWriter>(null, methodDef.DeclaringType.FullName + "." + name, stringWriter);
                        cur.SetObjectAttribute<TextWriter>(textWriterAlloc, "TextWriter", stringWriter);
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
