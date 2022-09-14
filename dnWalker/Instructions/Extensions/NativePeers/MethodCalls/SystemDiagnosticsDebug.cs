using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    // TODO: switch implementation based on running platform???
    [NativePeer("System.Diagnostics.Debug")]
    public class SystemDiagnosticsDebug : CompiledMethodCallNativePeer<SystemDiagnosticsDebug>
    {
        private static bool Assert(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            // implementation ported from CallInstructionExec, should cover more overloads...
            bool violated = !args[0].ToBool();

            if (violated)
            {
                if (args.Length > 1)
                {
                    cur.Logger.Warning("short message: {0}", args[1].ToString());
                }
                if (args.Length > 2)
                {
                    cur.Logger.Warning("long message: {0}", args[2].ToString());
                }
                returnValue = InstructionExecBase.assertionViolatedRetval;
            }
            else
            {
                cur.Logger.Debug("assertion passed.");
                returnValue = InstructionExecBase.nextRetval;
            }

            return true;
        }
    }
}
