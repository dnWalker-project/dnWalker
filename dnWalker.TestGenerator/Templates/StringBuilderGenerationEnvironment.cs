using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    /// <summary>
    /// Implementation of the <see cref="IGenerationEnvironment"/> using <see cref="StringBuilder"/>.
    /// </summary>
    public class StringBuilderGenerationEnvironment : IGenerationEnvironment
    {
        private StringBuilder? _output;

        [System.Diagnostics.CodeAnalysis.MemberNotNull("_output")]
        private void EnsureOutput()
        { 
            if (_output == null) _output = new StringBuilder(); 
        }

        public IGenerationEnvironment Append<T>(T value) where T : struct
        {
            EnsureOutput();
            _output.Append(value.ToString());
            return this;
        }

        public IGenerationEnvironment Append(string value)
        {
            EnsureOutput();
            _output?.Append(value);
            return this;
        }

        public IGenerationEnvironment AppendLine(string value)
        {
            EnsureOutput();
            _output.AppendLine(value);
            return this;
        }

        public IGenerationEnvironment AppendFormat(IFormatProvider? provider, string format, params object[] args)
        {
            EnsureOutput();
            _output.AppendFormat(provider, format, args);
            return this;
        }

        public IGenerationEnvironment Clear()
        {
            if (_output != null) _output.Clear();
            return this;
        }

        public IGenerationEnvironment Append(char value, int repeatCount)
        {
            EnsureOutput();
            _output.Append(value, repeatCount);
            return this;
        }

        public IGenerationEnvironment Append(ReadOnlySpan<char> value)
        {
            EnsureOutput();
            _output.Append(value);
            return this;
        }

        public IGenerationEnvironment Append(ReadOnlyMemory<char> value)
        {
            EnsureOutput();
            _output.Append(value);
            return this;
        }

        public override string? ToString()
        {
            return _output?.ToString() ?? string.Empty;
        }

        public int Length
        {
            get
            {
                return _output?.Length ?? 0;
            }
        }
    }
}
