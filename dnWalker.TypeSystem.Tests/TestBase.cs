using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests
{
    public class TestBase
    {
        protected static IDefinitionProvider DefinitionProvider
        {
            get;
        }

        static TestBase()
        {
            DefinitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(TestBase).Assembly));
        }

        public TestBase()
        {
            
        }
    }
}
