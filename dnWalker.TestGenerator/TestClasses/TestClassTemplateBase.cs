using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses
{
    public abstract class TestClassTemplateBase : TemplateBase<ITestClassContext>
    {
        private readonly ITemplateProvider _templateProvider;

        protected TestClassTemplateBase(ITemplateProvider templateProvider)
        {
            _templateProvider = templateProvider ?? throw new ArgumentNullException(nameof(templateProvider));
        }

        public ITemplateProvider TemplateProvider => _templateProvider;

        public IEnumerable<IGrouping<string, string>> GetNamespaces()
        {
            IEnumerable<string> namespaces = Context.Usings.Concat(_templateProvider.Namespaces);

            return namespaces.GroupBy(static ns =>
            {
                int dot = ns.IndexOf('.');
                if (dot < 0) return ns;
                return ns.Substring(0, dot);
            });
        }
    }
}
