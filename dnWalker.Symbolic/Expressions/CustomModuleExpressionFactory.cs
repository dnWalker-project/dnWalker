using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class CustomModuleExpressionFactory : ExpressionFactory
    {
        private readonly ModuleDef _module;

        public CustomModuleExpressionFactory(ModuleDef module)
        {
            _module = module;
        }

        protected override TypeSig GetIntegerSig()
        {
            return _module.CorLibTypes.Int64;
        }

        protected override TypeSig GetBooleanSig()
        {
            return _module.CorLibTypes.Boolean;
        }

        protected override TypeSig GetDoubleSig()
        {
            return _module.CorLibTypes.Double;
        }

        protected override TypeSig GetStringSig()
        {
            return _module.CorLibTypes.String;
        }
        protected override TypeSig GetObjectSig()
        {
            return _module.CorLibTypes.Object;
        }
    }
}
