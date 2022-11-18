using dnlib.DotNet;

using dnWalker.TestUtils;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Tests.Generators.Schemas;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Act
{
    file class TestClass
    {
        public static int StaticMethod(int x, int y) => 0;
        public int InstanceMethod(int x, int y) => 0;
    }

    public class SimpleActPrimitivesTests : TestSchemaTestBase
    {
        public SimpleActPrimitivesTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void TestActStaticNoReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.StaticMethod));

            string expected =
            $"""
            {method.DeclaringType!.Name}.StaticMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteAct(GetTestContext(method), writer, null).Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActStaticWithReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.StaticMethod));

            string expected =
            $"""
            int result = {method.DeclaringType!.Name}.StaticMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteAct(GetTestContext(method), writer, "result").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActInstanceNoReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.InstanceMethod));

            string expected =
            $"""
            @this.InstanceMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteAct(GetTestContext(method), writer, null).Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActInstanceWithReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.InstanceMethod));

            string expected =
            $"""
            int result = @this.InstanceMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteAct(GetTestContext(method), writer, "result").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActDelegateStaticNoReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.StaticMethod));

            string expected =
            $$"""
            Action act = () => {{method.DeclaringType!.Name}}.StaticMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteActDelegate(GetTestContext(method), writer, null, "act").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActDelegateStaticWithReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.StaticMethod));

            string expected =
            $$"""
            int result = 0;
            Action act = () => { result = {{ method.DeclaringType!.Name }}.StaticMethod(x, y); };
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteActDelegate(GetTestContext(method), writer, "result", "act").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActDelegateInstanceNoReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.InstanceMethod));

            string expected =
            """
            Action act = () => @this.InstanceMethod(x, y);
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteActDelegate(GetTestContext(method), writer, null, "act").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }

        [Fact]
        public void TestActDelegateInstanceWithReturnSymbol()
        {
            MethodDef method = GetMethod(typeof(TestClass), nameof(TestClass.InstanceMethod));

            string expected =
            """
            int result = 0;
            Action act = () => { result = @this.InstanceMethod(x, y); };
            """;

            SimpleActWriter actWriter = new SimpleActWriter();
            Writer writer = new Writer();

            actWriter.TryWriteActDelegate(GetTestContext(method), writer, "result", "act").Should().BeTrue();

            writer.ToString().Trim().Should().Be(expected);
        }
    }
}
