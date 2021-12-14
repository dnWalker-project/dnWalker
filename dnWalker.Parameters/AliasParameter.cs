using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class AliasParameter : Parameter, IAliasParameter
    {
        public AliasParameter(IReferenceTypeParameter referencedParameter, string typeName) : base(typeName)
        {
            ReferencedParameter = referencedParameter ?? throw new ArgumentNullException(nameof(referencedParameter));
        }

        public AliasParameter(IReferenceTypeParameter referencedParameter, string typeName, int id) : base(typeName, id)
        {
            ReferencedParameter = referencedParameter ?? throw new ArgumentNullException(nameof(referencedParameter));
        }

        public IReferenceTypeParameter ReferencedParameter
        {
            get;
            set;
        }

        public override IEnumerable<IParameter> GetChildren()
        {
            return ReferencedParameter.GetChildren();
        }

        public override IParameter ShallowCopy(ParameterStore store, int id)
        {
            return this.GetReferencedParameter().CreateAlias(store, id);
        }

        public bool? IsNull
        {
            get
            {
                return ReferencedParameter.IsNull;
            }

            set
            {
                // ??
                ReferencedParameter.IsNull = value;
            }
        }

        //private readonly HashSet<IAliasParameter> _aliases = new HashSet<IAliasParameter>();

        //public ICollection<IAliasParameter> Aliases
        //{
        //    get
        //    {
        //        return _aliases;
        //    }
        //}
    }
}
