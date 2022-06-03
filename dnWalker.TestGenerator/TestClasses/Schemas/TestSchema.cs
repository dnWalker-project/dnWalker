
using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    /// <summary>
    /// Base class for all assertion schemas.
    /// </summary>
    public abstract class TestSchema
    {
        private readonly ITestClassContext _context;

        protected TestSchema(ITestClassContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ITestClassContext Context => _context;
        public IReadOnlyModel InputModel => _context.InputModel;
        public IReadOnlyModel OutputModel => _context.OutputModel;
        public IMethod Method => _context.Method;

        public virtual string TestMethodName => $"{Method.Name}_{GetType().Name}";

        public abstract void Write(IWriter output, ITemplateProvider templates);
    }
}
