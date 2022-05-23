using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
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

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class LDFLD : DecisionMaker
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

                if (ExpressionUtils.GetExpressions(cur, instanceDE, out Expression instanceExpression))
                {
                    Debug.Assert(instanceExpression is VariableExpression);

                    context.EnsureInitialized(Variable.InstanceField(((VariableExpression)instanceExpression).Variable, (IField)baseExecutor.Operand), cur);

                    IIEReturnValue retValue = next(baseExecutor, cur);

                    MakeDecision(cur, retValue, (cur, edge, instance) =>
                    {
                        ExpressionFactory ef = cur.GetExpressionFactory();
                        IVariable arrayVariable = ((VariableExpression)instance).Variable;
                        return edge switch
                        {
                            NextEdge => Expression.MakeNotEqual(instance, ef.NullExpression),
                            ExceptionEdge { ExceptionType.FullName: "System.NullReferenceException" } => Expression.MakeEqual(instance, ef.NullExpression),
                            _ => throw new NotSupportedException("Only next edge and exception edge with NullReference are supported by LDFLD.")
                        };
                    }, instanceExpression);

                    return retValue;
                }
            }

            return next(baseExecutor, cur);
        }
    }
}
