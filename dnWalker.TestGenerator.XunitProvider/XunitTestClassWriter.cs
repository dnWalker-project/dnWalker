using dnWalker.TestGenerator.Templates;
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
        private readonly XunitFramework _framework;
        private readonly ITemplateProvider _templates;
        private readonly XunitTestClassTemplate _mainTemplate;

        public XunitTestClassWriter(XunitFramework framework, ITemplateProvider templates)
        {
            _framework = framework;
            _templates = templates;
            _mainTemplate = new XunitTestClassTemplate(_templates);
        }

        public void WriteTestClass(TextWriter output, ITestClassContext context)
        {
            string content = _mainTemplate.GenerateContent(context);

            output.WriteLine(content);
        }
    }
}
