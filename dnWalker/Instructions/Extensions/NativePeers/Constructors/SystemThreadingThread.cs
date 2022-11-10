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

namespace dnWalker.Instructions.Extensions.NativePeers.Constructors
{
    [NativePeer(typeof(System.Threading.Thread), ".ctor")]
    public class SystemThreadingThread : ConstructorCallNativePeerBase
    {
        public override bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            ThreadHandlers.Thread_internal(method, args, cur);
            return Next(out returnValue);
        }
    }
}
