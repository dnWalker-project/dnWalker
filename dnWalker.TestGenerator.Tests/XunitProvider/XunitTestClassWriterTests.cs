using System;

using dnWalker.TypeSystem;
using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.Templates.FluentAssertions;
using dnWalker.TestGenerator.Templates.Moq;
using dnWalker.TestGenerator.XunitProvider;

namespace dnWalker.TestGenerator.Tests.XunitProvider
{
    public class XunitTestClassWriterTests
    {
        private XunitFramework _framework;
        private XunitTestClassWriter _classWriter;

        public XunitTestClassWriterTests()
        {
            _framework = new XunitFramework();
            _classWriter = _framework.CreateClassWriter(TestUtils.Templates);
        }


    }
}

