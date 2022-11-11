using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3.Tests
{
    public abstract class DnlibProvider
    {
        protected ModuleDef Module
        {
            get;
        }

        protected ExpressionFactory Expressions
        {
            get;
        }

        protected DnlibProvider()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            Module = ModuleDefMD.Load(typeof(DnlibProvider).Module, ctx);

            Expressions = new CustomModuleExpressionFactory(Module);
        }
    }
}
