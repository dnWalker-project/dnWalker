using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IVariable
    {
        TypeSig Type { get; }
        string Name { get; }
    }

    public interface IRootVariable : IVariable
    {
    }

    public interface IMemberVariable : IVariable
    {
        IVariable Parent { get; }

        bool IsSameMemberAs(IMemberVariable other);
    }
}
