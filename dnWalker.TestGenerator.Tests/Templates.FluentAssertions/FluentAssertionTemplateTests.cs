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
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNull(output, "result");

            output.ToString().Trim().Should().Be("result.Should().BeNull();");
        }

        [Fact]
        public void AssertNotNull()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotNull(output, "result");

            output.ToString().Trim().Should().Be("result.Should().NotBeNull();");
        }

        [Fact]
        public void AssertEqual()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertEqual(output, "result", "5");

            output.ToString().Trim().Should().Be("result.Should().Be(5);");
        }

        [Fact]
        public void AssertNotEqual()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotEqual(output, "result", "5");

            output.ToString().Trim().Should().Be("result.Should().NotBe(5);");
        }

        [Fact]
        public void AssertSame()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertSame(output, "result", "instance");

            output.ToString().Trim().Should().Be("result.Should().BeSameAs(instance);");
        }

        [Fact]
        public void AssertNotSame()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNotSame(output, "result", "instance");

            output.ToString().Trim().Should().Be("result.Should().NotBeSameAs(instance);");
        }

        [Fact]
        public void AssertExceptionThrown()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertExceptionThrown(output, "method", TestUtils.DefinitionProvider.GetTypeDefinition("System.NullReferenceException").ToTypeSig());

            output.ToString().Trim().Should().Be("method.Should().Throw<NullReferenceException>();");
        }

        [Fact]
        public void AssertExceptionNotThrown()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertExceptionNotThrown(output, "method", TestUtils.DefinitionProvider.GetTypeDefinition("System.NullReferenceException").ToTypeSig());

            output.ToString().Trim().Should().Be("method.Should().NotThrow<NullReferenceException>();");
        }

        [Fact]
        public void AssertNoExceptionThrow()
        {
            DummyWriter output = new DummyWriter();
            FluentAssertionsTemplate template = new FluentAssertionsTemplate();

            template.WriteAssertNoExceptionThrown(output, "method");

            output.ToString().Trim().Should().Be("method.Should().NotThrow();");
        }
    }
}

