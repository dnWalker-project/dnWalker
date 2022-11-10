using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.Symbolic
{
    public interface ISymbolicNativePeer
    {
        void Handle(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue);
    }
}
