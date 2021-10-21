using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ArrayParameterTests : ReferenceTypeParameterTests<ArrayParameter>
    {
        protected override ArrayParameter Create(String name = "p")
        {
            return new ArrayParameter("System.Int32", name);
        }


    }
}
