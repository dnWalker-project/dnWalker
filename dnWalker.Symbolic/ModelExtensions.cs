using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static partial class ModelExtensions
    {
        /// <summary>
        /// Evaluates the expression with regards to the model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IValue Evaluate(this IReadOnlyModel model, Expression expression)
        {
            // TODO: handle more complex expressions
            // via ExpressionVisitor and value stack...
            if (expression is VariableExpression varExpr && 
                varExpr.Variable is IRootVariable rootVar &&
                model.TryGetValue(rootVar, out IValue? value))
            {
                return value;
            }

            // return ExpressionEvaluator.Evaluate(model, expression);

            throw new NotSupportedException();
        }

        public static IValue GetValueOrDefault(this IReadOnlyModel model, IVariable variable)
        {
            if (model.TryGetValue(variable, out IValue? value))
            {
                return value;
            }
            return ValueFactory.GetDefault(variable.Type);
        }

        /// <summary>
        /// Tries to get the value for the variable by walking the heap using the variable chain.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue(this IReadOnlyModel model, IVariable variable, [NotNullWhen(true)]out IValue? value, bool forceInitialization = false)
        {
            if (variable is IRootVariable rootVar) return model.TryGetValue(rootVar, out value);
            else if (variable is IMemberVariable memberVar)
            {
                if (!model.TryGetValue(memberVar.Parent, out value))
                {
                    // could not get the parent... some weird error
                    value = null;
                    return false;
                }

                Location parentLocation = (Location)value;
                IReadOnlyHeapNode parentNode = model.HeapInfo.GetNode(parentLocation);

                if (variable is ArrayElementVariable aev)
                {
                    if (forceInitialization)
                    {
                        value = ((IReadOnlyArrayHeapNode)parentNode).GetElementOrDefault(aev.Index);
                        return true;
                    }
                    return ((IReadOnlyArrayHeapNode)parentNode).TryGetElement(aev.Index, out value);
                }
                else if (variable is ArrayLengthVariable)
                {
                    value = new PrimitiveValue<uint>((uint)((IReadOnlyArrayHeapNode)parentNode).Length);
                    return true;
                }
                else if (variable is InstanceFieldVariable ifv)
                {
                    if (forceInitialization)
                    {
                        value = ((IReadOnlyObjectHeapNode)parentNode).GetFieldOrDefault(ifv.Field);
                        return true;
                    }
                    return ((IReadOnlyObjectHeapNode)parentNode).TryGetField(ifv.Field, out value);
                }
                else if (variable is MethodResultVariable mrv)
                {
                    if (forceInitialization)
                    {
                        value = ((IReadOnlyObjectHeapNode)parentNode).GetMethodResultOrDefault(mrv.Method, mrv.Invocation);
                        return true;
                    }
                    return ((IReadOnlyObjectHeapNode)parentNode).TryGetMethodResult(mrv.Method, mrv.Invocation, out value);
                }

                return false;
            }

            throw new InvalidOperationException("Unexpected variable type.");
        }

        /// <summary>
        /// Tries to set the value for the variable by walking the heap using the variable chain.
        /// If some part of the chain is missing, fails.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TrySetValue(this IModel model, IVariable variable, IValue value)
        {
            if (variable is IRootVariable rootVar) { model.SetValue(rootVar, value); return true; }
            else if (variable is IMemberVariable memberVar)
            {
                if (!model.TryGetValue(memberVar.Parent, out IValue? parentValue))
                {
                    // could not get the parent... some weird error
                    return false;
                }

                Location parentLocation = (Location)parentValue;
                IHeapNode parentNode = model.HeapInfo.GetNode(parentLocation);

                if (variable is ArrayElementVariable aev)
                {
                    ((IArrayHeapNode)parentNode).SetElement(aev.Index, value);
                    return true;
                }
                else if (variable is ArrayLengthVariable)
                {
                    ((IArrayHeapNode)parentNode).Length = (int)((PrimitiveValue<uint>)value).Value;
                    return true;
                }
                else if (variable is InstanceFieldVariable ifv)
                {
                    ((IObjectHeapNode)parentNode).SetField(ifv.Field, value);
                    return true;
                }
                else if (variable is MethodResultVariable mrv)
                {
                    ((IObjectHeapNode)parentNode).SetMethodResult(mrv.Method, mrv.Invocation, value);
                    return true;
                }
            }

            throw new InvalidOperationException("Unexpected variable type.");

        }

        public static bool IsEmpty(this IReadOnlyModel self)
        {
            return self.Variables.Count == 0 &&
                self.HeapInfo.IsEmpty();
        }

        public static bool TryGetReturnValue(this IReadOnlyModel self, dnlib.DotNet.IMethod method, [NotNullWhen(true)]out IValue? value)
        {
            return self.TryGetValue(new ReturnValueVariable(method), out value);
        }
    }
}
