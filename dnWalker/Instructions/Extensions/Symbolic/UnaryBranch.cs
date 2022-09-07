using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
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
    //public class UnaryBranch : IInstructionExecutor
    //{
    //    private static readonly OpCode[] _supportedOpCodes = new OpCode[]
    //    {
    //        OpCodes.Brfalse,
    //        OpCodes.Brfalse_S,
    //        OpCodes.Brtrue,
    //        OpCodes.Brtrue_S,
    //    };


    //    public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

    //    public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
    //    {
    //        IDataElement operandDE = cur.EvalStack.Peek();

    //        IIEReturnValue retValue = next(baseExecutor, cur);

    //        if (!operandDE.TryGetExpression(cur, out Expression expression))
    //        {
    //            return retValue;
    //        }

    //        DecisionHelper.MakeDecision(cur, retValue, static (_, edge, checksTrue, operand) => (checksTrue, edge) switch
    //        {
    //            (true, JumpEdge) => operand,
    //            (true, NextEdge) => Expression.MakeNot(operand),
    //            (false, JumpEdge) => Expression.MakeNot(operand),
    //            (false, NextEdge) => operand,
    //            _ => throw new InvalidOperationException("Only Next or Jump edge can be used within UnaryBranch executor.")
    //        }, baseExecutor.Instruction.OpCode.Code is Code.Brtrue or Code.Brtrue_S, expression.AsBoolean());

    //        return retValue;
    //    }
    //}
}
