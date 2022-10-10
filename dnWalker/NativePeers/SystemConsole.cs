//using System.IO;
//using dnlib.DotNet;
//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//namespace dnWalker.NativePeers
//{
//    public class SystemConsole : NativePeer
//    {
//        private const string ConsoleOutRef = "console-out-ref";

//        public static ObjectReference GetConsoleOut(ExplicitActiveState cur)
//        {
//            if (!TryGetConsoleOut(cur, out ObjectReference textOutRef))
//            {
//                TypeDef textWriterType = cur.DefinitionProvider.GetTypeDefinition(typeof(TextWriter).FullName);
//                textOutRef = cur.DynamicArea.AllocateObject(textWriterType);
//                cur.DynamicArea.SetPinnedAllocation(textOutRef, true);

//                cur.PathStore.CurrentPath.SetPathAttribute(ConsoleOutRef, textOutRef);
//            }

//            return textOutRef;
//        }

//        public static bool TryGetConsoleOut(ExplicitActiveState cur, out ObjectReference consoleOut)
//        {
//            return cur.PathStore.CurrentPath.TryGetPathAttribute(ConsoleOutRef, out consoleOut);
//        }

//        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
//        {
//            var name = methodDef.Name.Replace("get_", "");
            
//            switch (name)
//            {
//                case "Out":
//                    cur.EvalStack.Push(GetConsoleOut(cur));
//                    iieReturnValue = InstructionExecBase.nextRetval;
//                    return true;
//            }

//            iieReturnValue = InstructionExecBase.nextRetval;
//            return false;
//        }

//    }
//}
