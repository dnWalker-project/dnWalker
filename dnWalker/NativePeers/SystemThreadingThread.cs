using dnlib.DotNet;
using MMC.Data;
using MMC.ICall;
using MMC.InstructionExec;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.NativePeers
{
    public class SystemThreadingThread : NativePeer
    {
        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            throw new NotImplementedException();
        }

        public override bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
        {
            ThreadHandlers.Thread_internal(methodDef, args, cur);

            //MMC.ICall.IntCallManager.
            //ObjectReference threadObjectRef = cur.DynamicArea.AllocateObject(
            //cur.DynamicArea.DeterminePlacement(false),
            //cur.DefinitionProvider.GetTypeDefinition("System.Threading.Thread"));*

            //cur.ThreadPool.NewThread(null, threadObjectRef);
            //return threadObjectRef;
            //return cur.EvalStack.Pop();
            return true;// base.TryConstruct(methodDef, args, cur);
        }
    }
}
