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
    public class SystemThreadingMonitor : CompiledMethodCallNativePeer<SystemThreadingMonitor>
    {
        private static bool Enter(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters.Count != 2)
            {
                returnValue = null;
                return false;
            }

            var lockTaken = args[1];
            var ptr = lockTaken as IManagedPointer;
            if (ptr != null)
            {
                lockTaken = ptr.Value;
            }

            if (lockTaken.ToBool())
            {
                var msg = SystemEnvironment.GetResourceString("Argument_MustBeFalse");
                InstructionExecBase.ThrowException(new ArgumentException(msg, "lockTaken"), cur);
                returnValue = InstructionExecBase.ehLookupRetval;
                return true;
            }
            var dataElement = MonitorHandlers.Enter(args, cur);
            if (ptr != null)
            {
                ptr.Value = dataElement;
            }

            returnValue = InstructionExecBase.nextRetval;
            return true;
        }

        private static bool Exit(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            MonitorHandlers.Exit(args, cur);

            returnValue = InstructionExecBase.nextRetval;
            return true;
        }
    }
}
