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

namespace dnWalker.Instructions.Extensions.NativePeers.Symbolic
{
    public class SymbolicMethodHandlers : IInstructionExecutor
    {
        private static readonly INativePeerCache<ISymbolicNativePeer> _cache = new ReflectionNativePeerCache<ISymbolicNativePeer>();

        private static readonly OpCode[] _opCodes = new OpCode[] { OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt };

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

            DataElementList args = CreateArgumentList(method, cur);

            IIEReturnValue returnValue = next(baseExecutor, cur);

            if (_cache.TryGetNativePeer(method, out ISymbolicNativePeer peer))
            {
                peer.Handle(method, args, cur, returnValue);
            }

            return returnValue;
        }

        protected static DataElementList CreateArgumentList(MethodDef method, ExplicitActiveState cur)
        {
            var size = method.ParamDefs.Count + (method.HasThis ? 1 : 0);
            var retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (var i = 0; i < size; ++i)
                retval[size - i - 1] = cur.EvalStack.Peek(i);

            return retval;
        }

        protected static MethodDef GetMethod(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            return (instruction.Operand as IMethod).ResolveMethodDefThrow();
        }
    }
}
