using System;
using System.Text;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.Tests
{
    internal class DummyWriter : TestGenerator.Templates.IWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private string _currentIndent = string.Empty;
        private Stack<int> _indentLengths = new Stack<int>();
        private bool _pendingIndent = false;

        private void WriteIndent()
        {
            if (_pendingIndent)
            {
                _pendingIndent = false;
                _sb.Append(_currentIndent);
            }
        }

        private string PreprocessText(string text)
        {
            string newLineAndIndent = Environment.NewLine + _currentIndent;
            return text.Replace(Environment.NewLine, newLineAndIndent);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }


        public string CurrentIndent => _currentIndent;

        public void ClearIndent()
        {
            _currentIndent = string.Empty;
            _indentLengths.Clear();
        }

        public string PopIndent()
        {
            if (_indentLengths.TryPop(out int length))
            {
                string popped = _currentIndent.Substring(_currentIndent.Length - length);
                _currentIndent = _currentIndent.Substring(0, length);
                return popped;
            }
            return string.Empty;
        }

        public void PushIndent(string indent)
        {
            _indentLengths.Push(indent.Length);
            _currentIndent = _currentIndent + indent;
        }

        public void Write(string text)
        {
            WriteIndent();
            text = PreprocessText(text);
            _sb.Append(text);
        }

        public void Write(string format, params object[] args)
        {
            WriteIndent();
            string text = PreprocessText(string.Format(format, args));
            _sb.Append(text);
        }

        public void WriteLine(string text)
        {
            WriteIndent();
            text = PreprocessText(text);
            _sb.AppendLine(text);
            _pendingIndent = true;

        }

        public void WriteLine(string format, params object[] args)
        {
            WriteIndent();
            string text = PreprocessText(string.Format(format, args));
            _sb.AppendLine(text);
            _pendingIndent = true;
        }
    }
}

