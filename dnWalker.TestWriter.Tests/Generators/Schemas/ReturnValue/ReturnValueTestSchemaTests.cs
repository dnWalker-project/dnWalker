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
    file class TestClass
    {
        public int ReturnInteger() { return 1; }
        public static int StaticReturnInteger() { return 1; }


        public string ReturnString() { return "Hello world"; }
        public static string StaticReturnString() { return "Hello world"; }
    }

    public class ReturnValueTestSchemaTests : TestSchemaTestBase
    {
        public ReturnValueTestSchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoArrange()
        {
            TypeDef testClassTD = GetType(typeof(TestClass));

            string expected =
            $"""
            {testClassTD.Name} @this = new {testClassTD.Name}();

            int result = @this.ReturnInteger();

            Assert.Equal(result, 0);
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
            schema.Write(TestTemplate, writer);

            writer.ToString().Trim().Should().Be(expected);
        }
    }
}
