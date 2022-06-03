using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public abstract class TemplateBase : IWriter
    {
        public abstract string TransformText();
        public virtual void Initialize()
        {
            _sb.Clear();
            _errors?.Clear();
            _indentLengths.Clear();
            _currentIndent = string.Empty;
            _endsWithNewline = false;
        }

        private StringBuilder _sb = new StringBuilder();
        private System.CodeDom.Compiler.CompilerErrorCollection _errors;
        private List<int> _indentLengths = new List<int>();
        private string _currentIndent = string.Empty;
        private bool _endsWithNewline;
        private IDictionary<string, object> _session = new Dictionary<string, object>();

        protected StringBuilder GenerationEnvironment
        {
            get 
            {
                if (_sb == null) _sb = new StringBuilder();
                return _sb;
            }
            set { _sb = value; }
        }

        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return _errors;
            }
        }

        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return _currentIndent;
            }
        }

        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual IDictionary<string, object> Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        protected void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
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
            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
            {
                _endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (_currentIndent.Length == 0)
            {
                _sb.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + _currentIndent);
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (_endsWithNewline)
            {
                _sb.Append(textToAppend, 0, textToAppend.Length - _currentIndent.Length);
            }
            else
            {
                _sb.Append(textToAppend);
            }
        }

        void IWriter.Write(string text) => Write(text);

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        protected void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            _sb.AppendLine();
            _endsWithNewline = true;
        }

        void IWriter.WriteLine(string text) => WriteLine(text);

        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        protected void Write(string format, params object[] args)
        {
            Write(string.Format(CultureInfo.CurrentCulture, format, args));
        }
        void IWriter.Write(string format, params object[] args) => WriteLine(format, args);



        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        protected void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }
        void IWriter.WriteLine(string format, params object[] args) => WriteLine(format, args);

        /// <summary>
        /// Raise an error
        /// </summary>
        protected void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        protected void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        protected void PushIndent(string indent)
        {
            if (indent == null)
            {
                throw new ArgumentNullException("indent");
            }
            _currentIndent = _currentIndent + indent;
            _indentLengths.Add(indent.Length);
        }

        void IWriter.PushIndent(string indent) => PushIndent(indent);

        protected void PushIndent() => PushIndent(TemplateUtils.Indent);

        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        protected string PopIndent()
        {
            string returnValue = "";
            if (_indentLengths.Count > 0)
            {
                int indentLength = _indentLengths[_indentLengths.Count - 1];
                _indentLengths.RemoveAt(_indentLengths.Count - 1);
                if (indentLength > 0)
                {
                    returnValue = _currentIndent.Substring(_currentIndent.Length - indentLength);
                    _currentIndent = _currentIndent.Remove(_currentIndent.Length - indentLength);
                }
            }
            return returnValue;
        }

        string IWriter.PopIndent() => PopIndent();

        /// <summary>
        /// Remove any indentation
        /// </summary>
        protected void ClearIndent()
        {
            _indentLengths.Clear();
            _currentIndent = "";
        }

        void IWriter.ClearIndent() => ClearIndent();
        #endregion


        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        protected class ToStringInstanceHelper
        {
            private IFormatProvider _formatProvider = CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public IFormatProvider FormatProvider
            {
                get
                {
                    return _formatProvider;
                }
                set
                {
                    if (value != null)
                    {
                        _formatProvider = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string? ToStringWithCulture(object objectToConvert)
            {
                if (objectToConvert == null)
                {
                    throw new ArgumentNullException("objectToConvert");
                }
                Type t = objectToConvert.GetType();
                MethodInfo? method = t.GetMethod("ToString", new Type[] { typeof(IFormatProvider)});
                if (method == null)
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return (string?)method.Invoke(objectToConvert, new object[] { _formatProvider });
                }
            }
        }
        private ToStringInstanceHelper _toStringHelper = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        protected ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return _toStringHelper;
            }
        }
        #endregion ToString Helpers
    }
}
