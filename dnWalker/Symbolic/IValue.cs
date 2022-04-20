using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IValue : IEquatable<IValue>
    {
        IValue Clone();
    }
}
