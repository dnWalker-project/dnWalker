using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    public interface INativePeer
    {
        bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue);

    }
}
