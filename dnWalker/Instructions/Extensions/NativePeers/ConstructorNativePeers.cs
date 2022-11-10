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

            DataElementList args = CreateArgumentList(method, cur);
            if (_cache.TryGetNativePeer(method, out IConstructorCallNativePeer nativePeer) &&
                nativePeer.TryExecute(method, args, cur, out IIEReturnValue returnValue))
            {
                args.Dispose();
                return returnValue;
            }

            // all native peers failed => return the popped arguments on the stack
            ReturnArguemntList(args, cur);

            return next(baseExecutor, cur); ;
        }


        protected static MethodDef GetMethod(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            return (instruction.Operand as IMethod).ResolveMethodDefThrow();
        }

        protected static DataElementList CreateArgumentList(MethodDef method, ExplicitActiveState cur)
        {
            int size = method.ParamDefs.Count + (method.HasThis ? 1 : 0);
            DataElementList retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size > 0; --size)
                retval[size] = cur.EvalStack.Pop();

            return retval;
        }

        protected static void ReturnArguemntList(DataElementList args, ExplicitActiveState cur)
        {
            for (int i = 1; i < args.Length; ++i)
            {
                cur.EvalStack.Push(args[i]);
            }
        }
    }
}
