using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public abstract class Parameter
    {
        public string FullTypeName { get; }
        public string Name { get; }


        protected Parameter(string fullTypeName, string name)
        {
            FullTypeName = fullTypeName;
            Name = name;
        }


        public abstract void Initialize(CodeWriter codeWriter);
    }
}
