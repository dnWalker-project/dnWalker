using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.Exceptions
{
    public class ExceptionSchemaProviderTests : TestWriterTestBase
    {
        private class TestClass
        {
            public void NoReturnValue()
            {
            }
        }

        public ExceptionSchemaProviderTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoExceptionThrown()
        {
            ExceptionSchemaProvider provider = new ExceptionSchemaProvider();

            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)));

            provider.GetSchemas(iteration).Should().HaveCount(1);
        }

        [Fact]
        public void ExceptionThrown()
        {
            ExceptionSchemaProvider provider = new ExceptionSchemaProvider();

            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)), it => it.Exception = GetType(typeof(Exception)).ToTypeSig());

            provider.GetSchemas(iteration).Should().HaveCount(1);
        }
    }
}
