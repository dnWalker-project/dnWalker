using dnWalker.Parameters;
using dnWalker.TestGenerator.TestClasses;
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

        protected static TestClassContext BuildContext(IParameterContext context, IReadOnlyParameterSet baseSet, IReadOnlyParameterSet execSet)
        {
            TestClassContext.Builder builder = TestClassContext.Builder.NewEmpty();
            builder.ParameterContext = context;
            builder.BaseSet = baseSet;
            builder.ExecutionSet = execSet;
            return builder.Build();
        }
    }
}
