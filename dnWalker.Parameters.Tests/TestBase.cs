using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            DefinitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(TestBase).Assembly));
        }

        protected IDefinitionProvider DefinitionProvider { get; }


    }
}
