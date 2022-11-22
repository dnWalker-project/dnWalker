using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IVariable : IModelVisitable, IEquatable<IVariable>
    {
        void IModelVisitable.Accept(dnWalker.Symbolic.IModelVisitor visitor)
        {
            visitor.VisitVariable(this);
        }

        IVariable Substitute(IVariable from, IVariable to);

        TypeSig Type { get; }
        string Name { get; }
    }

    public interface IRootVariable : IVariable
    {
        IVariable IVariable.Substitute(IVariable from, IVariable to)
        {
            // as a root variable, we cannot substitute any variable up the chain...
            if (this.Equals(from)) return to;
            return this;
        }
    }

    public interface IMemberVariable : IVariable
    {
        IVariable Parent { get; }

        bool IsSameMemberAs(IMemberVariable other);
    }
}
