using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static partial class Extensions
    {
        public static ExtendableInstructionFactory AddSymbolicExecution(this ExtendableInstructionFactory factory)
        {
            // conversion instructions
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.CONV());

            return factory;
        }
    }
}
