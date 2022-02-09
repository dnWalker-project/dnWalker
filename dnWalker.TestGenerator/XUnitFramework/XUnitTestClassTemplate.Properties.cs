using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XUnitFramework
{
    internal partial class XUnitTestClassTemplate : TemplateBase
    {

        public string GenerateContent(TestClassContext context)
        {
            Initialize(context);
            return TransformText();
        }
    }
}
