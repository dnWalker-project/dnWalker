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
        public class TopLevel
        {
            public class SubLevel
            {
            }

            public class GenericSubLevel<T>
            {

            }
        }

        public class GenericTopLevel<TTop>
        {
            public class SubLevel
            {

            }

            public class GenericSubLevel<TSub>
            {

            }
        }

        public class TypeNameTemplateTest : TemplateBase
        {
            public override string TransformText()
            {
                WriteTypeName(_type);

                return base.TransformText();
            }

            private readonly Type _type;

            public TypeNameTemplateTest(Type type)
            {
                _type = type;
            }
        }

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
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(UriFormat), "UriFormat")]
        [InlineData(typeof(ResolveEventArgs), "ResolveEventArgs")]
        public void NormalNonGenericTypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(int[]), "int[]")]
        [InlineData(typeof(ResolveEventArgs[]), "ResolveEventArgs[]")]
        public void ArrayTypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<int>), "List<int>")]
        [InlineData(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
        public void SimpleGenericTypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<Dictionary<int, Uri>>), "List<Dictionary<int, Uri>>")]
        [InlineData(typeof(Dictionary<int, List<string>>), "Dictionary<int, List<string>>")]
        public void NestedGenericTypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(List<Dictionary<int, Uri[]>>), "List<Dictionary<int, Uri[]>>")]
        [InlineData(typeof(Dictionary<int, List<string>>[]), "Dictionary<int, List<string>>[]")]
        public void ArrayGenericMixedTypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        //[Theory(Skip = "Still problem with nested types - buffer overflow!!!")]
        [Theory]
        [InlineData(typeof(TopLevel.SubLevel), "TypeNameTemplateTests.TopLevel.SubLevel")]
        [InlineData(typeof(TopLevel.GenericSubLevel<int>), "TypeNameTemplateTests.TopLevel.GenericSubLevel<int>")]
        //[InlineData(typeof(GenericTopLevel<double>.SubLevel), "TypeNameTemplateTests.GenericTopLevel<double>.SubLevel")]
        //[InlineData(typeof(GenericTopLevel<List<double>>.GenericSubLevel<int>), "TypeNameTemplateTests.GenericTopLevel<List<double>>.GenericSubLevel<int>")]
        public void Nestedtypes(Type type, string expected)
        {
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }
    }
}
