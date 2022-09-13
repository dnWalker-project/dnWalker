using dnlib.DotNet;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    internal interface IConstructorCallNativePeer
    {
        bool TryExecute(MethodDef method, ExplicitActiveState cur, out IIEReturnValue returnValue);
    }
}
