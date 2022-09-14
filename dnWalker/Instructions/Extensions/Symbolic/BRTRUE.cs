﻿using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

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
    public class BRTRUE : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new OpCode[]
        {
            OpCodes.Brtrue,
            OpCodes.Brtrue_S,
        };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement operandDE = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!operandDE.TryGetExpression(cur, out Expression expression))
            {
                return retValue;
            }

            DecisionHelper.JumpOrNext(cur, retValue, expression.AsBoolean(), Expression.MakeNot(expression.AsBoolean()));

            return retValue;
        }
    }
}
