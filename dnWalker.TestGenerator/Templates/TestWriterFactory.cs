using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class TemplateProviderFactory
    {
        private IArrangeTemplate? _arrangeTemplate;
        private IActTemplate? _actTemplate;
        private IAssertTemplate? _assertTemplate;

        public TemplateProviderFactory()
        {
        }

        public TemplateProviderFactory(IArrangeTemplate? arrangeTemplate, IAssertTemplate? assertTemplate) 
            : this(arrangeTemplate, BasicActTemplate.Instance, assertTemplate)
        { }

        public TemplateProviderFactory(IArrangeTemplate? arrangeTemplate, IActTemplate? actTemplate, IAssertTemplate? assertTemplate)
        {
            _arrangeTemplate = arrangeTemplate;
            _actTemplate = actTemplate;
            _assertTemplate = assertTemplate;
        }

        public IArrangeTemplate? ArrangeTemplate
        {
            get
            {
                return _arrangeTemplate;
            }

            set
            {
                _arrangeTemplate = value;
            }
        }

        public IActTemplate? ActTemplate
        {
            get
            {
                return _actTemplate;
            }

            set
            {
                _actTemplate = value;
            }
        }

        public IAssertTemplate? AssertTemplate
        {
            get
            {
                return _assertTemplate;
            }

            set
            {
                _assertTemplate = value;
            }
        }

        public ITemplateProvider Create()
        {
            return new TemplateProvider(
                _arrangeTemplate ?? throw new InvalidOperationException("The arrange template is not set."),
                _actTemplate ?? throw new InvalidOperationException("The act template is not set."),
                _assertTemplate ?? throw new InvalidOperationException("The assert template is not set.")
                );
        }
    }
}
