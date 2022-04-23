using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public readonly struct Valuation
    {
        public readonly IVariable Variable;
        public readonly IValue Value;

        public Valuation(IVariable variable, IValue value)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
