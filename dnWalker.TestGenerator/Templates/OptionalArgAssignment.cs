using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public readonly struct OptionalArgAssignment : ICodeExpression
    {
        public OptionalArgAssignment(string argument, ICodeExpression value)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException($"'{nameof(argument)}' cannot be null or whitespace.", nameof(argument));
            }

            Argument = argument;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Argument { get; }
        public ICodeExpression Value { get; }

        public void WriteTo(StringBuilder output)
        {
            output.AppendFormat("{0}: ", Argument);
            Value.WriteTo(output);
        }
    }
}
