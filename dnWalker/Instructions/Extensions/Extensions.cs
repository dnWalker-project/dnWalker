using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static class Extensions
    {
        public static ExtendableInstructionFactory AddStandardExtensions(this ExtendableInstructionFactory factory)
        {
            factory.AddParameterHandlers();
            factory.AddSymbolicExecution();
            factory.RegisterExtension()
            factory.AddPathConstraintProducers();

            return factory;
        }
    }
}
