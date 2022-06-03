using dnlib.DotNet;

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

        public void WriteAssertEqual(IWriter output, string leftSymbol, string rightSymbol)
        {
            output.WriteLine($"{leftSymbol}.Should().Be({rightSymbol});");
        }

        public void WriteAssertNotEqual(IWriter output, string leftSymbol, string rightSymbol)
        {
            output.WriteLine($"{leftSymbol}.Should().NotBe({rightSymbol});");
        }

        public void WriteAssertSame(IWriter output, string leftSymbol, string rightSymbol)
        {
            output.WriteLine($"{leftSymbol}.Should().BeSameAs({rightSymbol});");
        }

        public void WriteAssertNotSame(IWriter output, string leftSymbol, string rightSymbol)
        {
            output.WriteLine($"{leftSymbol}.Should().NotBeSameAs({rightSymbol});");
        }


        public void WriteAssertExceptionThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            output.Write($"{delegateSymbol}.Should().Throw<");
            output.WriteFullName(exceptionType);
            output.WriteLine(">();");
        }
        public void WriteAssertExceptionNotThrown(IWriter output, string delegateSymbol, TypeSig exceptionType)
        {
            output.Write($"{delegateSymbol}.Should().NotThrow<");
            output.WriteFullName(exceptionType);
            output.WriteLine(">();");
        }

        public void WriteAssertNoExceptionThrown(IWriter output, string delegateSymbol)
        {
            output.WriteLine($"{delegateSymbol}.Should().NotThrow();");
        }
    }
}