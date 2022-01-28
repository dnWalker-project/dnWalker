using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteVariableDeclaration(Type type, string name)
        {
            if (type == null)
            {
                Write("var");
            }
            else
            {
                WriteTypeName(type);
            }

            Write(TemplateHelpers.WhiteSpace);
            Write(name);
        }
    }
}
