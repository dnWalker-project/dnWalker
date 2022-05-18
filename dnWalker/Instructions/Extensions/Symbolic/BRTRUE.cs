//using dnlib.DotNet.Emit;

//using dnWalker.Symbolic;
//using dnWalker.Symbolic.Expressions;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;

//namespace dnWalker.Instructions.Extensions.Symbolic
//{
//    public class BRTRUE : DecisionMaker
//    {
//        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Brtrue, OpCodes.Brtrue_S };
//        public override IEnumerable<OpCode> SupportedOpCodes
//        {
//            get
//            {
//                return _supportedOpCodes;
//            }
//        }


//        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
//        {
//            IDataElement operandDE = cur.EvalStack.Peek();

//            IIEReturnValue retValue = next(baseExecutor, cur);

//            if (!operandDE.TryGetExpression(cur, out Expression expression))
//            {
//                return retValue;
//            }

//            Instruction nextInstruction = GetNextInstruction(retValue, cur);
//            bool willJump = nextInstruction != null;

//            expression = expression.AsBoolean();
//            Expression not = Expression.MakeNot(expression);

//            //  willJump => expression is     valid => PathConstraint := expression
//            // !willJump => expression is not valid => PathConstriant := Not(expression)
//            int decision = willJump ? 1 : 0;

//            MakeDecision(cur, decision, not, expression);



//            // nextInstruction == null => will NOT branch => the condition is FALSE => we need to negate it for the path constraint
//            //Expression condition = nextInstruction == null ? Expression.Not(expression) : expression;

//            //SetPathConstraint(baseExecutor, nextInstruction, cur, condition);

//            return retValue;
//        }
//    }
//}
