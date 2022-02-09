using dnlib.DotNet;

using dnWalker.DataElements;
using dnWalker.Parameters.Expressions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DnParameter = dnlib.DotNet.Parameter;

using static dnWalker.Parameters.ParameterAccessor;

namespace dnWalker.Parameters
{
    public class ParameterStore
    {
        private readonly MethodDef _entryPoint;

        private readonly IBaseParameterContext _baseContext;
        private IExecutionParameterContext _executionContext;

        public ParameterStore(MethodDef entryPoint)
        {
            _baseContext = new BaseParameterContext();

            foreach (var arg in entryPoint.Parameters)
            {
                IParameter parameter = _baseContext.CreateParameter(arg.Type);
                parameter.Accessors.Add(new MethodArgumentParameterAccessor(arg.Name));
                _baseContext.Roots.Add(arg.Name, parameter.Reference);
            }
            _entryPoint = entryPoint;

            if (entryPoint.HasThis)
            {
                IParameter parameter = _baseContext.CreateParameter(entryPoint.DeclaringType.ToTypeSig());
                parameter.Accessors.Add(new MethodArgumentParameterAccessor(ThisName));
                _baseContext.Roots.Add(ThisName, parameter.Reference);
            }
        }

        /// <summary>
        /// Gets the parameter context which describes input into the next execution.
        /// </summary>
        public IBaseParameterContext BaseContext
        {
            get 
            {
                return _baseContext ?? throw new InvalidOperationException("Cannot access the 'BaseContext', the store is not initialized!");
            }
        }

        public MethodDef EntryPoint
        {
            get
            {
                return _entryPoint;
            }
        }

        /// <summary>
        /// Gets the parameter context which describes current state.
        /// </summary>
        public IExecutionParameterContext ExecutionContext
        {
            get
            {
                return _executionContext ?? throw new InvalidOperationException("Cannot access the 'ExecutionContext', it is not initialized!");
            }
        }

        public DataElementList CreateMethodArguments(ExplicitActiveState cur)
        {
            MethodDef method = _entryPoint;

            int parameterCount = method.GetParamCount() + (method.HasThis ? 1 : 0);
            DataElementList arguments = cur.StorageFactory.CreateList(parameterCount);

            int idx = 0;

            //IParameterContext ctx = _baseContext;
            IParameterContext ctx = _executionContext;

            if (method.HasThis)
            {
                IParameter thisParameter = ctx.Roots[ThisName].Resolve(ctx);
                arguments[idx++] = thisParameter.AsDataElement(cur);
            }

            foreach (DnParameter p in method.Parameters)
            {
                IParameter argParameter = ctx.Roots[p.Name].Resolve(ctx);
                arguments[idx++] = argParameter.AsDataElement(cur);
            }

            return arguments;
        }

        /// <summary>
        /// Clears the <see cref="ParameterStore.ExecutionContext"/> and initializes a new <see cref="ParameterStore.ExecutionContext"/>.
        /// </summary>
        public void InitializeExecutionContext()
        {
            _executionContext = _baseContext.CreateExecutionContext();
        }

        private ParameterRef _returnValue;

        public void SetReturnValue(IDataElement retValue, ExplicitActiveState cur, TypeSig retValueType)
        {
            IParameter parameter = retValue.GetOrCreateParameter(cur, retValueType);
            parameter.Accessors.Add(new ReturnValueParameterAccessor());

            _executionContext.Roots.Add(ReturnValueName, parameter.Reference);

            _returnValue = parameter.Reference;
        }

        public ParameterRef ReturnValue
        {
            get
            {
                return _returnValue;
            }
        }

        private class ExprSorter : IComparer<ParameterTrait>
        {
            public int Compare(ParameterTrait x, ParameterTrait y)
            {
                // return -1 if expr1 <  expr2 => we want to handle expr1 before expr2
                // return  0 if expr1 == expr2 => we do not care which one we handle first
                // return  1 if expr1 >  expr2 => we want to handle expr2 before expr1

                ParameterExpression px = x.Expression;
                ParameterExpression py = y.Expression;

                switch ((px, py))
                {
                    // value of, length of, is null - all must be handled before any ref equals
                    case (ValueOfParameterExpression _, RefEqualsParameterExpression _):
                    case (LengthOfParameterExpression _, RefEqualsParameterExpression _):
                    case (IsNullParameterExpression _, RefEqualsParameterExpression _):
                        return -1;

                    case (RefEqualsParameterExpression _, RefEqualsParameterExpression _):
                    default:
                        // left is ! refEquals, right is ! RefEquals
                        return 0;

                    case (RefEqualsParameterExpression _, _):
                        return 1;
                }
            }

            private static readonly Lazy<ExprSorter> _lazy = new Lazy<ExprSorter>();

            public static ExprSorter Instance { get { return _lazy.Value; } }
        }

        public void Apply(IEnumerable<ParameterTrait> traits)
        {
            ParameterTrait[] traitsArray = traits.ToArray();
            // sort the traits so that RefEquals are last
            Array.Sort(traitsArray, ExprSorter.Instance);

            for (int i = 0; i < traitsArray.Length; ++i)
            {
                traitsArray[i].ApplyTo(_baseContext);
            }
        }
    }
}
