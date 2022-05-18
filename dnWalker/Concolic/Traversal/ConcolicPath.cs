using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Traversal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class ConcolicPath : Path
    {
        public SymbolicContext SymbolicContext
        {
            get;
            set;
        }

        public IList<ConstraintNode> ConstraintNodes { get; } = new List<ConstraintNode>();
    }
}
