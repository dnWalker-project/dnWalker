using dnWalker.Concolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.Strings
{
    public abstract class MethodsWithStringParameterTestsBase : ExamplesTestBase
    {
        private const string ClassName = "Examples.Concolic.Features.Strings.MethodsWithStringParameter";

        public MethodsWithStringParameterTestsBase(ITestOutputHelper output) : base(output)
        {
        }

        protected static ExplorationResult Run(IExplorer explorer, [CallerMemberName] String? methodName = null)
        {
            return explorer.Run(ClassName + "." + methodName ?? throw new ArgumentNullException(nameof(methodName)));
        }

    }
}
