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
    public class MethodCallNativePeers : IInstructionExecutor
    {
        // dirty hack - load & sort all of the "native" peers using reflection
        private static readonly INativePeerCache<IMethodCallNativePeer> _cache = new ReflectionNativePeerCache<IMethodCallNativePeer>();

        private static readonly OpCode[] _opCodes = new OpCode[] { OpCodes.Call, OpCodes.Callvirt, OpCodes.Calli };

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
            if(_cache.TryGetNativePeer(method, out IMethodCallNativePeer nativePeer) && 
                nativePeer.TryExecute(method, args, cur, out IIEReturnValue returnValue))
            {
                args.Dispose();
                return returnValue;
            }

            // all native peers failed => return the popped arguments on the stack
            ReturnArguemntList(args, cur);

            return next(baseExecutor, cur);
        }

        protected static void ReturnArguemntList(DataElementList args, ExplicitActiveState cur)
        {
            foreach (IDataElement de in args)
            {
                cur.EvalStack.Push(de);
            }
        }

        protected static DataElementList CreateArgumentList(MethodDef method, ExplicitActiveState cur)
        {
            int size = method.ParamDefs.Count + (method.HasThis ? 1 : 0);
            DataElementList retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size >= 0; --size)
                retval[size] = cur.EvalStack.Pop();

            return retval;
        }

        protected static MethodDef GetMethod(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (instruction.Instruction.OpCode == OpCodes.Call ||
                instruction.Instruction.OpCode == OpCodes.Callvirt)
            {
                return (instruction.Operand as IMethod).ResolveMethodDefThrow();
            }
            else if (instruction.Instruction.OpCode == OpCodes.Calli)
            {
                MethodPointer methodPointer = (MethodPointer)cur.EvalStack.Peek(0);
                return methodPointer.Value;
            }

            throw new NotSupportedException();
        }
    }
}
