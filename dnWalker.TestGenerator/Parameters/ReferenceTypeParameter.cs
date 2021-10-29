using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter
    {
        protected ReferenceTypeParameter(string fullTypeName, string name) : base(fullTypeName, name)
        {
        }

        public Boolean IsNull 
        {
            get; 
            set; 
        }
    }
}
