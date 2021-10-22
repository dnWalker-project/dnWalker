using dnlib.DotNet;

using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Parameter = dnWalker.Concolic.Parameters.Parameter;

namespace dnWalker.Tests.Concolic.Parameters
{

    public class ParameterStoreTests : dnlibTypeTestBase
    {
        [Fact]
        public void Adding_NamelessParameter_Should_Throw()
        {
            var store = new ParameterStore();
            Parameter p = new Int32Parameter();

            Assert.Throws<InvalidOperationException>(() => store.AddParameter(p));
        }

        [Fact]
        public void Can_Retrive_AddedParameter()
        {
            const string name = "x";

            var store = new ParameterStore();
            Parameter p = new Int32Parameter(name);

            store.AddParameter(p);

            store.TryGetParameter(name, out var p2).Should().BeTrue();
            p2.Should().BeSameAs(p);
        }

        private static Dictionary<string, object> ConstructData(string[] traitNames, object[] traitValues)
        {
            return Enumerable.Range(0, traitNames.Length)
                .ToDictionary(i => traitNames[i], i => traitValues[i]);
        }

        [Theory]
        [InlineData(new [] { typeof(MyItem) }, new[] { "item" }, new[] { "item:#__IS_NULL__", "item:_id" }, new object[] { false, -5 })]
        public void Test_SetTraits(Type[] rootParameterTypes, string[] rootParameterNames, string[] traitNames, object[] traitValues)
        {
            var data = ConstructData(traitNames, traitValues);

            var store = new ParameterStore();

            store.InitializeRootParameters(rootParameterTypes.Select(t => GetType(t)).ToArray(), rootParameterNames);

            store.SetTraits(DefinitionProvider, data);
        }

        [Fact]
        public void Test_SetTraits_InterfaceAsRootParameter()
        {
            var data = new Dictionary<string, object>()
            {
                ["myInterface:#__IS_NULL__"] = false,
                ["myInterface:get_Count|1"] = 2,
                ["myInterface:get_Count|2"] = 5,
                ["myInterface:GetMyClass|1:#__IS_NULL__"] = true,
            };

            var pTypes = new[] { GetType(typeof(IMyInterface)) };
            var pNames = new[] { "myInterface" };

            var store = new ParameterStore();
            store.InitializeRootParameters(pTypes, pNames);

            store.SetTraits(DefinitionProvider, data);

            store.TryGetParameter("myInterface:GetMyClass|1", out var p);
            p.Should().BeOfType<ObjectParameter>();

            var op = (ObjectParameter)p;
            op.IsNull.Should().BeTrue();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(null)]
        public void Test_SetTraits_PrimitiveArrayAsRootParameter_Setting_Length(int? length)
        {
            var data = new Dictionary<string, object>()
            {
                ["iArray:#__LENGTH__"] = length
            };

            var pTypes = new[] { GetType(typeof(int[])) };
            var pNames = new[] { "iArray" };

            var store = new ParameterStore();
            store.InitializeRootParameters(pTypes, pNames);

            store.SetTraits(DefinitionProvider, data);

            store.TryGetParameter("iArray", out var p).Should().BeTrue();
            p.Should().BeOfType<ArrayParameter>();

            var ap = (ArrayParameter)p;
            ap.Length.Should().Be(length);
        }

        [Theory]
        [InlineData(0, 5)]
        public void Test_SetTraits_PrimitiveArrayAsRootParameter_Setting_Items(int index, int item)
        {
            var data = new Dictionary<string, object>()
            {
                //["iArray:#__LENGTH__"] = length
                ["iArray:" + index] = item
            };

            var pTypes = new[] { GetType(typeof(int[])) };
            var pNames = new[] { "iArray" };

            var store = new ParameterStore();
            store.InitializeRootParameters(pTypes, pNames);

            store.SetTraits(DefinitionProvider, data);

            store.TryGetParameter("iArray", out var p).Should().BeTrue();
            p.Should().BeOfType<ArrayParameter>();

            var ap = (ArrayParameter)p;
            ap.TryGetItemAt(index, out var itemP).Should().BeTrue();
            itemP.Should().BeOfType<Int32Parameter>();

            var ip = (Int32Parameter)itemP;
            ip.Value.Should().Be(item);
        }
    }
}
