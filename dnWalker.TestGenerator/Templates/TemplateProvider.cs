using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IArrangeTemplate _arrangeTemplate;
        private readonly IActTemplate _actTemplate;
        private readonly IAssertTemplate _assertTemplate;

        private readonly string[] _namespaces;

        public TemplateProvider(IArrangeTemplate arrangeTemplate, IActTemplate actTemplate, IAssertTemplate assertTemplate)
        {
            _arrangeTemplate = arrangeTemplate ?? throw new ArgumentNullException(nameof(arrangeTemplate));
            _actTemplate = actTemplate ?? throw new ArgumentNullException(nameof(actTemplate));
            _assertTemplate = assertTemplate ?? throw new ArgumentNullException(nameof(assertTemplate));

            _namespaces = arrangeTemplate.Namespaces
                .Concat(actTemplate.Namespaces)
                .Concat(assertTemplate.Namespaces)
                .ToArray();

            //Array.Sort(_namespaces, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> Namespaces
        {
            get
            {
                return _namespaces;
            }
        }


        public IArrangeTemplate ArrangeTemplate => _arrangeTemplate;
        public IActTemplate ActTemplate => _actTemplate;
        public IAssertTemplate AssertTemplate => _assertTemplate;
    }
}
