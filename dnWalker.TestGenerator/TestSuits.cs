using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public enum TestSuit
    {
        xUnit
    }

    public static class TestSuitExtensions
    {
        public static ITestSuitContext GetContext(this TestSuit testSuit)
        {
            switch (testSuit)
            {
                case TestSuit.xUnit:
                    return new xUnit.XUnitTestSuitContext();

                default:
                    throw new Exception("Unexpected test suit requested.");
            }
        }
    }
}
