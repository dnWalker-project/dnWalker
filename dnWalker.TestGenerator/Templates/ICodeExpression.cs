using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public interface ICodeExpression
    {
        void WriteTo(StringBuilder output);
    }
}
