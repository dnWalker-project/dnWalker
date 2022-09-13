using dnlib.DotNet;
using dnlib.DotNet.Emit;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    public class ConstructorNativePeers : IInstructionExecutor
    {
        // dirty hack - load & sort all of the "native" peers using reflection
        private static readonly INativePeerCache<IConstructorCallNativePeer> _cache = new ReflectionNativePeerCache<IConstructorCallNativePeer>();

        private static OpCode[] _opCodes = new OpCode[] { OpCodes.Newobj };
        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _opCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            MethodDef method = GetMethod(baseExecutor, cur);
            if (_cache.TryGetNativePeer(method, out IConstructorCallNativePeer nativePeer) &&
                nativePeer.TryExecute(method, cur, out IIEReturnValue returnValue))
            {
                return returnValue;
            }
            return next(baseExecutor, cur); ;
        }


        protected static MethodDef GetMethod(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            return (instruction.Operand as IMethod).ResolveMethodDefThrow();
        }
    }
}
