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
            factory.RegisterExtension(new BRTRUE_ParameterHandler());
            factory.RegisterExtension(new BRFALSE_ParameterHandler());
            factory.RegisterExtension(new CEQ_ReferenceTypeParameterHandler());
            factory.RegisterExtension(new CNE_ReferenceTypeParameterHandler());

            // array type parameter
            factory.RegisterExtension(new LDLEN_ParameterHandler());
            factory.RegisterExtension(new LDELEM_ParameterHandler());
            factory.RegisterExtension(new STELEM_ParameterHandler());

            // object type parameter
            factory.RegisterExtension(new LDFLD_ParameterHandler());
            factory.RegisterExtension(new STFLD_ParameterHandler());
            factory.RegisterExtension(new CALLVIRT_ParameterHandler());

            // return value initializer
            factory.RegisterExtension(new ReturnValueParameterHandler());

            return factory;
        }
    }
}
