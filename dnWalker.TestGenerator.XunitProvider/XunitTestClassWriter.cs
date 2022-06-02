using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XunitProvider
{
    internal class XunitTestClassWriter : ITestClassWriter
    {
        private readonly XunitTestClassTemplate _template = new XunitTestClassTemplate();
        private readonly XunitFramework _framework;

        public XunitTestClassWriter(XunitFramework framework)
        {
            _framework = framework;
        }

        public void WriteTestClass(TextWriter output, ITestClassContext context)
        {
            string content = _template.GenerateContent(context);

            output.WriteLine(content);
        }
    }
}
