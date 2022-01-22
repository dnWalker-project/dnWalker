
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class RuntimeTemplateBase : IRuntimeTemplate
    {
        private bool _endsWithNewline = true;
        private string _currentIndent = string.Empty;
        private ToStringInstanceHelper? _toStringHelper;
        private readonly Stack<int> _indentLengths = new Stack<int>();

        public IDictionary<string, object> Session
        {
            get; set;
        } = new Dictionary<string, object>();

        public CompilerErrorCollection Errors
        {
            get;
        } = new CompilerErrorCollection();

        public virtual IGenerationEnvironment GenerationEnvironment
        {
            get;
            set;
        } = new StringBuilderGenerationEnvironment();

        public virtual string TransformText()
        {
            return GenerationEnvironment?.ToString() ?? string.Empty;
        }

        
        public void Initialize()
        {

        }

        public void Warning(string message)
        {
            CompilerError error = new CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            Errors.Add(error);
        }

        public void Error(string message)
        {
            CompilerError error = new CompilerError();
            error.ErrorText = message;
            Errors.Add(error);
        }

        public string CurrentIndent
        {
            get
            {
                return _currentIndent;
            }
        }

        public void PushIndent(string indent)
        {
            if (indent == null)
            {
                throw new ArgumentNullException(nameof(indent));
            }
            _currentIndent = _currentIndent + indent;
            _indentLengths.Push(indent.Length);
        }

        public string PopIndent()
        {
            string returnValue = "";
            if (_indentLengths.Count > 0)
            {
                int indentLength = _indentLengths.Pop();
                if (indentLength > 0)
                {
                    returnValue = _currentIndent.Substring(_currentIndent.Length - indentLength);
                    _currentIndent = _currentIndent.Remove(_currentIndent.Length - indentLength);
                }
            }
            return returnValue;
        }

        public void ClearIndent()
        {
            _indentLengths.Clear();
            _currentIndent = string.Empty;
        }

        private void StartNewLine()
        {
            GenerationEnvironment.Append(_currentIndent);
            _endsWithNewline = false;
        }
        private void FinishLine()
        {
            _endsWithNewline = true;
        }

        private string DoIndent(string original)
        {
            return original.Replace(Environment.NewLine, Environment.NewLine + _currentIndent);
        }

        private bool IsAtLineEnd
        {
            get
            {
                return _endsWithNewline;
            }
        }
        private bool HasIndent
        {
            get
            {
                return _currentIndent.Length > 0;
            }
        }

        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (IsAtLineEnd)
            {
                StartNewLine();
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
            {
                FinishLine();
            }

            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (!HasIndent)
            {
                GenerationEnvironment.Append(textToAppend);
                return;
            }

            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = DoIndent(textToAppend);
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (_endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend.AsSpan(0, textToAppend.Length - _currentIndent.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }

        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            FinishLine();
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                if (_toStringHelper == null)
                {
                    _toStringHelper = new ToStringInstanceHelper();
                }
                return _toStringHelper;
            }
        }
    }
}
