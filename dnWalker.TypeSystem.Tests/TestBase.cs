using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests
{
    public class TestBase
    {
        protected IDefinitionProvider DefinitionProvider
        {
            get;
        }

        public TestBase()
        {
            DefinitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(TestBase).Assembly));
        }
    }
}
