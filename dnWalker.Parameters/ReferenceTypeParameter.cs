using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter, IReferenceTypeParameter
    {
        protected ReferenceTypeParameter(string typeName) : base(typeName)
        {
        }

        protected ReferenceTypeParameter(string typeName, int id) : base(typeName, id)
        {
        }

        public bool? IsNull
        {
            get;
            set;
        } = null;


        //private readonly HashSet<IAliasParameter> _aliases = new HashSet<IAliasParameter>();

        //public ICollection<IAliasParameter> Aliases
        //{
        //    get { return _aliases; }
        //}
    }
}
