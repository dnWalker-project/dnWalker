using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas.ReturnValue;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.ReturnValue
{


    public class ReturnValueSchemaProviderTests : TestSchemaTestBase
    {
        private class TestClass
        {
            public void NoReturnValue()
            {

            }

            public int WithReturnValue() => -1;
        }

        public ReturnValueSchemaProviderTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoReturnValue()
        {
            ReturnValueSchemaProvider provider = new ReturnValueSchemaProvider();

            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)));

            provider.GetSchemas(iteration).Should().BeEmpty();
        }

        [Fact]
        public void ThrowException()
        {
            ReturnValueSchemaProvider provider = new ReturnValueSchemaProvider();

            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.WithReturnValue)), it => it.Exception = GetType(typeof(Exception)).ToTypeSig());

            provider.GetSchemas(iteration).Should().BeEmpty();
        }

        [Fact]
        public void HasReturnValue()
        {
            ReturnValueSchemaProvider provider = new ReturnValueSchemaProvider();

            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.WithReturnValue)));

            provider.GetSchemas(iteration).Should().HaveCount(1);
        }
    }
}
