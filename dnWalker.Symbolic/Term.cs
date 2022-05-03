using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public abstract class Term
    {
        public abstract void GetVariables(ICollection<IVariable> variables);
    }
}
