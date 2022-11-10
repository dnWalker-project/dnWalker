using dnWalker.Input;
using dnWalker.Symbolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Input
{
    public class UserLiteralTests : DnlibTestBase
    {
        // missing BuildNamed*** Tests

        public UserLiteralTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Theory]
        [InlineData("some not null or empty literal")]
        public void BuildLiteralWithouExpectedType(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue) { Type = DefinitionProvider.BaseTypes.String};
            IValue value = literal.Build(new Model(), null, new Dictionary<string, IValue>());

            value.Should().Be(new StringValue(literalValue));
        }

        [Theory]
        [InlineData("null")]
        [InlineData(null)]
        [InlineData("")]
        public void BuildNullString(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.String, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<StringValue>();
            value.Should().Be(StringValue.Null);
        }

        [Theory]
        [InlineData("\"\"")]
        public void BuildEmptyString(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.String, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<StringValue>();
            value.Should().Be(StringValue.Empty);
        }

        [Theory]
        [InlineData("\"hello world\"")]
        [InlineData("hello world\"")]
        [InlineData("\"hello world")]
        public void BuildNonEmptyString(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.String, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<StringValue>();
            value.Should().Be(new StringValue(literalValue.Trim('"')));
        }

        [Theory]
        [InlineData("\"hello world\"", "myString")]
        [InlineData("hello world\"", "myString")]
        [InlineData("\"hello world", "myString")]
        public void BuildNamedString(string literalValue, string name)
        {
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();
            UserLiteral literal = new UserLiteral(literalValue) { Id = name};
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.String, references);

            references.Should().ContainKey(name);
            references[name].Should().Be(value);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void BuildBoolean(string literalValue, bool expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Boolean, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<bool>>();
            value.Should().Be(new PrimitiveValue<bool>(expected));
        }

        [Theory]
        [InlineData("true", true, "mybool")]
        public void BuildNamedBoolean(string literalValue, bool expected, string name)
        {
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();
            UserLiteral literal = new UserLiteral(literalValue) { Id = name };
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Boolean, references);

            references.Should().ContainKey(name);
            references[name].Should().Be(value);
        }


        [Theory]
        [InlineData("A", 'A')]
        [InlineData(null, default(char))]
        [InlineData("", default(char))]
        public void BuildChar(string literalValue, char expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Char, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<char>>();
            value.Should().Be(new PrimitiveValue<char>(expected));
        }

        [Theory]
        [InlineData("A", 'A', "myChar")]
        public void BuildNamedChar(string literalValue, char expected, string name)
        {
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();
            UserLiteral literal = new UserLiteral(literalValue) { Id = name };
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Char, references);

            references.Should().ContainKey(name);
            references[name].Should().Be(value);
        }

        [Theory]
        [InlineData("0", byte.MinValue)]
        [InlineData("255", byte.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildByte(string literalValue, byte expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Byte, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<byte>>();
            value.Should().Be(new PrimitiveValue<byte>(expected));
        }

        [Theory]
        [InlineData("128", 128, "myByte")]
        public void BuildNamedByte(string literalValue, byte expected, string name)
        {
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();
            UserLiteral literal = new UserLiteral(literalValue) { Id = name };
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Byte, references);

            references.Should().ContainKey(name);
            references[name].Should().Be(value);
        }


        [Theory]
        [InlineData("0", ushort.MinValue)]
        [InlineData("65535", ushort.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildUInt16(string literalValue, ushort expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.UInt16, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<ushort>>();
            value.Should().Be(new PrimitiveValue<ushort>(expected));
        }

        [Theory]
        [InlineData("0", uint.MinValue)]
        [InlineData("4294967295", uint.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildUInt32(string literalValue, uint expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.UInt32, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<uint>>();
            value.Should().Be(new PrimitiveValue<uint>(expected));
        }

        [Theory]
        [InlineData("0", ulong.MinValue)]
        [InlineData("18446744073709551615", ulong.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildUInt64(string literalValue, ulong expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.UInt64, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<ulong>>();
            value.Should().Be(new PrimitiveValue<ulong>(expected));
        }

        [Theory]
        [InlineData("-128", sbyte.MinValue)]
        [InlineData("127", sbyte.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildSByte(string literalValue, sbyte expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.SByte, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<sbyte>>();
            value.Should().Be(new PrimitiveValue<sbyte>(expected));
        }

        [Theory]
        [InlineData("-32768", short.MinValue)]
        [InlineData("32767", short.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildInt16(string literalValue, short expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Int16, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<short>>();
            value.Should().Be(new PrimitiveValue<short>(expected));
        }

        [Theory]
        [InlineData("-2147483648", int.MinValue)]
        [InlineData("2147483647", int.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildInt32(string literalValue, int expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Int32, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<int>>();
            value.Should().Be(new PrimitiveValue<int>(expected));
        }

        [Theory]
        [InlineData("-9223372036854775808", long.MinValue)]
        [InlineData("9223372036854775807", long.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildInt64(string literalValue, long expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Int64, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<long>>();
            value.Should().Be(new PrimitiveValue<long>(expected));
        }

        [Theory]
        [InlineData("-3.40282347E+38", float.MinValue)]
        [InlineData("3.40282347E+38", float.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildSingle(string literalValue, float expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Single, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<float>>();
            value.Should().Be(new PrimitiveValue<float>(expected));
        }

        [Theory]
        [InlineData("-1.7976931348623157E+308", double.MinValue)]
        [InlineData("1.7976931348623157E+308", double.MaxValue)]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        public void BuildDouble(string literalValue, double expected)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Double, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<PrimitiveValue<double>>();
            value.Should().Be(new PrimitiveValue<double>(expected));
        }

        [Theory]
        [InlineData("null")]
        [InlineData(null)]
        [InlineData("")]
        public void BuildNullObject(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            IValue value = literal.Build(new Model(), DefinitionProvider.BaseTypes.Object, new Dictionary<string, IValue>());

            value.Should().NotBeNull();
            value.Should().BeOfType<Location>();
            value.Should().Be(Location.Null);
        }

        [Theory]
        [InlineData("something which is not null or empty string")]
        public void BuildNonNullObject(string literalValue)
        {
            UserLiteral literal = new UserLiteral(literalValue);
            Action act = () => literal.Build(new Model(), DefinitionProvider.BaseTypes.Object, new Dictionary<string, IValue>());

            act.Should().Throw<NotSupportedException>();
        }
    }
}
