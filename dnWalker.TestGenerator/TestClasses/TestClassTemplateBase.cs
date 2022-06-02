using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public abstract class TestClassTemplateBase : Templates.TemplateBase<ITestClassContext>
    {
        private readonly ITemplateProvider _templateProvider;

        protected TestClassTemplateBase(ITemplateProvider templateProvider)
        {
            _templateProvider = templateProvider ?? throw new ArgumentNullException(nameof(templateProvider));
        }

        public ITemplateProvider TemplateProvider => _templateProvider;
    }
}
