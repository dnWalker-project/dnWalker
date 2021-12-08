using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class InterfaceParameter : MethodResolverParameter, IInterfaceParameter
    {
        public InterfaceParameter(string typeName) : base(typeName)
        {
        }

        public InterfaceParameter(string typeName, int id) : base(typeName, id)
        {
        }



        public override IEnumerable<IParameter> GetChildren()
        {
            return GetMethodResults().SelectMany(mr => mr.Value).Where(mr => mr != null).Select(mr => mr!);
        }
    }
}
