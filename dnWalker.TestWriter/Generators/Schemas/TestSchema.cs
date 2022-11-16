using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Schemas
{
    public abstract class TestSchema : ITestSchema
    {
        private readonly ITestContext _testContext;

        protected TestSchema(ITestContext testContext)
        {
            _testContext = testContext;
        }

        public static readonly IEnumerable<ITestSchema> NoSchemas = Array.Empty<ITestSchema>();

        public ITestContext TestContext
        {
            get
            {
                return _testContext;
            }
        }

        public abstract void Write(ITestTemplate testTemplate, IWriter output);
    }
}
