using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer("System.Console", MatchMethods = true)]
    public class SystemConsole : CompiledMethodCallNativePeer<SystemConsole>
    {
        private const string ConsoleOutRef = "console-out-ref";
        public static bool TryGetConsoleOut(ExplicitActiveState cur, out ObjectReference consoleOut)
        {
            return cur.PathStore.CurrentPath.TryGetPathAttribute(ConsoleOutRef, out consoleOut);
        }

        public static ObjectReference GetConsoleOut(ExplicitActiveState cur)
        {
            if (!TryGetConsoleOut(cur, out var textOutRef))
            {
                var textWriterType = cur.DefinitionProvider.GetTypeDefinition(typeof(TextWriter).FullName);
                textOutRef = cur.DynamicArea.AllocateObject(textWriterType);
                cur.DynamicArea.SetPinnedAllocation(textOutRef, true);

                cur.PathStore.CurrentPath.SetPathAttribute(ConsoleOutRef, textOutRef);
            }

            return textOutRef;
        }

#pragma warning disable IDE1006 // Naming Styles - method name must match native method names

        private static bool get_Out(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            return PushReturnValue(GetConsoleOut(cur), cur, out returnValue);
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
