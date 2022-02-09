using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteVariableDeclaration(TypeSignature type, string name)
        {
            if (type == TypeSignature.Empty)
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
