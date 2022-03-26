using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

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
    public partial class SystemMath : NativePeer
    {
        private static readonly Type[] _oneDouble = new Type[] {typeof(double)};
        private static readonly Type[] _twoDoubles = new Type[] {typeof(double), typeof(double) };

        private static readonly Dictionary<string, IMethodHandler> _handlers = new Dictionary<string, IMethodHandler>()
        {
            ["Sqrt"] = new Sqrt(),
        };


        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            MethodDef method = GetMethod(baseExecutor, cur);

            if (method.DeclaringType != cur.DefinitionProvider.GetTypeDefinition("System.Math"))
            {
                return next(baseExecutor, cur);
            }

            // a system.math method
            if (!_handlers.TryGetValue(method.Name, out var handler))
            {
                // no method handler is available for this method
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

            bool isSymbolic = expressions.All(static e => e != null);

            if (!isSymbolic)
            {
                // no argument is symbolic => we do not create any expression
                return returnValue;
            }

            if (returnValue is ExceptionHandlerLookupReturnValue)
            {
                // an exception was thrown => we need to update the path constraint
                // TODO later when a constraint tree is integrated
                handler.HandleException(expressions, baseExecutor, cur);
                return returnValue;
            }
            else
            {
                handler.HandleResult(expressions, baseExecutor, cur);
            }

            return returnValue;
        }

    }
}
