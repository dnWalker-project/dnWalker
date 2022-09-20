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

using Xunit;

namespace dnWalker.Symbolic.Tests.Xml
{
    public class XmlModelSerializerTests
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

        public XmlModelSerializerTests()
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

            Model model = new Model();

            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);
            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[1]), ValueFactory.GetValue(5.0));
            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[2]), ValueFactory.GetValue(-3));


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[1]), ValueFactory.GetValue(value));

            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(string.Format(XML, representation));
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

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), StringValue.Null);


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), new StringValue(""));


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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

            Model model = new Model();

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[4]), new StringValue("Hello world"));


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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
    <ObjectNode IsDirty=""false"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" />
  </Heap>
</Model>";
            MethodDef method = TestMethod;

            Model model = new Model();

            IObjectHeapNode otherNode = model.HeapInfo.InitializeObject(TestType.ToTypeSig());

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), otherNode.Location);


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" FieldName=""RefField"" Value=""null"" />
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" FieldName=""StringField"" Value=""&quot;NotNullOrEmpty&quot;"" />
    </ObjectNode>
  </Heap>
</Model>";
            MethodDef method = TestMethod;

            Model model = new Model();

            IObjectHeapNode otherNode = model.HeapInfo.InitializeObject(TestType.ToTypeSig());
            otherNode.SetField(TestType.FindField("RefField"), Location.Null);
            otherNode.SetField(TestType.FindField("StringField"), new StringValue("NotNullOrEmpty"));

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), otherNode.Location);


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" FieldName=""RefField"" Value=""null"" />
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" FieldName=""StringField"" Value=""&quot;NotNullOrEmpty&quot;"" />
      <MethodResult Method=""System.Double dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass::MockedMethod()"" Invocation=""1"" Value=""5"" />
      <MethodResult Method=""System.Double dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass::MockedMethod()"" Invocation=""2"" Value=""-5"" />
    </ObjectNode>
  </Heap>
</Model>";
            MethodDef method = TestMethod;

            Model model = new Model();

            IObjectHeapNode otherNode = model.HeapInfo.InitializeObject(TestType.ToTypeSig());
            otherNode.SetField(TestType.FindField("RefField"), Location.Null);
            otherNode.SetField(TestType.FindField("StringField"), new StringValue("NotNullOrEmpty"));

            otherNode.SetMethodResult(TestType.FindMethod("MockedMethod"), 1, ValueFactory.GetValue(5.0));
            otherNode.SetMethodResult(TestType.FindMethod("MockedMethod"), 2, ValueFactory.GetValue(-5.0));

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), otherNode.Location);


            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
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
    <ObjectNode IsDirty=""true"" Location=""@00000001"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"">
      <InstanceField DeclaringType=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass"" FieldName=""RefArrField"" Value=""@00000002"" />
    </ObjectNode>
    <ArrayNode IsDirty=""true"" Length=""4"" Location=""@00000002"" Type=""dnWalker.Symbolic.Tests.Xml.XmlModelSerializerTests/TestClass[]"">
      <ArrayElement Index=""2"" Value=""null"" />
      <ArrayElement Index=""3"" Value=""@00000001"" />
    </ArrayNode>
  </Heap>
</Model>";

            MethodDef method = TestMethod;

            Model model = new Model();

            IObjectHeapNode otherNode = model.HeapInfo.InitializeObject(TestType.ToTypeSig());
            IArrayHeapNode arrNode = model.HeapInfo.InitializeArray(TestType.ToTypeSig(), 4);

            otherNode.SetField(TestType.FindField("RefArrField"), arrNode.Location);

            model.SetValue((IRootVariable)Variable.MethodArgument(method.Parameters[3]), otherNode.Location);

            arrNode.SetElement(2, Location.Null);
            arrNode.SetElement(3, otherNode.Location);

            XmlModelSerializer serializer = new XmlModelSerializer(TypeParser, MethodParser);

            string xml = serializer.ToXml(model).ToString();

            xml.Should().Be(XML);
        }

    }
}
