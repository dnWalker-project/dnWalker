using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.Generators;
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

    public class ReturnValueTestSchemaTests : TestWriterTestBase
    {
        private class TestClass
        {
            public int ReturnInteger() { return 1; }
            public static int StaticReturnInteger() { return 1; }


            public string? ReturnString() { return "Hello world"; }
            public static string? StaticReturnString() { return "Hello world"; }


            public TestClass? ReturnRefType() { return this; }
            public static TestClass? StaticReturnRefType() { return null; }
        }

        public ReturnValueTestSchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void TrivialSUTArrangeAndPrimitiveResult()
        {
            TypeDef testClassTD = GetType(typeof(TestClass));

            const string Expected =
            """
            // Arrange input model heap
            ReturnValueTestSchemaTests.TestClass returnValueTestSchemaTests_TestClass1 = new ReturnValueTestSchemaTests.TestClass();

            // Arrange method arguments
            ReturnValueTestSchemaTests.TestClass @this = returnValueTestSchemaTests_TestClass1;

            int result = @this.ReturnInteger();
            Assert.Equal(0, result);
            """;

            MethodDef md = testClassTD.FindMethod(nameof(TestClass.ReturnInteger));

            ConcolicExplorationIteration it = GetIteration(md, b =>
            {
                IModel input = new Model();
                input.SetValue((IRootVariable)Variable.MethodArgument(md.Parameters[0]), input.HeapInfo.InitializeObject(testClassTD.ToTypeSig()).Location);

                b.InputModel= input;
                IModel output = input.Clone();
                output.SetValue(new ReturnValueVariable(md), ValueFactory.GetValue(0));

                b.OutputModel = output;
            });

            ReturnValueSchema schema = new ReturnValueSchema(new TestContext(it));

            Writer writer = new Writer();
            schema.Write(GetTestTemplate(), writer);

            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TrivialSUTArrangeAndNullResult()
        {
            TypeDef testClassTD = GetType(typeof(TestClass));

            const string Expected =
            """
            // Arrange input model heap
            ReturnValueTestSchemaTests.TestClass returnValueTestSchemaTests_TestClass1 = new ReturnValueTestSchemaTests.TestClass();

            // Arrange method arguments
            ReturnValueTestSchemaTests.TestClass @this = returnValueTestSchemaTests_TestClass1;

            ReturnValueTestSchemaTests.TestClass result = @this.ReturnRefType();
            Assert.Null(result);
            """;

            MethodDef md = testClassTD.FindMethod(nameof(TestClass.ReturnRefType));

            ConcolicExplorationIteration it = GetIteration(md, b =>
            {
                IModel input = new Model();
                input.SetValue((IRootVariable)Variable.MethodArgument(md.Parameters[0]), input.HeapInfo.InitializeObject(testClassTD.ToTypeSig()).Location);

                b.InputModel = input;
                IModel output = input.Clone();
                output.SetValue(new ReturnValueVariable(md), Location.Null);

                b.OutputModel = output;
            });

            ReturnValueSchema schema = new ReturnValueSchema(new TestContext(it));

            Writer writer = new Writer();
            schema.Write(GetTestTemplate(), writer);

            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TrivialSUTArrangeAndNonNullResult()
        {
            TypeDef testClassTD = GetType(typeof(TestClass));

            const string Expected =
            """
            // Arrange input model heap
            ReturnValueTestSchemaTests.TestClass returnValueTestSchemaTests_TestClass1 = new ReturnValueTestSchemaTests.TestClass();

            // Arrange method arguments
            ReturnValueTestSchemaTests.TestClass @this = returnValueTestSchemaTests_TestClass1;

            ReturnValueTestSchemaTests.TestClass result = @this.ReturnRefType();
            Assert.NotNull(result);
            Assert.Same(returnValueTestSchemaTests_TestClass1, result);
            """;

            MethodDef md = testClassTD.FindMethod(nameof(TestClass.ReturnRefType));

            ConcolicExplorationIteration it = GetIteration(md, b =>
            {
                IModel input = new Model();
                Location thisLocation = input.HeapInfo.InitializeObject(testClassTD.ToTypeSig()).Location;

                input.SetValue((IRootVariable)Variable.MethodArgument(md.Parameters[0]), thisLocation);

                b.InputModel = input;
                IModel output = input.Clone();
                output.SetValue(new ReturnValueVariable(md), thisLocation);

                b.OutputModel = output;
            });

            ReturnValueSchema schema = new ReturnValueSchema(new TestContext(it));

            Writer writer = new Writer();
            schema.Write(GetTestTemplate(), writer);

            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }
    }
}
