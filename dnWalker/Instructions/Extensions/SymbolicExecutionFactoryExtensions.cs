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
            const string SymbolicExtensionsNamespace = "dnWalker.Instructions.Extensions.Symbolic";
            return factory.RegisterExtensionsFrom(SymbolicExtensionsNamespace);


            //    // conversion instructions
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.LDARG());
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.LDARGA());

            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.CONV());
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.CONV_OVF());

            //    // branching instruction
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BinaryBranch());
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BRFALSE());
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BRTRUE());

            //    // operations
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.BinaryOperation());
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.UnaryOperation());

            //    // math methods
            //    factory.RegisterExtension(new dnWalker.Instructions.Extensions.Symbolic.SystemMath());

            //    return factory;
        }
    }
}
