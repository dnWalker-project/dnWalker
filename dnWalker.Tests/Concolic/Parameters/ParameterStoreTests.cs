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

    public class ParameterStoreTests : dnlibTypeTestBase
    {
        [Fact]
        public void Adding_NamelessParameter_Should_Throw()
        {
            ParameterStore store = new ParameterStore();
            Parameter p = new Int32Parameter();

            Assert.Throws<InvalidOperationException>(() => store.AddParameter(p));
        }

        [Fact]
        public void Can_Retrive_AddedParameter()
        {
            const String name = "x";

            ParameterStore store = new ParameterStore();
            Parameter p = new Int32Parameter(name);

            store.AddParameter(p);

            store.TryGetParameter(name, out Parameter p2).Should().BeTrue();
            p2.Should().BeSameAs(p);
        }
    }
}
