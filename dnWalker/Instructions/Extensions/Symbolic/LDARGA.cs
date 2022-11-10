using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class LDARGA : IInstructionExecutor
    {
        private readonly OpCode[] _supportedCodes = new OpCode[]
        {
            OpCodes.Ldarga,
            OpCodes.Ldarga_S
        };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedCodes;

        private static Parameter GetParameter(InstructionExecBase baseExecutor, MethodDef method)
        {
            object operand = baseExecutor.Operand;
            if (baseExecutor.HasImplicitOperand)
            {
                int index = ((Int4)operand).Value;
                return method.Parameters[index];
            }
            else
            {
                return (Parameter)operand;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            if (cur.TryGetSymbolicContext(out SymbolicContext context))
            {
                MethodDef method = cur.CurrentMethod.Definition;
                Parameter parameter = GetParameter(baseExecutor, method);

                context.EnsureInitialized(Variable.MethodArgument(parameter), cur);
            }

            return next(baseExecutor, cur);
        }
    }
}
