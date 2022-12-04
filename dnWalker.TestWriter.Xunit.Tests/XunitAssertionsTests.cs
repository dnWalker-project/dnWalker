using dnWalker.TestWriter.Generators;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Xunit.Tests
{
    public class XunitAssertionsTests
    {
        [Fact]
        public void TestAssertNull()
        {
            const string Expected =
            """
            Assert.Null(obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNull(null, writer, "obj").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertNotNull()
        {
            const string Expected =
            """
            Assert.NotNull(obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNotNull(null, writer, "obj").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertEqual()
        {
            const string Expected =
            """
            Assert.Equal(5, obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertEqual(null, writer, "obj", "5").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertNotEqual()
        {
            const string Expected =
            """
            Assert.NotEqual(5, obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNotEqual(null, writer, "obj", "5").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertSame()
        {
            const string Expected =
            """
            Assert.Same(obj2, obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertSame(null, writer, "obj", "obj2").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertNotSame()
        {
            const string Expected =
            """
            Assert.NotSame(obj2, obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNotSame(null, writer, "obj", "obj2").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestExceptionThrown()
        {
            const string Expected =
            """
            Assert.Throws<MyException>(act);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertExceptionThrown(null, writer, "act", "MyException").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestExceptionNotThrown()
        {
            const string Expected =
            """
            Assert.IsNotType<MyException>(Record.Exception(act));
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertExceptionNotThrown(null, writer, "act", "MyException").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestNoExceptionThrown()
        {
            const string Expected =
            """
            Assert.Null(Record.Exception(act));
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNoExceptionThrown(null, writer, "act").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertOfType()
        {
            const string Expected =
            """
            Assert.IsType<MyType>(obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertOfType(null, writer, "obj", "MyType").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestAssertNotOfType()
        {
            const string Expected =
            """
            Assert.IsNotType<MyType>(obj);
            """;

            Writer writer = new Writer();

            XunitAssertions xunit = new XunitAssertions();
            xunit.TryWriteAssertNotOfType(null, writer, "obj", "MyType").Should().BeTrue();

            writer.ToString().Trim().Should().Be(Expected);
        }
    }
}
