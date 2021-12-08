using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class AliasParameter : Parameter, IAliasParameter
    {
        public AliasParameter(IParameter referencedParameter, string typeName) : base(typeName)
        {
            ReferencedParameter = referencedParameter ?? throw new ArgumentNullException(nameof(referencedParameter));
        }

        public AliasParameter(IParameter referencedParameter, string typeName, int id) : base(typeName, id)
        {
            ReferencedParameter = referencedParameter ?? throw new ArgumentNullException(nameof(referencedParameter));
        }

        public IParameter ReferencedParameter
        {
            get;
        }

        public override IEnumerable<IParameter> GetChildren()
        {
            return Enumerable.Empty<IParameter>();
        }
    }
}
