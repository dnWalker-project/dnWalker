using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class TextWriterGenerationEnvironment : IGenerationEnvironment
    {
        private readonly TextWriter _output;

        public TextWriterGenerationEnvironment(TextWriter output)
        {
            _output = output;
        }

        public IGenerationEnvironment Append<T>(T value) where T : struct
        {
            _output.Write(value.ToString());
            return this;
        }

        public IGenerationEnvironment Append(string value)
        {
            _output?.Write(value);
            return this;
        }

        public IGenerationEnvironment AppendLine(string value)
        {
            _output.WriteLine(value);
            return this;
        }

        public IGenerationEnvironment AppendFormat(IFormatProvider? provider, string format, params object[] args)
        {
            _output.Write(string.Format(provider, format, args));
            return this;
        }


        public IGenerationEnvironment Append(char value, int repeatCount)
        {
            Span<char> span = stackalloc char[repeatCount];

            span.Fill(value);

            _output.Write(span);
            return this;
        }

        public IGenerationEnvironment Append(ReadOnlySpan<char> value)
        {
            _output.Write(value);
            return this;
        }

        public IGenerationEnvironment Append(ReadOnlyMemory<char> value)
        {
            _output.Write(value);
            return this;
        }

        public override string? ToString()
        {
            return _output?.ToString() ?? string.Empty;
        }
    }
}
