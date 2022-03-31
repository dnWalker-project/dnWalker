using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.TypeSystem;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract class SymbolicNativePeer : IInstructionExecutor
    {
        protected interface IMethodHandler
        {
            public void HandleException(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur);
            public void HandleResult(Expression[] arguments, InstructionExecBase exec, ExplicitActiveState cur);
        }


        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get { return _supportedOpCodes; }
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

        private IReadOnlyDictionary<string, IMethodHandler> _handlers;

        protected abstract ITypeDefOrRef GetPeerType(IDefinitionProvider definitionProvider);
        protected void SetHandlers(IReadOnlyDictionary<string, IMethodHandler> handlers)
        {
            _handlers = handlers;
        }

        public virtual IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            MethodDef method = GetMethod(baseExecutor, cur);

            if (method.DeclaringType != GetPeerType(cur.DefinitionProvider))
            {
                // not the peer type
                return next(baseExecutor, cur);
            }

            if (!_handlers.TryGetValue(method.Name, out IMethodHandler handler))
            {
                // not handled method
                return next(baseExecutor, cur);
            }

            IDataElement[] args = new IDataElement[method.GetParamCount()];
            for (int i = 0; i < args.Length; ++i)
            {
                args[args.Length - i - 1] = cur.EvalStack.Peek(i);
            }

            IIEReturnValue returnValue = next(baseExecutor, cur);

            Expression[] expressions = new Expression[args.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                args[i].TryGetExpression(cur, out expressions[i]);
            }

            bool isSymbolic = expressions.Any(static e => e != null);

            if (!isSymbolic)
            {
                // no argument is symbolic => we do not create any expression
                return returnValue;
            }

            for (int i = 0; i < expressions.Length; ++i)
            {
                expressions[i] ??= args[i].AsExpression();
            }

            if (returnValue is ExceptionHandlerLookupReturnValue)
            {
                // an exception was thrown => we need to update the path constraint
                // TODO later when a constraint tree is integrated
                handler.HandleException(expressions, baseExecutor, cur);
            }
            else
            {
                handler.HandleResult(expressions, baseExecutor, cur);
            }

            return returnValue;
        }
    }
}
