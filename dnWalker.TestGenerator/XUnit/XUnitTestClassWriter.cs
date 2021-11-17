using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XUnit
{
    public class XUnitTestClassWriter : IDisposable
    {
        private readonly XUnitTestClassTemplate _testClassTemplate = new XUnitTestClassTemplate();
        private readonly TextWriter _output;
        private readonly bool _leaveOpen;

        public XUnitTestClassWriter(TextWriter output, bool leaveOpen = false)
        {
            _output = output;
            _leaveOpen = leaveOpen;
        }

        public void Write(TestGeneratorContext testData)
        {
            _testClassTemplate.TestData = testData;

            _output.WriteLine(_testClassTemplate.TransformText());
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                ((IDisposable)_output).Dispose();
            }
        }
    }
}
