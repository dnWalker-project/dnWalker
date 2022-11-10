using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public interface IWriter
    {
        void Write(string text);
        void WriteLine(string text);

        void Write(string format, params object[] args);
        void WriteLine(string format, params object[] args);

        void PushIndent(string indent);
        string PopIndent();

        void ClearIndent();

        string CurrentIndent { get; }
    }
}
