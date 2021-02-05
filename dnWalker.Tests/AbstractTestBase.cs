using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace dnWalker.Tests
{
    public abstract class AbstractTestBase : IDisposable
    {
        private TextWriter _before;

        public AbstractTestBase(ITestOutputHelper testOutputHelper)
        {
            var converter = new Converter(testOutputHelper);
            _before = Console.Out;
            Console.SetOut(converter);
        }

        public virtual void Dispose()
        {
            Console.SetOut(_before);
        }

        private class Converter : TextWriter
        {
            private readonly ITestOutputHelper _output;

            public Converter(ITestOutputHelper output)
            {
                _output = output;
            }

            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }

            public override void WriteLine(string message)
            {
                _output.WriteLine(message);
            }

            public override void WriteLine(string format, params object[] args)
            {
                _output.WriteLine(format, args);
            }
        }
    }
}
