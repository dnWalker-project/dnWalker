using System;
using System.Collections.Generic;
using System.Diagnostics;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.DataElements;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class CALLVIRT : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Callvirt };
        private static IDataElement[] GetArguments(MethodDef method, ExplicitActiveState cur)
        {
            int argCount = method.GetParamCount() + (method.IsStatic ? 0 : 1);
            IDataElement[] args = new IDataElement[argCount];

            for (int i = 0; i < argCount; ++i)
            {
                args[i] = cur.EvalStack.Peek(argCount - i - 1);
            }

            return args;
        }

        private static ObjectReference GetInstance(MethodDef method, ExplicitActiveState cur)
        {
            return method.IsStatic ? ObjectReference.Null : (ObjectReference)cur.EvalStack.Peek(method.GetParamCount());
        }

        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            // 1st, we check whether the operand (method) is one we can substitute
            CallInstructionExec callModel = (CallInstructionExec)baseExecutor;
            MethodDef method = callModel.Method;
            IDataElement[] args = GetArguments(method, cur);

            // if there is symbolic value for this method execution - fake it 
            if (TryFakeMethod(cur, method, args, forceInitialization: false))
            {
                return InstructionExecBase.nextRetval;
            }

            IIEReturnValue returnValue = next(baseExecutor, cur);

            if (returnValue == InstructionExecBase.ehLookupRetval &&
                cur.CurrentThread.UnhandledException.Type.FullName == "System.MissingMethodException")
            {
                if (TryFakeMethod(cur, method, args, forceInitialization: true))
                {
                    InstructionExecBase.UnthrowException(cur);
                    returnValue = InstructionExecBase.nextRetval;
                }
            }

            if (!method.IsStatic &&
                ExpressionUtils.GetExpressions(cur, args[0], out Expression instanceExpression))
            {
                DecisionHelper.ThrowNullOrNext(cur, returnValue, instanceExpression);

            }

            return returnValue;
        }

        private static bool TryFakeMethod(ExplicitActiveState cur, MethodDef method, IDataElement[] args, bool forceInitialization = false)
        {
            if (cur.TryGetSymbolicContext(out SymbolicContext context))
            {
                ObjectReference instance = (ObjectReference)args[0];

                if (instance.TryGetExpression(cur, out Expression instanceExpr))
                {
                    Debug.Assert(instanceExpr is VariableExpression);
                    dnWalker.Symbolic.IVariable instanceVar = ((VariableExpression)instanceExpr).Variable;

                    if (context.InputModel.TryGetValue(instanceVar, out IValue instanceValue, forceInitialization))
                    {
                        Location symbolicLocation = (Location)instanceValue;
                        int invocation = context.GetInvocation(symbolicLocation, method);

                        dnWalker.Symbolic.IVariable resultVar = Variable.MethodResult(instanceVar, method, invocation);

                        IDataElement result = context.LazyInitialize(resultVar, cur);
                        result.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(resultVar));
                        cur.EvalStack.Push(result);
                        
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

