using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public readonly struct RawCode : ICodeExpression
    {
        public RawCode(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public string Code { get; }

        public void WriteTo(StringBuilder output)
        {
            output.Append(Code);
        }

        public static implicit operator RawCode(string str)
        {
            return new RawCode(str);
        }
    }

    public readonly struct StringLiteral : ICodeExpression
    {
        public StringLiteral(string literal)
        {
            Literal = literal ?? throw new ArgumentNullException(nameof(literal));
        }

        public string Literal { get; }

        public void WriteTo(StringBuilder output)
        {
            output.AppendFormat("\"{0}\"", Literal);
        }

        public static implicit operator StringLiteral(string str)
        {
            return new StringLiteral(str);
        }
    }
}
