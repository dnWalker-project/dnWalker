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
    public static partial class CALLVIRT
    {
        


        public class MethodResultInjector : IInstructionExecutor
        {

            private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Callvirt };

            public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

            public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                // 1st, we check whether the operand (method) is one we can substitute
                CallInstructionExec callModel = (CallInstructionExec)baseExecutor;
                MethodDef method = callModel.Method;
                IDataElement[] args = GetArguments(method, cur);

                IIEReturnValue returnValue = next(baseExecutor, cur);

                // TODO: check for user configuration - whether this method should be faked or not
                if (returnValue == InstructionExecBase.ehLookupRetval ||
                    (/* !FakeMethod(method) && */ cur.CallStack.Peek() != null && !method.IsStatic))
                {
                    // an exception during the execution, e.i. NullReference OR
                    // we do not care about this method => do not inject any method result
                    return returnValue;
                }

                // we will fake this method...

                if (cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    ObjectReference instance = (ObjectReference)args[0];

                    if (instance.TryGetExpression(cur, out Expression instanceExor))
                    {
                        Debug.Assert(instanceExor is VariableExpression);
                        dnWalker.Symbolic.IVariable instanceVar = ((VariableExpression)instanceExor).Variable;
                        if (context.InputModel.TryGetValue(instanceVar, out IValue instanceValue))
                        {
                            Location symbolicLocation = (Location)instanceValue;
                            int invocation = context.GetInvocation(symbolicLocation, method);

                            dnWalker.Symbolic.IVariable resultVar = Variable.MethodResult(instanceVar, method, invocation);

                            IDataElement result = context.LazyInitialize(resultVar, cur);
                            result.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(resultVar));
                            cur.EvalStack.Push(result);

                        }
                    }
                }

                return returnValue;
            }
        }
    }
}
