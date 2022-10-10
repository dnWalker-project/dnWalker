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
    [NativePeer(typeof(System.Random), ".ctor")]
    public class SystemRandom : ConstructorCallNativePeerBase
    {
        public override bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var objectRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(false),
                method.DeclaringType);

            return PushReturnValue(objectRef, cur, out returnValue);
        }
    }
}
