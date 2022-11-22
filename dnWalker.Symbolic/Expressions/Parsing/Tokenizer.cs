using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Parsing
{
    // should be done in no alloc manner with readonly span and other ref structs
    public class Tokenizer<TType>
            where TType : struct
    {
        public record Token(TType Type, string Value, int Offset);

        public record TokenDefinition(Regex Regex, TType Type)
        {
            public bool TryMatch(int offset, string text, [NotNullWhen(true)] out Token? match, [NotNullWhen(true)] out string? remainingText)
            {
                Match m = Regex.Match(text);
                if (m.Success)
                {
                    remainingText = string.Empty;

                    if (m.Length != text.Length)
                    {
                        remainingText = text.Substring(m.Length);
                    }

                    match = new Token(Type, m.Value, offset);
                    return true;
                }
                match = null;
                remainingText = null;
                return false;
            }
        }

        private readonly TokenDefinition[] _tokenDefinitions;

        public Tokenizer(IEnumerable<Tokenizer<TType>.TokenDefinition> tokenDefinitions)
        {
            _tokenDefinitions = tokenDefinitions.ToArray();
        }

        public IList<Token> Tokenize(string text, TType invalidToken = default(TType))
        {
            List<Token> tokens = new List<Token>();

            int offset = 0;
            while (!string.IsNullOrWhiteSpace(text))
            {
                if (TryFindMatch(offset, text, out Token? token, out string? remainingText))
                {
                    offset += token.Value.Length;
                    tokens.Add(token);
                    text = remainingText;
                }
                else if (char.IsWhiteSpace(text[0]))
                {
                    offset++;
                    text = text.Substring(1);
                }
                else
                {
                    tokens.Add(new Token(invalidToken, string.Empty, offset));
                    break;
                }
            }

            return tokens;
        }

        private bool TryFindMatch(int offset, string text, [NotNullWhen(true)]out Token? token, [NotNullWhen(true)] out string? remainingText)
        {
            foreach (TokenDefinition d in _tokenDefinitions) 
            {
                if (d.TryMatch(offset, text, out token, out remainingText)) return true;
            }

            token = null;
            remainingText = null;
            return false;
        }
    }
}
