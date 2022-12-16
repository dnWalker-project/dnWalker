using dnlib.DotNet;

using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class BasicTestClassNamingStrategy : ITestClassNamingStrategy
    {
        public string GetClassName(IMethod methodUnderTest)
        {
            ITypeDefOrRef classUnderTest = methodUnderTest.DeclaringType;
            
            return $"{classUnderTest.GetName().Replace('.', '_')}_{methodUnderTest.Name}_Tests";
        }

        public string GetNamespaceName(IMethod methodUnderTest)
        {
            ModuleDef module = methodUnderTest.Module;
            string moduleName = module.Name;
            
            // remove the suffix
            if (moduleName.EndsWith(".exe") || moduleName.EndsWith(".dll"))
            {
                moduleName = moduleName.Substring(0, moduleName.Length - 4);
            }

            string classUnderTestNS = methodUnderTest.DeclaringType.GetNamespace();

            if (!classUnderTestNS.StartsWith(moduleName))
            {
                return $"Tests.{classUnderTestNS}";
            }
            else
            {
                return $"{moduleName}.Tests.{classUnderTestNS.Substring(moduleName.Length + 1)}";
            }
        }
    }
}
