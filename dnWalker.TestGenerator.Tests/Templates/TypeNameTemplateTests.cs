using dnWalker.TestGenerator.Templates;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class TypeNameTemplateTests
    {
        [Theory]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(ushort), "ushort")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(string), "string")]
        public void BuildIns(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(UriFormat), "UriFormat")]
        [InlineData(typeof(ResolveEventArgs), "ResolveEventArgs")]
        public void NormalNonGenericTypes(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(int[]), "int[]")]
        [InlineData(typeof(ResolveEventArgs[]), "ResolveEventArgs[]")]
        public void ArrayTypes(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<int>), "List<int>")]
        [InlineData(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
        public void SimpleGenericTypes(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<Dictionary<int, Uri>>), "List<Dictionary<int, Uri>>")]
        [InlineData(typeof(Dictionary<int, List<string>>), "Dictionary<int, List<string>>")]
        public void NestedGenericTypes(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<Dictionary<int, Uri[]>>), "List<Dictionary<int, Uri[]>>")]
        [InlineData(typeof(Dictionary<int, List<string>>[]), "Dictionary<int, List<string>>[]")]
        public void ArrayGenericMixedTypes(Type type, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new TypeName(type).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }
    }
}
