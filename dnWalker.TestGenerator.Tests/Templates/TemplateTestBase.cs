using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class TemplateTestBase
    {
        protected IDefinitionProvider DefinitionProvider { get; }

        public TemplateTestBase()
        {
            DefinitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(TemplateTestBase).Assembly));
        }
    }
}
