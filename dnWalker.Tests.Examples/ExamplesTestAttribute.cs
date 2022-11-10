using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Sdk;

namespace dnWalker.Tests.Examples
{
    [XunitTestCaseDiscoverer("dnWalker.Tests.Examples.ExamplesDiscoverer", "dnWalker.Tests.Examples")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExamplesTestAttribute : FactAttribute
    {
    }
}
