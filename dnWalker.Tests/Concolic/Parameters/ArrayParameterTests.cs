using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ArrayParameterTests : ReferenceTypeParameterTests<ArrayParameter>
    {
        protected override ArrayParameter Create(string name = "p")
        {
            return new ArrayParameter("System.Int32", name);
        }



        [Fact]
        public void ChangingName_Changes_LengthParameter_Name()
        {
            var parameter = new ArrayParameter("System.Int32", "p");

            parameter.Name = "new_name";

            parameter.LengthParameter.Name.Should().BeEquivalentTo(ParameterName.ConstructField("new_name", ArrayParameter.LengthParameterName));
        }
    }
}
