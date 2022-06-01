using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestSchemas
{
    public abstract class TestSchema
    {
        public abstract void Write(IWriter output, TestWriter writer);
    }
}
