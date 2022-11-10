using dnlib.DotNet;

using dnWalker.Input;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Input
{
    public class UserArrayTests : DnlibTestBase
    {
        public UserArrayTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void BuildEmptyArray()
        {
            TypeSig elementType = DefinitionProvider.BaseTypes.String;

            UserArray array = new UserArray() { ElementType = elementType };

            Model model = new Model();
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();

            IValue result = array.Build(model, null, references);
            result.Should().BeOfType<Location>();
            result.Should().NotBe(Location.Null);

            Location location = (Location)result;

            model.HeapInfo.TryGetNode(location, out IHeapNode? arrayNode).Should().BeTrue();
            ((IArrayHeapNode)arrayNode!).Length.Should().Be(0);
        }

        [Theory]
        [InlineData("x", "y", "z")]
        [InlineData("x", null, "z")]
        [InlineData(null, null, null, null)]
        public void BuildArray(params string[] elements)
        {
            TypeSig elementType = DefinitionProvider.BaseTypes.String;

            UserArray array = new UserArray() { ElementType = elementType };
            int length = 0;
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i] != null)
                {
                    array.Elements[i] = new UserLiteral(elements[i]);
                    length = i + 1;
                }
            }

            Model model = new Model();
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();

            IValue result = array.Build(model, null, references);
            result.Should().BeOfType<Location>();
            result.Should().NotBe(Location.Null);

            Location location = (Location)result;

            model.HeapInfo.TryGetNode(location, out IHeapNode? node).Should().BeTrue();
            IArrayHeapNode arrayNode = (IArrayHeapNode)node!;

            arrayNode.Length.Should().Be(length);
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i] != null)
                {
                    arrayNode.TryGetElement(i, out IValue? value).Should().BeTrue();
                    value.Should().BeOfType<StringValue>();
                    StringValue strVal = (StringValue)value!;

                    strVal.Content.Should().Be(elements[i]);
                }
            }
        }

        [Theory]
        [InlineData("x", "y", "z")]
        public void BuildArrayUsingExpectedType(params string[] elements)
        {
            TypeSig elementType = DefinitionProvider.BaseTypes.String;

            UserArray array = new UserArray() { ElementType = elementType };
            int length = 0;
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i] != null)
                {
                    array.Elements[i] = new UserLiteral(elements[i]);
                    length = i + 1;
                }
            }

            Model model = new Model();
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();

            IValue result = array.Build(model, new SZArraySig(elementType), references);
            result.Should().BeOfType<Location>();
            result.Should().NotBe(Location.Null);

            Location location = (Location)result;

            model.HeapInfo.TryGetNode(location, out IHeapNode? node).Should().BeTrue();
            IArrayHeapNode arrayNode = (IArrayHeapNode)node!;

            arrayNode.Length.Should().Be(length);
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i] != null)
                {
                    arrayNode.TryGetElement(i, out IValue? value).Should().BeTrue();
                    value.Should().BeOfType<StringValue>();
                    StringValue strVal = (StringValue)value!;

                    strVal.Content.Should().Be(elements[i]);
                }
            }
        }


        [Theory]
        [InlineData("x", "y", "z")]
        public void BuildNamedArray(params string[] elements)
        {
            TypeSig elementType = DefinitionProvider.BaseTypes.String;

            string name = "myArray";

            UserArray array = new UserArray() { ElementType = elementType, Id = name };
            int length = 0;
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i] != null)
                {
                    array.Elements[i] = new UserLiteral(elements[i]);
                    length = i + 1;
                }
            }

            Model model = new Model();
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();

            IValue result = array.Build(model, null, references);

            references.Should().Contain(KeyValuePair.Create(name, result));
        }
    }
}
