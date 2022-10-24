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
    public class UserObjectTests : DnlibTestBase
    {
        public class TestClass
        {
            public int Field;
            public string? Property { get; }
            public bool Method() { return false; }
        }

        private readonly TypeDef TestTypeDef;
        private readonly TypeSig TestTypeSig;

        public UserObjectTests(ITestOutputHelper textOutput) : base(textOutput)
        {
            TestTypeDef = DefinitionProvider.GetTypeDefinition("dnWalker.Tests.Input.UserObjectTests/TestClass");
            TestTypeSig = TestTypeDef.ToTypeSig();
        }

        [Fact]
        public void BuildEmptyObject()
        {
            TypeSig ts = TestTypeSig;

            UserObject obj = new UserObject() { Type = ts };

            Model model = new Model();

            IValue value= obj.Build(model, null, new Dictionary<string, IValue>());

            Location location = (Location)value;

            location.Should().NotBe(Location.Null);

            model.HeapInfo.TryGetNode(location, out IHeapNode? node).Should().BeTrue();
            IObjectHeapNode objNode = (IObjectHeapNode)node!;

            objNode.Type.Should().Be(ts);
            objNode.Fields.Should().BeEmpty();
            objNode.MethodInvocations.Should().BeEmpty();
        }

        [Fact]
        public void BuildEmptyObjectUsingExpectedType()
        {
            TypeSig ts = TestTypeSig;

            UserObject obj = new UserObject();

            Model model = new Model();

            IValue value = obj.Build(model, ts, new Dictionary<string, IValue>());

            Location location = (Location)value;

            location.Should().NotBe(Location.Null);

            model.HeapInfo.TryGetNode(location, out IHeapNode? node).Should().BeTrue();
            IObjectHeapNode objNode = (IObjectHeapNode)node!;

            objNode.Type.Should().Be(ts);
            objNode.Fields.Should().BeEmpty();
            objNode.MethodInvocations.Should().BeEmpty();
        }

        [Theory]
        [InlineData(5)]
        public void BuildObjectWithFields(int fieldValue)
        {
            TypeSig ts = TestTypeSig;
            TypeDef td = TestTypeDef;

            FieldDef fd = td.FindField(nameof(TestClass.Field));

            UserObject obj = new UserObject() { Type = ts };
            obj.Fields[fd] = new UserLiteral(fieldValue.ToString());

            Model model = new Model();

            IValue value = obj.Build(model, ts, new Dictionary<string, IValue>());
            value.Should().NotBeNull();
            value.Should().BeOfType<Location>();

            model.HeapInfo.TryGetNode((Location)value, out IHeapNode? node).Should().BeTrue();

            IObjectHeapNode objNode = (IObjectHeapNode)node!;

            objNode.MethodInvocations.Should().BeEmpty();

            objNode.Fields.Should().BeEquivalentTo(new IField[] { fd });
            objNode.TryGetField(fd, out IValue? fldValue).Should().BeTrue();
            PrimitiveValue<int> intFldValue = (PrimitiveValue<int>)fldValue!;
            intFldValue.Value.Should().Be(fieldValue);
        }


        [Theory]
        [InlineData(new string?[] { null }, new int[] {0})]
        [InlineData(new string?[] { "hello world", "string" }, new int[] { 0, 1 })]
        [InlineData(new string?[] { "hello world", "string" }, new int[] { 2, 5 })]
        [InlineData(new string?[] { "hello world", "\"\"", "6", null, "null" }, new int[] { 2, 5, 6, 7, 8 })]
        public void BuildObjectWithPropertis(string?[] values, int[] invocations)
        {
            TypeSig ts = TestTypeSig;
            TypeDef td = TestTypeDef;

            PropertyDef pd = td.FindProperty(nameof(TestClass.Property));
            MethodDef pGetter = pd.GetMethod;

            UserObject obj = new UserObject() { Type = ts };
            
            for (int i = 0; i < invocations.Length; i++)
            {
                obj.MethodResults[(pGetter, invocations[i])] = new UserLiteral(values[i]);
            }

            Model model = new Model();

            IValue value = obj.Build(model, ts, new Dictionary<string, IValue>());
            value.Should().NotBeNull();
            value.Should().BeOfType<Location>();

            model.HeapInfo.TryGetNode((Location)value, out IHeapNode? node).Should().BeTrue();

            IObjectHeapNode objNode = (IObjectHeapNode)node!;

            objNode.Fields.Should().BeEmpty();
            objNode.MethodInvocations.Should().HaveCount(invocations.Length);

            int maxInvocation = invocations.Length == 0 ? 0 : invocations.Max();

            for (int i = 0; i < maxInvocation; i++)
            {
                int idx = Array.IndexOf(invocations, i);

                objNode.TryGetMethodResult(pGetter, i, out IValue? mr).Should().Be(idx >= 0);

                if (idx >= 0)
                {
                    ((StringValue)mr!).Content.Should().Be(values[idx]?.Trim('"'));
                }
            }
        }



        [Theory]
        [InlineData(new bool[] { true }, new int[] { 0 })]
        [InlineData(new bool[] { true, false }, new int[] { 0, 1 })]
        [InlineData(new bool[] { false, true }, new int[] { 2, 5 })]
        public void BuildObjectWithMethods(bool[] values, int[] invocations)
        {
            TypeSig ts = TestTypeSig;
            TypeDef td = TestTypeDef;

            MethodDef md = td.FindMethod(nameof(TestClass.Method));

            UserObject obj = new UserObject() { Type = ts };

            for (int i = 0; i < invocations.Length; i++)
            {
                obj.MethodResults[(md, invocations[i])] = new UserLiteral(values[i].ToString());
            }

            Model model = new Model();

            IValue value = obj.Build(model, ts, new Dictionary<string, IValue>());
            value.Should().NotBeNull();
            value.Should().BeOfType<Location>();

            model.HeapInfo.TryGetNode((Location)value, out IHeapNode? node).Should().BeTrue();

            IObjectHeapNode objNode = (IObjectHeapNode)node!;

            objNode.Fields.Should().BeEmpty();
            objNode.MethodInvocations.Should().HaveCount(invocations.Length);

            int maxInvocation = invocations.Length == 0 ? 0 : invocations.Max();

            for (int i = 0; i < maxInvocation; i++)
            {
                int idx = Array.IndexOf(invocations, i);

                objNode.TryGetMethodResult(md, i, out IValue? mr).Should().Be(idx >= 0);

                if (idx >= 0)
                {
                    ((PrimitiveValue<bool>)mr!).Value.Should().Be(values[idx]);
                }
            }
        }
    }
}
