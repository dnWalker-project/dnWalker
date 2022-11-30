using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IMethodFaker
    {
        Location SymbolicLocation { get; }
        IVariable InstanceVariable { get; }
        IMethod Method { get; }
        IDataElement GetConcreteValue(ExplicitActiveState cur, IDataElement[] args);
    }
}
