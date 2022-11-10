using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public abstract class TemplateBase<TContext> : TemplateBase
        where TContext : class
    {
        private TContext? _context = null;


        public virtual string GenerateContent(TContext context)
        {
            if (Interlocked.CompareExchange(ref _context, context, null) != null) throw new InvalidOperationException("The template already runs with different context.");

            string content = TransformText();

            Interlocked.Exchange(ref _context, null);

            return content;
        }

        public TContext Context => _context ?? throw new InvalidOperationException("The template is not initialized.");
    }
}
