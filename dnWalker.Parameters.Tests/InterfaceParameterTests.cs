using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Tests
{
    public class InterfaceParameterTests : MethodResolverParameterTests<InterfaceParameter>
    {
        protected override InterfaceParameter Create(int id)
        {
            return new InterfaceParameter(typeof(IMyInterface).FullName!, id);
        }


    }
}
