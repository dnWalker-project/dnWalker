using System.Collections.Generic;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public interface ITestInterface
    {
        int PrimitiveValue_Method();
        int PrimitiveValue_Method(double dbl, string str);
        List<string> ComplexValue_Method(List<double> dbls);
        List<string> ComplexValue_Method();
    }
}
