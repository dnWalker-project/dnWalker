using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    public static class TestFrameworkExtensions
    {
        public static IEnumerable<PackageReference> GetPackages(this ITestFramework testFramework)
        {
            List<PackageReference> packages = new();

            // add packages from the test framework
            foreach (AddPackageAttribute pAtt in testFramework.GetType().GetCustomAttributes<AddPackageAttribute>())
            {
                packages.Add(pAtt.GetPackageReference());
            }

            return packages;
        }
    }
}
