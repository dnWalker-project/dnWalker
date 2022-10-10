using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    public abstract class CompiledMethodCallNativePeer : MethodCallNativePeerBase, IMethodCallNativePeer
    {
        protected delegate bool MethodHandler(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue);

        private static readonly ConcurrentDictionary<Type, MethodHandler> _compiledHandlersCache;

        static CompiledMethodCallNativePeer()
        {
            _compiledHandlersCache = new ConcurrentDictionary<Type, MethodHandler>();
        }


        protected static MethodHandler CompileHandler<T>()
        {
            var type = typeof(T);
            MethodHandler handler = _compiledHandlersCache.GetOrAdd(type, t => CompileHandlerCore(t));

            return handler;
        }

        private static MethodHandler CompileHandlerCore(Type type)
        {
            // resulting method is something like this
            // bool (MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
            // {
            //      switch (method.Name)
            //      {
            //          case "handler1": return <type>.handler1(method, args, cur, out returnValue);
            //          case "handler2": return <type>.handler2(method, args, cur, out returnValue);
            //          ...
            //      }
            //      returnValue = null;
            //      return false;
            // }


            // gets all method which match MethodHandler signature
            var handlers = ReflectionHelpers.GetHandlerMethods(type).ToArray();

            // initialize the arguments
            var methodExpr = Expression.Parameter(typeof(MethodDef), "method");
            var argsExpr = Expression.Parameter(typeof(DataElementList), "args");
            var stateExpr = Expression.Parameter(typeof(ExplicitActiveState), "cur");
            var resultExpr = Expression.Parameter(typeof(IIEReturnValue).MakeByRefType(), "result");

            // method.Name expression
            Expression methodName = Expression.Property(Expression.Property(methodExpr, nameof(MethodDef.Name)), nameof(UTF8String.String));

            // setup main return label to which the execution jumps if handler was matched
            var ret = Expression.Label(typeof(bool));

            // for each discovered handler method, generate SwitchCase
            // - block - invoke the discovered handler & return the result
            // - test value - just one - the name of the discovered handler
            var invokeHandlers = handlers
                .Select(m => Expression.SwitchCase(Expression.Return(ret, Expression.Call(null, m, methodExpr, argsExpr, stateExpr, resultExpr)), Expression.Constant(m.Name)))
                .ToArray();

            // default case if no handler is matched, the returnValue = null and return false
            Expression retFail = Expression.Block
                (
                    Expression.Assign(resultExpr, Expression.Constant(null, typeof(IIEReturnValue))),
                    Expression.Return(ret, Expression.Constant(false))
                );

            // build the switch
            Expression mainSwitch = Expression.Switch(methodName, retFail, invokeHandlers);

            // build the method body
            Expression body = Expression.Block(typeof(bool), mainSwitch, Expression.Label(ret, Expression.Constant(false)));

            // compile the handler
            var handler = Expression.Lambda<MethodHandler>(body, methodExpr, argsExpr, stateExpr, resultExpr).Compile();
            return handler;
        }
    }


    /// <summary>
    /// Compiles handler methods - all static methods which have signature bool(MethodDef, ExplicitActiveState, out IIEReturnValue)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CompiledMethodCallNativePeer<T> : CompiledMethodCallNativePeer, IMethodCallNativePeer
    {
        private static readonly MethodHandler _handler;

        static CompiledMethodCallNativePeer()
        {
            _handler = CompileHandler<T>();
        }

        public sealed override bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            return _handler.Invoke(method, args, cur, out returnValue);
        }
    }
}
