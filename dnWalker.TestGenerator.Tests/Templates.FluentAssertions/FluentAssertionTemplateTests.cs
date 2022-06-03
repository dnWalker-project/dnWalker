using dnlib.DotNet;

using dnWalker.TestGenerator.Templates.FluentAssertions;

using FluentAssertions;

using System;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates.FluentAssertions
{
    public class FluentAssertionTemplateTests
    {
        [Fact]
        public void AssertNull()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNull(output, "result");

            output.ToString().Should().Be("result.Should().BeNull();");
        }

        [Fact]
        public void AssertNotNull()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotNull(output, "result");

            output.ToString().Should().Be("result.Should().NotBeNull();");
        }

        [Fact]
        public void AssertEqual()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertEqual(output, "result", "5");

            output.ToString().Should().Be("result.Should().Be(5);");
        }

        [Fact]
        public void AssertNotEqual()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotEqual(output, "result", "5");

            output.ToString().Should().Be("result.Should().NotBe(5);");
        }

        [Fact]
        public void AssertSame()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertSame(output, "result", "instance");

            output.ToString().Should().Be("result.Should().BeSameAs(instance);");
        }

        [Fact]
        public void AssertNotSame()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotSame(output, "result", "instance");

            output.ToString().Should().Be("result.Should().NotBeSameAs(instance);");
        }

        [Fact]
        public void AssertExceptionThrown()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertExceptionThrown(output, "method", TestUtils.DefinitionProvider.GetTypeDefinition("System.NullReferenceException").ToTypeSig());

            output.ToString().Should().Be("method.Should().Throw<NullReferenceException>();");
        }

        [Fact]
        public void AssertExceptionNotThrown()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertExceptionNotThrown(output, "method", TestUtils.DefinitionProvider.GetTypeDefinition("System.NullReferenceException").ToTypeSig());

            output.ToString().Should().Be("method.Should().NotThrow<NullReferenceException>();");
        }

        [Fact]
        public void AssertNoExceptionThrow()
        {
            TestWriter output = new TestWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNoExceptionThrown(output, "method");

            output.ToString().Should().Be("method.Should().NotThrow();");
        }
    }
}

