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

            // branching instruction
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BinaryBranch());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BRFALSE());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BRTRUE());

            // operations
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BinaryOperation());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.UnaryOperation());

            // math methods
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.SystemMath());

            return factory;
        }
    }
}
