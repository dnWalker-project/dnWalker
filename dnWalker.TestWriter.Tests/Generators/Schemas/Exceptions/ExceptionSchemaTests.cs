using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic.Variables;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;
using dnWalker.TestWriter.Generators;
using FluentAssertions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.Exceptions
{
    public class ExceptionSchemaTests : TestWriterTestBase
    {
        private class TestClass
        {
            public void Method() { }
        }

        public ExceptionSchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoExceptionThrown()
        {
            const string Expected =
            """
            // Arrange input model heap
            ExceptionSchemaTests.TestClass exceptionSchemaTests_TestClass1 = new ExceptionSchemaTests.TestClass();

            // Arrange method arguments
            ExceptionSchemaTests.TestClass @this = exceptionSchemaTests_TestClass1;

            Action method = () => @this.Method();
            Assert.Null(Record.Exception(method));
            """;

            TypeDef testClassTD = GetType(typeof(TestClass));

            MethodDef md = testClassTD.FindMethod(nameof(TestClass.Method));


            ConcolicExplorationIteration it = GetIteration(md, b =>
            {
                IModel input = new Model();
                input.SetValue((IRootVariable)Variable.MethodArgument(md.Parameters[0]), input.HeapInfo.InitializeObject(testClassTD.ToTypeSig()).Location);

                b.InputModel = input;
                IModel output = input.Clone();
                b.OutputModel = output;
            });

            ExceptionSchema schema = new ExceptionSchema(new TestContext(it));

            Writer writer = new Writer();
            schema.Write(GetTestTemplate(), writer);

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void ExceptionThrown()
        {
            const string Expected =
            """
            // Arrange input model heap
            ExceptionSchemaTests.TestClass exceptionSchemaTests_TestClass1 = new ExceptionSchemaTests.TestClass();

            // Arrange method arguments
            ExceptionSchemaTests.TestClass @this = exceptionSchemaTests_TestClass1;

            Action method = () => @this.Method();
            Assert.Throws<InvalidOperationException>(method);
            """;

            TypeDef testClassTD = GetType(typeof(TestClass));

            MethodDef md = testClassTD.FindMethod(nameof(TestClass.Method));


            ConcolicExplorationIteration it = GetIteration(md, b =>
            {
                IModel input = new Model();
                input.SetValue((IRootVariable)Variable.MethodArgument(md.Parameters[0]), input.HeapInfo.InitializeObject(testClassTD.ToTypeSig()).Location);

                b.InputModel = input;
                IModel output = input.Clone();
                b.OutputModel = output;

                b.Exception = GetType(typeof(InvalidOperationException)).ToTypeSig();
            });

            ExceptionSchema schema = new ExceptionSchema(new TestContext(it));

            Writer writer = new Writer();
            schema.Write(GetTestTemplate(), writer);

            writer.ToString().Trim().Should().Be(Expected);
        }
    }
}
