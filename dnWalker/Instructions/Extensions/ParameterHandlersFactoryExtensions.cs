using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static class ParameterHandlersFactoryExtensions
    {
        public static ExtendableInstructionFactory AddParameterHandlers(this ExtendableInstructionFactory factory)
        {
            // reference type parameter, ref/equal, is null?
            factory.RegisterExtension(new BRTRUE_ParameterHandler());
            factory.RegisterExtension(new BRFALSE_ParameterHandler());
            factory.RegisterExtension(new CEQ_ReferenceTypeParameterHandler());
            factory.RegisterExtension(new CNE_ReferenceTypeParameterHandler());

            // array type parameter
            factory.RegisterExtension(new LDLEN_ParameterHandler());

            return factory;
        }
    }
}
