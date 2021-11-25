using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ArrayParameterTests : ReferenceTypeParameterTests<ArrayParameter>
    {
        protected override ArrayParameter Create(string name = "p")
        {
            return new ArrayParameter("System.Int32", name);
        }

    }
}
