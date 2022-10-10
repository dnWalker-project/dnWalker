using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.Constructors
{
    [NativePeer(typeof(System.UIntPtr), ".ctor")]
    public class SystemUIntPtr : ConstructorCallNativePeerBase
    {
        public override bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            return PushReturnValue(new Int4(((Int4)args[0]).Value), cur, out returnValue);
        }
    }
}
