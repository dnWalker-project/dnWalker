using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    internal class Writer : IWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private string _currentIndent = string.Empty;
        private int _indentLevel = 0;

        private bool _endsWithNewline;

        public override string ToString()
        {
            return _sb.ToString();
        }

        public void Clear()
        {
            _sb.Clear();
        }

        public int Indent 
        {
            get { return _indentLevel; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");
                if (_indentLevel != value)
                {
                    _indentLevel = value;
                    _currentIndent = GenerateIndent(value, IndentString);
                }
            }
        }

        private static string GenerateIndent(int level, string indentString)
        {
            StringBuilder sb = new StringBuilder(indentString.Length * level);

            for (int i = 0; i < level; ++i)
            {
                sb.Append(indentString);
            }

            return sb.ToString();
        }

        public string IndentString { get; set; } = "    ";
        public string NewLine { get; set; } = Environment.NewLine;


        public void Write(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (_sb.Length == 0 || _endsWithNewline)
            {
                _sb.Append(_currentIndent);
                _endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (text.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
            {
                _endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (_currentIndent.Length == 0)
            {
                _sb.Append(text);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            text = text.Replace(Environment.NewLine, Environment.NewLine + _currentIndent);
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (_endsWithNewline)
            {
                _sb.Append(text, 0, text.Length - _currentIndent.Length);
            }
            else
            {
                _sb.Append(text);
            }
        }

        public void WriteLine(string text)
        {
            Write(text);
            _sb.AppendLine();
            _endsWithNewline = true;
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }


        public void ClearIndent()
        {
            Indent= 0;
        }
    }
}
