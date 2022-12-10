using dnlib.DotNet;
using dnlib.DotNet.MD;

using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Utils;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public partial class SymbolicContext
    {
        private class ConstraintedMethodFaker : IMethodFaker
        {
            private readonly Expression[] _conditions;

            private readonly Location _symbolicLocation;
            private readonly IVariable _instanceVariable;
            private readonly IMethod _method;

            public ConstraintedMethodFaker(IMethod method, Location symbolicLocation, IVariable instanceVariable, IEnumerable<Expression> conditions)
            {
                _method = method ?? throw new ArgumentNullException(nameof(method));
                _symbolicLocation = symbolicLocation;
                _instanceVariable = instanceVariable ?? throw new ArgumentNullException(nameof(instanceVariable));
                _conditions = conditions?.ToArray() ?? throw new ArgumentNullException(nameof(conditions));
            }

            public Location SymbolicLocation
            {
                get
                {
                    return _symbolicLocation;
                }
            }

            public IVariable InstanceVariable
            {
                get
                {
                    return _instanceVariable;
                }
            }

            public IMethod Method
            {
                get
                {
                    return _method;
                }
            }

            public IDataElement GetConcreteValue(ExplicitActiveState cur, IDataElement[] args)
            {
                if (!cur.TryGetSymbolicContext(out SymbolicContext ctx)) throw new InvalidOperationException("The symbolic context is not initialized!!!");

                ISolver solver = cur.Services.GetService<ISolver>();

                TypeSig returnType = _method.MethodSig.RetType;

                Expression[] argExpressions = new Expression[args.Length];
                for (int i = 0; i < argExpressions.Length; ++i)
                {
                    argExpressions[i] = args[i].AsExpression(cur);
                }

                Dictionary<IVariable, Expression> substition = new Dictionary<IVariable, Expression>
                (
                    _method
                    .ResolveMethodDefThrow()
                    .Parameters.Where(p => !p.IsReturnTypeParameter)
                    .Select(p => KeyValuePair.Create((IVariable)new NamedVariable(p.Type, p.Name), argExpressions[p.Index]))
                );

                Constraint currentConstraint = cur.Services.GetService<ConstraintTreeExplorer>().Current.GetPrecondition();

                foreach (Expression condition in _conditions) 
                {
                    Expression substituted = VariableSubstitutor.Substitute(condition, substition);
                    Constraint selectTheCondition = currentConstraint.Clone();
                    selectTheCondition.AddExpressionConstraint(substituted);

                    if (solver.Solve(selectTheCondition) != null) 
                    {
                        return ctx.LazyInitialize(new ConditionalMethodResultVariable(_instanceVariable, _method, condition), cur);
                    }
                }

                return DataElement.GetNullValue(returnType);
            }
        }
        private class InvocationBasedMethodFaker : IMethodFaker
        {
            private int _invocationCounter = 1;

            private readonly Location _symbolicLocation;
            private readonly IVariable _instanceVariable;
            private readonly IMethod _method;

            public InvocationBasedMethodFaker(IMethod method, Location symbolicLocation, IVariable instanceVariable)
            {
                _method = method;
                _symbolicLocation = symbolicLocation;
                _instanceVariable = instanceVariable;
            }

            public IMethod Method
            {
                get
                {
                    return _method;
                }
            }

            public Location SymbolicLocation
            {
                get
                {
                    return _symbolicLocation;
                }
            }

            public IVariable InstanceVariable
            {
                get
                {
                    return _instanceVariable;
                }
            }

            public IDataElement GetConcreteValue(ExplicitActiveState cur, IDataElement[] args)
            {
                if (!cur.TryGetSymbolicContext(out SymbolicContext ctx)) throw new InvalidOperationException("The symbolic context is not initialized!!!");

                int invocation = _invocationCounter++;
                IVariable variable = new MethodResultVariable(_instanceVariable, _method, invocation);
                IDataElement result = ctx.LazyInitialize(variable, cur);
                result.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(variable));
                return result;
            }
        }

        private readonly Dictionary<IVariable,Dictionary<IMethod, IMethodFaker>> _fakers = new();

        public IMethodFaker GetMethodFaker(ExplicitActiveState cur, IVariable instanceVariable, IMethod method) 
        {
            if (_inputModel.TryGetValue(instanceVariable, out IValue v))
            {
                if (v is Location l)
                {
                    if (!_fakers.TryGetValue(instanceVariable, out Dictionary<IMethod, IMethodFaker> f))
                    {
                        f = new Dictionary<IMethod, IMethodFaker>(MethodEqualityComparer.CompareDeclaringTypes);
                        _fakers[instanceVariable] = f;
                    }

                    if (!f.TryGetValue(method, out IMethodFaker faker))
                    {
                        faker = CreateFaker(l, instanceVariable, method);
                        f[method] = faker;
                    }
                    return faker;
                }
            }

            Debug.Fail("The object should exist in the input model.");

            return null;
        }

        private IMethodFaker CreateFaker(Location location, IVariable instanceVariable, IMethod method)
        {
            if (!_inputModel.HeapInfo.TryGetNode(location, out IHeapNode n)) throw new InvalidOperationException("This should not have happened");

            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)n;

            if (objNode.TryGetConstrainedMethodResults(method, out IEnumerable<KeyValuePair<Expression, IValue>> results))
            {
                return new ConstraintedMethodFaker(method, location, instanceVariable, results.Select(cr => cr.Key));
            }
            else
            {
                return new InvocationBasedMethodFaker(method, location, instanceVariable);
            }
        }
    }
}
