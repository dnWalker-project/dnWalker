using System;
using dnlib.DotNet;
using MMC.Data;
using MMC.ICall;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.NativePeers
{
    public class SystemThreadingMonitor : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            switch (method.FullName)
            {
                case "System.Void System.Threading.Monitor::Enter(System.Object,System.Boolean&)":
                    // if (lockTaken) { ThrowLockTakenException(); }
                    // ReliableEnter(obj, ref lockTaken);
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
                        iieReturnValue = InstructionExecBase.ehLookupRetval;
                        return true;
                    }
                    var dataElement = MonitorHandlers.Enter(args, cur);
                    if (ptr != null)
                    {
                        ptr.Value = dataElement;
                    }

                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
                case "System.Void System.Threading.Monitor::Exit(System.Object)":
                    MonitorHandlers.Exit(args, cur);
                    iieReturnValue = InstructionExecBase.nextRetval;
                    return true;
            }

            throw new NotImplementedException(method.FullName);
        }
    }
}
