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
using dnWalker.TypeSystem;

namespace dnWalker.Parameters
{
    public class ParameterStore
    {
        private readonly MethodDef _entryPoint;

        private readonly IParameterContext _context;

        private readonly IBaseParameterSet _baseSet;
        private readonly List<IExecutionParameterSet> _executionSets = new List<IExecutionParameterSet>();

        public ParameterStore(MethodDef entryPoint, IDefinitionProvider definitionProvider)
        {
            _context = new ParameterContext(definitionProvider);
            _baseSet = new BaseParameterSet(_context);

            foreach (var arg in entryPoint.Parameters)
            {
                IParameter parameter = _baseSet.CreateParameter(arg.Type);
                parameter.Accessors.Add(new MethodArgumentParameterAccessor(arg.Name));
                _baseSet.Roots.Add(arg.Name, parameter.Reference);
            }
            _entryPoint = entryPoint;

            if (entryPoint.HasThis)
            {
                IParameter parameter = _baseSet.CreateParameter(entryPoint.DeclaringType.ToTypeSig());
                parameter.Accessors.Add(new MethodArgumentParameterAccessor(ThisName));
                _baseSet.Roots.Add(ThisName, parameter.Reference);
            }
        }

        /// <summary>
        /// Gets the parameter context which describes input into the next execution.
        /// </summary>
        public IBaseParameterSet BaseSet
        {
            get 
            {
                return _baseSet ?? throw new InvalidOperationException("Cannot access the 'BaseContext', the store is not initialized!");
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
        public IExecutionParameterSet ExecutionSet
        {
            get
            {
                return _executionSets.Count > 0 ? _executionSets[_executionSets.Count - 1] : throw new InvalidOperationException("Cannot access the 'ExecutionContext', it is not initialized!");
            }
        }

        public IReadOnlyList<IExecutionParameterSet> ExecutionSets
        {
            get
            {
                return _executionSets;
            }
        }

        public DataElementList CreateMethodArguments(ExplicitActiveState cur)
        {
            MethodDef method = _entryPoint;

            int parameterCount = method.GetParamCount() + (method.HasThis ? 1 : 0);
            DataElementList arguments = cur.StorageFactory.CreateList(parameterCount);

            int idx = 0;

            //IParameterContext ctx = _baseContext;
            IParameterSet set = ExecutionSet;

            if (method.HasThis)
            {
                IParameter thisParameter = set.Roots[ThisName].Resolve(set);
                arguments[idx++] = thisParameter.AsDataElement(cur);
            }

            foreach (DnParameter p in method.Parameters)
            {
                IParameter argParameter = set.Roots[p.Name].Resolve(set);
                arguments[idx++] = argParameter.AsDataElement(cur);
            }

            return arguments;
        }

        /// <summary>
        /// Clears the <see cref="ParameterStore.ExecutionSet"/> and initializes a new <see cref="ParameterStore.ExecutionSet"/>.
        /// </summary>
        public void InitializeExecutionContext()
        {
            _executionSets.Add(_baseSet.CreateExecutionSet());
        }

        private ParameterRef _returnValue;

        public void SetReturnValue(IDataElement retValue, ExplicitActiveState cur, TypeSig retValueType)
        {
            IParameter parameter = retValue.GetOrCreateParameter(cur, new TypeSignature(retValueType.ToTypeDefOrRef()));
            parameter.Accessors.Add(new ReturnValueParameterAccessor());

            ExecutionSet.Roots.Add(ReturnValueName, parameter.Reference);

            _returnValue = parameter.Reference;
        }

        public ParameterRef ReturnValue
        {
            get
            {
                return _returnValue;
            }
        }

        public IParameterContext Context
        {
            get
            {
                return _context;
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
                traitsArray[i].ApplyTo(_baseSet);
            }
        }
    }
}
