using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;

using FluentAssertions;
using FluentAssertions.Equivalency;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace dnWalker.Symbolic.Tests.Xml
{
    public class XmlModelDeserializerTests
    {
        public DefinitionProvider DefinitionProvider { get; }
        public ITypeParser TypeParser { get; }
        public IMethodParser MethodParser { get; }
        public TypeDef TestType { get; }
        public MethodDef TestMethod { get; }


        public class TestClass
        {
            public void TestMethod(double dblArg, int intArg, TestClass other, String strArg)
            {

            }

            public Double PrimitiveField;
            public TestClass? RefField;
            public Double[]? PrimitiveArrField;
            public TestClass[]? RefArrField;
            public String? StringField;

            public Double MockedMethod()
            {
                return Math.Ceiling(PrimitiveField);
            }
        }

        public XmlModelDeserializerTests()
        {
            IDomain domain = Domain.LoadFromAppDomain(typeof(XmlModelSerializerTests).Assembly);
            DefinitionProvider = new DefinitionProvider(domain);

            ModuleDef module = domain.MainModule;

            TestType = DefinitionProvider.Context.MainModule.FindThrow("dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests+TestClass", true);
            TestMethod = TestType.FindMethod("TestMethod");

            TypeParser tp = new TypeParser(DefinitionProvider);
            TypeParser = tp;
            MethodParser = new MethodParser(DefinitionProvider, tp);
        }


        [Fact]
        public void EmptyModel()
        {
            const string XML =
@"<Model>
  <Variables />
  <Heap />
</Model>";

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);
            IReadOnlyModel model = deserializer.FromXml(XElement.Parse(XML), TestMethod);

            model.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void PrimitiveMethodArguments()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""dblArg"" Value=""5"" />
    <MethodArgument Name=""intArg"" Value=""-3"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IVariable arg1 = Variable.MethodArgument(method.Parameters[1]);
            IVariable arg2 = Variable.MethodArgument(method.Parameters[2]);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);
            fromXML.Variables.Should().HaveCount(2);
            fromXML.Variables.Should().ContainInOrder((IRootVariable)arg1, (IRootVariable)arg2);

            fromXML.TryGetValue(arg1, out IValue? val1).Should().BeTrue();
            val1.Should().Be(ValueFactory.GetValue(5.0));

            fromXML.TryGetValue(arg2, out IValue? val2).Should().BeTrue();
            val2.Should().Be(ValueFactory.GetValue(-3));

            //unfortunately Should().BeEquivalentTo() created false positives
        }



        [Theory]
        [InlineData(Double.NegativeInfinity, "-INF")]
        [InlineData(Double.PositiveInfinity, "+INF")]
        [InlineData(Double.NaN, "NAN")]
        public void SpecialDoubleValues(Double value, String representation)
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""dblArg"" Value=""{0}"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IVariable arg1 = Variable.MethodArgument(method.Parameters[1]);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(string.Format(XML, representation)), method);
            fromXML.Variables.Should().HaveCount(1);
            fromXML.Variables.Should().Contain((IRootVariable)arg1);

            fromXML.TryGetValue(arg1, out IValue? val1).Should().BeTrue();
            val1.Should().Be(ValueFactory.GetValue(value));
        }

        [Fact]
        public void NullStringMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""strArg"" Value=""null"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.Variables.Should().BeEquivalentTo(new[] { (IRootVariable)Variable.MethodArgument(method.Parameters[4]) });
            fromXML.TryGetValue(Variable.MethodArgument(method.Parameters[4]), out IValue? value).Should().BeTrue();
            value.Should().Be(StringValue.Null);
        }

        [Fact]
        public void EmptyStringMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""strArg"" Value=""&quot;&quot;"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.Variables.Should().BeEquivalentTo(new[] { (IRootVariable)Variable.MethodArgument(method.Parameters[4]) });
            fromXML.TryGetValue(Variable.MethodArgument(method.Parameters[4]), out IValue? value).Should().BeTrue();
            value.Should().Be(StringValue.Empty);
        }

        [Fact]
        public void NotEmptyOrNullStringMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""strArg"" Value=""&quot;Hello world&quot;"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.Variables.Should().BeEquivalentTo(new[] { (IRootVariable)Variable.MethodArgument(method.Parameters[4]) });
            fromXML.TryGetValue(Variable.MethodArgument(method.Parameters[4]), out IValue? value).Should().BeTrue();
            value.Should().Be(new StringValue("Hello world"));
        }

        [Fact]
        public void NullReferenceMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""other"" Value=""null"" />
  </Variables>
  <Heap />
