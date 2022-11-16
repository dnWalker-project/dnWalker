using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{ 
    public interface IWriter
    {
        void Write(string text);
        void WriteLine(string text);

        void Write(string format, params object[] args);
        void WriteLine(string format, params object[] args);

        int Indent { get; set; }

        string IndentString { get; set; }

        void ClearIndent();
    }
}
