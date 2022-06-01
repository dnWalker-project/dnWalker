using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Symbols;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class TestWriter : ITemplate, IArrangeTemplate, IActTemplate, IAssertTemplate
    {
        private readonly IArrangeTemplate _arrangeTemplate;
        private readonly IActTemplate _actTemplate;
        private readonly IAssertTemplate _assertTemplate;

        private readonly string[] _namespaces;

        public TestWriter(IArrangeTemplate arrangeTemplate, IActTemplate actTemplate, IAssertTemplate assertTemplate)
        {
            _arrangeTemplate = arrangeTemplate ?? throw new ArgumentNullException(nameof(arrangeTemplate));
            _actTemplate = actTemplate ?? throw new ArgumentNullException(nameof(actTemplate));
            _assertTemplate = assertTemplate ?? throw new ArgumentNullException(nameof(assertTemplate));

            _namespaces = arrangeTemplate.Namespaces
                .Concat(actTemplate.Namespaces)
                .Concat(assertTemplate.Namespaces)
                .ToArray();

            Array.Sort(_namespaces, StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> Namespaces
        {
            get
            {
                return _namespaces;
            }
        }

        #region IArrangeTemplate
        public IDictionary<Location, string> WriteArrange(IWriter output, IReadOnlyModel model)
        {
            return _arrangeTemplate.WriteArrange(output, model);
        }
        #endregion IArrangeTemplate

        #region IActTemplate
        public void WriteAct(IWriter output, IMethod method, string[] argumentSymbols, string? returnSymbol = null)
        {
            _actTemplate.WriteAct(output, method, argumentSymbols, returnSymbol);
        }
        public void WriteActDelegate(IWriter output, IMethod method, string[] argumentSymbols, string? returnSymbol = null, string delegateSymbol = "act")
        {
            _actTemplate.WriteActDelegate(output, method, argumentSymbols, returnSymbol, delegateSymbol);
        }
        #endregion IActTemplate

        #region IAssertTemplate
        public void WriteAssertNull(IWriter output, string symbol)
        {
            _assertTemplate.WriteAssertNull(output, symbol);
        }

        public void WriteAssertNotNull(IWriter output, string symbol)
        {
            _assertTemplate.WriteAssertNotNull(output, symbol);
        }

        public void WriteAssertEqual(IWriter output, string left, string right)
        {
            _assertTemplate.WriteAssertEqual(output, left, right);
        }

        public void WriteAssertNotEqual(IWriter output, string left, string right)
        {
            _assertTemplate.WriteAssertNotEqual(output, left, right);
        }

        public void WriteAssertSame(IWriter output, string left, string right)
        {
            _assertTemplate.WriteAssertSame(output, left, right);
        }

        public void WriteAssertNotSame(IWriter output, string left, string right)
        {
            _assertTemplate.WriteAssertNotSame(output, left, right);
        }

        public void WriteAssertExceptionThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            _assertTemplate.WriteAssertExceptionThrown(output, delegateSymbol, exceptionType);
        }

        public void WriteAssertExceptionNotThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            _assertTemplate.WriteAssertExceptionNotThrown(output, delegateSymbol, exceptionType);
        }

        public void WriteAssertNoExceptionThrown(IWriter output, string delegateSymbol)
        {
            _assertTemplate.WriteAssertNoExceptionThrown(output, delegateSymbol);
        }
        #endregion IAssertTemplate
    }
}
