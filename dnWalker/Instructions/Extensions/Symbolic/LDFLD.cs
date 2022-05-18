using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class LDFLD : Load
    {
        private readonly OpCode[] _supportedCodes = new OpCode[]
        {
            OpCodes.Ldfld
        };

        public override IEnumerable<OpCode> SupportedOpCodes => _supportedCodes;

        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            if (cur.TryGetSymbolicContext(out SymbolicContext context))
            {
                ObjectReference instanceDE = (ObjectReference)cur.EvalStack.Peek();

                if (instanceDE.TryGetExpression(cur, out Expression instanceExpression))
                {
                    Debug.Assert(instanceExpression is VariableExpression);

                    context.EnsureInitialized(Variable.InstanceField(((VariableExpression)instanceExpression).Variable, (IField)baseExecutor.Operand), cur);
                }
            }

            return next(baseExecutor, cur);
        }
    }
}