</Model>";
            MethodDef method = TestMethod;

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), Location.Null);


            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.Variables.Should().BeEquivalentTo(new[] { (IRootVariable)Variable.MethodArgument(method.Parameters[3]) });
            fromXML.TryGetValue(Variable.MethodArgument(method.Parameters[3]), out IValue? value).Should().BeTrue();
            value.Should().Be(Location.Null);
        }

        [Fact]
        public void NotNullReferenceMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""other"" Value=""@00000001"" />
  </Variables>
  <Heap>
    <ObjectNode IsDirty=""false"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" />
  </Heap>
</Model>";
            MethodDef method = TestMethod;

            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.Variables.Should().BeEquivalentTo(new[] { (IRootVariable)Variable.MethodArgument(method.Parameters[3]) });
            fromXML.TryGetValue(Variable.MethodArgument(method.Parameters[3]), out IValue? value).Should().BeTrue();
            value.Should().Be(new Location(1));
        }


        [Fact]
        public void NotNullReferenceWithInitializedFieldsMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""other"" Value=""@00000001"" />
  </Variables>
  <Heap>
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" FieldName=""RefField"" Value=""null"" />
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" FieldName=""StringField"" Value=""&quot;NotNullOrEmpty&quot;"" />
    </ObjectNode>
  </Heap>
</Model>";
            MethodDef method = TestMethod;
            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.HeapInfo.TryGetNode(new Location(1), out IReadOnlyHeapNode? node).Should().BeTrue();
            node.Should().BeAssignableTo<IReadOnlyObjectHeapNode>();

            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)node!;

            objNode.GetField("RefField").Should().Be(Location.Null);
            objNode.GetField("StringField").Should().Be(new StringValue("NotNullOrEmpty"));
        }


        [Fact]
        public void NotNullReferenceWithInitializedFieldsAndMockedMethodMethodArgument()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""other"" Value=""@00000001"" />
  </Variables>
  <Heap>
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" FieldName=""RefField"" Value=""null"" />
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" FieldName=""StringField"" Value=""&quot;NotNullOrEmpty&quot;"" />
      <MethodResult Method=""System.Double dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass::MockedMethod()"" Invocation=""1"" Value=""5"" />
      <MethodResult Method=""System.Double dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass::MockedMethod()"" Invocation=""2"" Value=""-5"" />
    </ObjectNode>
  </Heap>
</Model>";
            MethodDef method = TestMethod;
            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.HeapInfo.TryGetNode(new Location(1), out IReadOnlyHeapNode? node).Should().BeTrue();
            node.Should().BeAssignableTo<IReadOnlyObjectHeapNode>();

            IReadOnlyObjectHeapNode objNode = (IReadOnlyObjectHeapNode)node!;

            objNode.GetField("RefField").Should().Be(Location.Null);
            objNode.GetField("StringField").Should().Be(new StringValue("NotNullOrEmpty"));

            objNode.GetMethodResult("MockedMethod", 1).Should().Be(ValueFactory.GetValue(5.0));
            objNode.GetMethodResult("MockedMethod", 2).Should().Be(ValueFactory.GetValue(-5.0));
        }

        [Fact]
        public void ArrayItems()
        {
            const string XML =
@"<Model>
  <Variables>
    <MethodArgument Name=""other"" Value=""@00000001"" />
  </Variables>
  <Heap>
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass"" FieldName=""RefArrField"" Value=""@00000002"" />
    </ObjectNode>
    <ArrayNode IsDirty=""true"" Length=""4"" Location=""@00000002"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelDeserializerTests/TestClass[]"">
      <ArrayElement Index=""2"" Value=""null"" />
      <ArrayElement Index=""3"" Value=""@00000001"" />
    </ArrayNode>
  </Heap>
</Model>";
            MethodDef method = TestMethod;
            XmlModelDeserializer deserializer = new XmlModelDeserializer(TypeParser, MethodParser);

            IReadOnlyModel fromXML = deserializer.FromXml(XElement.Parse(XML), method);

            fromXML.HeapInfo.TryGetNode(new Location(2), out IReadOnlyHeapNode? node).Should().BeTrue();
            node.Should().BeAssignableTo<IReadOnlyArrayHeapNode>();

            IReadOnlyArrayHeapNode arrNode = (IReadOnlyArrayHeapNode)node!;
            arrNode.Length.Should().Be(4);

            arrNode.TryGetElement(0, out IValue? _).Should().BeFalse();
            arrNode.TryGetElement(1, out IValue? _).Should().BeFalse();
            arrNode.TryGetElement(2, out IValue? v2).Should().BeTrue();
            arrNode.TryGetElement(3, out IValue? v3).Should().BeTrue();

            v2.Should().Be(Location.Null);
            v3.Should().Be(new Location(1));

        }

    }
}
