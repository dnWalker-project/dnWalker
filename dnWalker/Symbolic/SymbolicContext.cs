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
        private readonly IModel _outputModel;

        private SymbolicContext(IModel inputModel, IModel outputModel)
        {
            _inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _outputModel = outputModel ?? throw new ArgumentNullException(nameof(outputModel));
        }

        public IModel InputModel => _inputModel;
        public IModel OutputModel => _outputModel;


        public static SymbolicContext Create(IModel model)
        {
            return new SymbolicContext(model, model.Clone());
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
    }
}
