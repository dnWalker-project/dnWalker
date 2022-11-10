using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class ExpressionFactory
    {
        public static readonly ExpressionFactory Default = new DefaultFactory();

        private class DefaultFactory : CustomModuleExpressionFactory
        {
            public DefaultFactory() : base(ModuleDefMD.Load(typeof(object).Assembly.ManifestModule, ModuleDef.CreateModuleContext()))
            {
            }

        }
    }
}
