using dnlib.DotNet;

using dnWalker.TestGenerator.Templates;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class TypeNameTemplateTests : TemplateTestBase
    {
        public class TypeNameTemplateTest : TemplateBase
        {
            public override string TransformText()
            {
                WriteTypeName(_type);

                return base.TransformText();
            }

            private readonly TypeSignature _type;

            public TypeNameTemplateTest(TypeSignature type)
            {
                _type = type;
            }
        }

        private readonly ITypeTranslator _translator;

        public TypeNameTemplateTests()
        {
            _translator = new TypeTranslator(DefinitionProvider);
        }

        [Theory]
        [InlineData("System.SByte", "sbyte")]
        [InlineData("System.Int16", "short")]
        [InlineData("System.Int32", "int")]
        [InlineData("System.Int64", "long")]
        [InlineData("System.Byte", "byte")]
        [InlineData("System.UInt16", "ushort")]
        [InlineData("System.UInt32", "uint")]
        [InlineData("System.UInt64", "ulong")]
        [InlineData("System.Single", "float")]
        [InlineData("System.Double", "double")]
        [InlineData("System.Char", "char")]
        [InlineData("System.Boolean", "bool")]
        [InlineData("System.String", "string")]
        public void BuildIns(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("System.UriFormat", "UriFormat")]
        [InlineData("System.ResolveEventArgs", "ResolveEventArgs")]
        public void NormalNonGenericTypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("System.Int32[]", "int[]")]
        [InlineData("System.ResolveEventArgs[]", "ResolveEventArgs[]")]
        public void ArrayTypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("System.Collections.Generic.List`1<System.Int32>", "List<int>")]
        [InlineData("System.Collections.Generic.Dictionary`2<System.Int32,System.String>", "Dictionary<int, string>")]
        public void SimpleGenericTypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("System.Collections.Generic.List`1<System.Collections.Generic.Dictionary`2<System.Int32, System.Uri>>", "List<Dictionary<int, Uri>>")]
        [InlineData("System.Collections.Generic.Dictionary`2<System.Int32, System.Collections.Generic.List`1<System.String>>", "Dictionary<int, List<string>>")]
        public void NestedGenericTypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("System.Collections.Generic.List`1<System.Collections.Generic.Dictionary`2<System.Int32,System.Uri[]>>", "List<Dictionary<int, Uri[]>>")]
        [InlineData("System.Collections.Generic.Dictionary`2<System.Int32,System.Collections.Generic.List`1<System.String>>[]", "Dictionary<int, List<string>>[]")]
        public void ArrayGenericMixedTypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }

        //[Theory(Skip = "Still problem with nested types - buffer overflow!!!")]
        [Theory]
        [InlineData("dnWalker.TestGenerator.Tests.Templates.TopLevel/SubLevel", "TopLevel.SubLevel")]
        [InlineData("dnWalker.TestGenerator.Tests.Templates.TopLevel/GenericSubLevel`1<System.Int32>", "TopLevel.GenericSubLevel<int>")]
        public void Nestedtypes(string fullTypeName, string expected)
        {
            TypeSignature type = _translator.FromString(fullTypeName);
            string result = new TypeNameTemplateTest(type).TransformText().Trim();
            result.Should().Be(expected);
        }
    }
}
