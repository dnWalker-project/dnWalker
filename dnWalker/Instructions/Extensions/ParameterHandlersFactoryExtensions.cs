using dnWalker.Instructions.Extensions.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static partial class Extensions
    {
        public static ExtendableInstructionFactory AddParameterHandlers(this ExtendableInstructionFactory factory)
        {
            // reference type parameter, ref/equal, is null?
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.BRTRUE());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.BRFALSE());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.CEQ());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.CGT());

            // array type parameter
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.LDLEN());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.LDELEM());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.LDELEMA());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.STELEM());

            // object type parameter
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.LDFLD());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.LDFLDA());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.STFLD());
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.CALLVIRT());

            // return value initializer
            factory.RegisterExtension(new dnWalker.Instructions.Extensions.Parameters.RET());

            return factory;
        }
    }
}
