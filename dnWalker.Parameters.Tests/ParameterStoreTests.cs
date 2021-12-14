using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class ParameterStoreTests
    {
        [Fact]
        public void Test_AddParameter()
        {
            const int Id = 10;

            ParameterStore store = new ParameterStore();
            IParameter p = new Int32Parameter(0, Id);

            store.AddParameter(p);

            store.TryGetParameter(Id, out IParameter? p2).Should().BeTrue();
            p.Should().BeSameAs(p2);
            store.GetAllParameters().Should().Contain(p);
        }

        [Fact]
        public void Test_AddRootParmater()
        {
            const int Id = 10;
            const string RootName = "x";

            ParameterStore store = new ParameterStore();
            IParameter p = new Int32Parameter(0, Id);

            store.AddRootParameter(RootName, p);

            store.TryGetRootParameter(RootName, out IParameter? p2).Should().BeTrue();
            p.Should().BeSameAs(p2);
            store.TryGetParameter(Id, out _).Should().BeTrue();
            store.GetAllParameters().Should().Contain(p);
            store.GetRootParameters().Should().Contain(KeyValuePair.Create(RootName, p));
        }

        [Fact]
        public void Test_AddThisParameter()
        {
            const int Id = 10;

            ParameterStore store = new ParameterStore();
            IParameter p = new Int32Parameter(0, Id);

            store.AddThisParameter(p);

            store.TryGetThisParameter(out IParameter? p2).Should().BeTrue();
            p.Should().BeSameAs(p2);
            store.TryGetParameter(Id, out _).Should().BeTrue();

            store.GetAllParameters().Should().Contain(p);
            store.GetRootParameters().Should().Contain(KeyValuePair.Create(ParameterStore.ThisName, p));
        }

        [Fact]
        public void Test_AddReturnParameter()
        {
            const int Id = 10;

            ParameterStore store = new ParameterStore();
            IParameter p = new Int32Parameter(0, Id);

            store.AddReturnParameter(p);

            store.TryGetReturnParameter(out IParameter? p2).Should().BeTrue();
            p.Should().BeSameAs(p2);
            store.TryGetParameter(Id, out _).Should().BeTrue();

            store.GetAllParameters().Should().Contain(p);
            store.GetRootParameters().Should().Contain(KeyValuePair.Create(ParameterStore.ReturnName, p));
        }

        [Fact]
        public void Test_RemoveParameter()
        {
            const int Id = 10;

            ParameterStore store = new ParameterStore();
            IParameter p = new Int32Parameter(0, Id);

            store.AddParameter(p);

            store.TryGetParameter(Id, out IParameter? p2).Should().BeTrue();
            p.Should().BeSameAs(p2);
            store.GetAllParameters().Should().Contain(p);

            store.RemoveParameter(p);

            store.TryGetParameter(Id, out _).Should().BeFalse();
            store.GetAllParameters().Should().NotContain(p);
        }
    }
}
