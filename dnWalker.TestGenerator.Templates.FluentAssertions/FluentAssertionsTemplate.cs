﻿using dnlib.DotNet;

namespace dnWalker.TestGenerator.Templates.FluentAssertions
{
    public class FluentAssertionsTemplate : IAssertTemplate
    {
        private static readonly string[] _namespaces = new[] { "FluentAssertions" };
        public IEnumerable<string> Namespaces => _namespaces;

        public void WriteAssertNull(IWriter output, string symbol)
        {
            output.WriteLine($"{symbol}.Should().BeNull();");
        }

        public void WriteAssertNotNull(IWriter output, string symbol)
        {
            output.WriteLine($"{symbol}.Should().NotBeNull();");
        }

        public void WriteAssertEqual(IWriter output, string actual, string expected)
        {
            output.WriteLine($"{actual}.Should().Be({expected});");
        }

        public void WriteAssertNotEqual(IWriter output, string actual, string expected)
        {
            output.WriteLine($"{actual}.Should().NotBe({expected});");
        }

        public void WriteAssertSame(IWriter output, string actual, string expected)
        {
            output.WriteLine($"{actual}.Should().BeSameAs({expected});");
        }

        public void WriteAssertNotSame(IWriter output, string actual, string expected)
        {
            output.WriteLine($"{actual}.Should().NotBeSameAs({expected});");
        }


        public void WriteAssertExceptionThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            output.Write($"{delegateSymbol}.Should().Throw<");
            output.WriteNameOrAlias(exceptionType);
            output.WriteLine(">();");
        }
        public void WriteAssertExceptionNotThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            output.Write($"{delegateSymbol}.Should().NotThrow<");
            output.WriteNameOrAlias(exceptionType);
            output.WriteLine(">();");
        }

        public void WriteAssertNoExceptionThrown(IWriter output, string delegateSymbol)
        {
            output.WriteLine($"{delegateSymbol}.Should().NotThrow();");
        }

        public void WriteAssertLength(IWriter output, string arraySymbol, string expected)
        {
            output.WriteLine($"{arraySymbol}.Should().HaveLength({expected});");
        }

        public void WriteAssertCount(IWriter output, string collectionSymbol, string expected)
        {
            output.WriteLine($"{collectionSymbol}.Should().HaveCount({expected});");
        }

        public void WriteAssertOfType(IWriter output, string objectSymbol, TypeSig expectedType)
        {
            output.Write($"{objectSymbol}.Should().BeOfType<");
            output.WriteNameOrAlias(expectedType);
            output.WriteLine(">();");
        }

        public void WriteAssertNotOfType(IWriter output, string objectSymbol, TypeSig expectedType)
        {
            output.Write($"{objectSymbol}.Should().NotBeOfType<");
            output.WriteNameOrAlias(expectedType);
            output.WriteLine(">();");
        }
    }
}