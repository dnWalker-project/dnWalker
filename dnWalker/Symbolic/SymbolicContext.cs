using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public partial class SymbolicContext
    {
        private readonly IModel _inputModel;
        private readonly IModel _execModel;

        private readonly MMC.State.ExplicitActiveState _state;

        private SymbolicContext(IModel inputModel, IModel execModel, MMC.State.ExplicitActiveState cur)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _execModel = execModel ?? throw new ArgumentNullException(nameof(execModel));
            _state = cur;
        }

        public IModel InputModel => _inputModel;
        public IModel OutputModel => _execModel;


        public static SymbolicContext Create(IModel model, MMC.State.ExplicitActiveState cur)
        {
            return new SymbolicContext(model, model.Clone(), cur);
        }

        public Expression ProcessExistingObject(ObjectReference objRef)
        {
            // this should not be executed...
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the variable has been initialized (queries appropriate parts of the explicit active state).
        /// If the variable is not initialized, performs lazy initialization.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="cur"></param>
        /// <returns>True if initialization has occurred, false otherwise.</returns>
        public bool EnsureInitialized(IVariable variable, ExplicitActiveState cur)
        {
            return variable switch
            {
                // root
                Variables.MethodArgumentVariable mav => EnsureInitialized(mav, cur),
                Variables.StaticFieldVariable sfv => EnsureInitialized(sfv, cur),

                // member
                Variables.ArrayElementVariable aev => EnsureInitialized(aev, cur),
                Variables.ArrayLengthVariable alv => EnsureInitialized(alv.Parent, cur), // we need to ensure the array is initialized... should be and this call is probably redundant, as the array must have been loaded using another LD* instruction...
                Variables.InstanceFieldVariable ifv => EnsureInitialized(ifv, cur),
                Variables.MethodResultVariable mrv => throw new InvalidOperationException("Method result may be lazily initialized, but should be handled directly by the executor!"),
                _ => throw new InvalidOperationException("Unexpected variable type.")
            };
        }

        private readonly IDictionary<Location, IDictionary<IMethod, int>> _invocationCounter = new Dictionary<Location, IDictionary<IMethod, int>>();

        public int GetInvocation(Location symbolicLocation, IMethod method)
        {
            if (!_invocationCounter.TryGetValue(symbolicLocation, out IDictionary<IMethod, int> methodCounter))
            {
                methodCounter = new Dictionary<IMethod, int>(MethodEqualityComparer.CompareDeclaringTypes);
                _invocationCounter.Add(symbolicLocation, methodCounter);
            }

            if (!methodCounter.ContainsKey(method))
            {
                methodCounter[method] = 1;
                return 1;
            }
            else
            {
                return ++methodCounter[method];
            }
        }

    }
}
