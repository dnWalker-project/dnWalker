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

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer("System.Threading.Thread")]
    public class SystemThreadingThread : CompiledMethodCallNativePeer<SystemThreadingThread>
    {
        public static bool Start(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            Int32 threadId = cur.ThreadPool.FindOwningThread(args[0]);
            if (threadId == LockManager.NoThread)
            {
                throw new NotSupportedException("Owning thread not found.");
            }
            cur.ThreadPool.Threads[threadId].State = System.Threading.ThreadState.Running;
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        // external method in dotnet framework... filtered out in CallInstructionExec.FilterCall
        public static bool StartupSetApartmentStateInternal(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        public static bool GetCurrentThreadNative(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            ThreadHandlers.CurrentThread_internal(method, args, cur);
            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
