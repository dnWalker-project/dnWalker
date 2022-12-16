using dnWalker.Tests.Examples;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.IntegrationTests
{
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class IntegrationTestAttribute : ExamplesTestAttribute
    {
    }
}
