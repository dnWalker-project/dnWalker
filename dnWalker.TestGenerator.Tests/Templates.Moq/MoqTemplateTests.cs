using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestGenerator.Templates.Moq;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates.Moq
{
    public class MoqTemplateTests
    {
        private abstract class ArgClass
        {
            public object PublicRefField;
            private object PrivateRefField;

            public int PublicPrimitiveField;
            private int PrivatePrimitiveField;

            public string PublicStringField;
            private string PrivateStringField;

            public abstract object RefMethodNoArgs();
            public abstract object RefMethodArgs(string name, ArgClass other, int i);

            public abstract int PrimitiveMethodNoArgs();
            public abstract int PrimitiveMethodArgs(string name, ArgClass other, int i);

            public static int MagicNumber;
            private static int PrivateMagicNumber;

            public static string MagicString;
            private static string PrivateMagicString;

            public static object MagicObject;
            private static object PrivateMagicObject;

        }

        private class TestClass
        {
            public static int TestedMethod(string strArg, ArgClass refArg, int primitiveArg) => 0;
        }

        private readonly MoqTemplate Template = new MoqTemplate();
        private readonly TypeDef TestClassTD = TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.Moq.MoqTemplateTests/TestClass");
        private readonly TypeSig TestClassTS = TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.Moq.MoqTemplateTests/TestClass").ToTypeSig();
        
        private readonly TypeDef ArgClassTD = TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.Moq.MoqTemplateTests/ArgClass");
        private readonly TypeSig ArgClassTS = TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.Moq.MoqTemplateTests/ArgClass").ToTypeSig();

        private readonly TypeSig ObjectTS = TestUtils.DefinitionProvider.BaseTypes.Object;
        private readonly TypeSig IntTS = TestUtils.DefinitionProvider.BaseTypes.Int32;

        private IField GetField(string fieldName)
        {
            return ArgClassTD.GetField(fieldName);
        }

        private IMethod GetMethod(string methodName)
        {
            return ArgClassTD.FindMethods(methodName).First();
        }

        private IMethod TestedMethod => TestClassTD.FindMethods("TestedMethod").First();


        [Fact]
        public void ArrangeEmptyModel()
        {
            Model model = new Model();
            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().BeEmpty();
        }

        #region Arrays
        [Fact]
        public void ArrangeArrayRef()
        {
            Model model = new Model();

            IArrayHeapNode arrayNode = model.HeapInfo.InitializeArray(ArgClassTS, 2);
            IObjectHeapNode objectNode = model.HeapInfo.InitializeObject(ArgClassTS);
            arrayNode.SetElement(0, objectNode.Location);
            arrayNode.SetElement(1, Location.Null);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    // ArgClass is abstract => we need to have the mock...
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "MoqTemplateTests.ArgClass[] argClassArray1 = new MoqTemplateTests.ArgClass[2];",
                    "argClassArray1[0] = argClass1;",
                    "argClassArray1[1] = null;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            
            locationNames.Should().HaveCount(2).And.ContainKeys(arrayNode.Location, objectNode.Location);
            locationNames[arrayNode.Location].Should().Be("argClassArray1");
            locationNames[objectNode.Location].Should().Be("argClass1");
        }

        [Fact]
        public void ArrangeArrayRefSameElement()
        {
            Model model = new Model();

            IArrayHeapNode arrayNode = model.HeapInfo.InitializeArray(ArgClassTS, 2);
            IObjectHeapNode objectNode = model.HeapInfo.InitializeObject(ArgClassTS);
            arrayNode.SetElement(0, objectNode.Location);
            arrayNode.SetElement(1, objectNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    // ArgClass is abstract => we need to have the mock...
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "MoqTemplateTests.ArgClass[] argClassArray1 = new MoqTemplateTests.ArgClass[2];",
                    "argClassArray1[0] = argClass1;",
                    "argClassArray1[1] = argClass1;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));

            locationNames.Should().HaveCount(2).And.ContainKeys(arrayNode.Location, objectNode.Location);
            locationNames[arrayNode.Location].Should().Be("argClassArray1");
            locationNames[objectNode.Location].Should().Be("argClass1");
        }

        [Fact]
        public void ArrangeArrayPrimitive()
        {
            Model model = new Model();

            IArrayHeapNode arrayNode = model.HeapInfo.InitializeArray(IntTS, 5);
            arrayNode.SetElement(0, ValueFactory.GetValue(5));
            arrayNode.SetElement(1, ValueFactory.GetValue(4));
            arrayNode.SetElement(2, ValueFactory.GetValue(3));
            arrayNode.SetElement(4, ValueFactory.GetValue(0));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "int[] intArray1 = new int[5];",
                    "intArray1[0] = 5;",
                    "intArray1[1] = 4;",
                    "intArray1[2] = 3;",
                    "intArray1[4] = 0;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));

            locationNames.Should().HaveCount(1).And.ContainKeys(arrayNode.Location);
            locationNames[arrayNode.Location].Should().Be("intArray1");
        }
        #endregion Arrays

        #region Instance Fields
        [Fact]
        public void ArrangeInstanceFieldPublicRef()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode fldNode = model.HeapInfo.InitializeObject(ObjectTS);

            objNode.SetField(GetField("PublicRefField"), fldNode.Location);

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine, 
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "argClass1.PublicRefField = object1;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[fldNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeInstanceFieldPrivateRef()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode fldNode = model.HeapInfo.InitializeObject(ObjectTS);

            objNode.SetField(GetField("PrivateRefField"), fldNode.Location);

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateRefField\", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(argClass1, object1);",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[fldNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeInstanceFieldPublicPrimitive()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);

            objNode.SetField(GetField("PublicPrimitiveField"), ValueFactory.GetValue(10));

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "argClass1.PublicPrimitiveField = 10;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(1);
            locationNames[objNode.Location].Should().Be("argClass1");
        }

        [Fact]
        public void ArrangeInstanceFieldPrivatePrimitive()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);

            objNode.SetField(GetField("PrivatePrimitiveField"), ValueFactory.GetValue(10));

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivatePrimitiveField\", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(argClass1, 10);",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(1);
            locationNames[objNode.Location].Should().Be("argClass1");
        }

        [Fact]
        public void ArrangeInstanceFieldPublicString()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);

            objNode.SetField(GetField("PublicStringField"), new StringValue("hello world!"));

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[0]), new StringValue("NOT A NULL!"));
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "argClass1.PublicStringField = \"hello world!\";",
                    "",
                    "// Arrange method arguments",
                    "string strArg = \"NOT A NULL!\";",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(1);
            locationNames[objNode.Location].Should().Be("argClass1");
        }

        [Fact]
        public void ArrangeInstanceFieldPrivateString()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);

            objNode.SetField(GetField("PrivateStringField"), new StringValue("hello world!"));

            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[0]), new StringValue("NOT A NULL!"));
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateStringField\", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(argClass1, \"hello world!\");",
                    "",
                    "// Arrange method arguments",
                    "string strArg = \"NOT A NULL!\";",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"
                    ));
            locationNames.Should().HaveCount(1);
            locationNames[objNode.Location].Should().Be("argClass1");
        }
        #endregion Instance Fields

        #region Method Results
        [Fact]
        public void ArrangeMethodResultPrimitive()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodNoArgs"), 1, ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodNoArgs())",
                    "        .Returns(10);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultPrimitiveSkipped()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodNoArgs"), 3, ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodNoArgs())",
                    "        .Returns(0)",
                    "        .Returns(0)",
                    "        .Returns(10);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultsPrimitiveMultiple()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodNoArgs"), 1, ValueFactory.GetValue(5));
            objNode.SetMethodResult(GetMethod("PrimitiveMethodNoArgs"), 2, ValueFactory.GetValue(10));
            objNode.SetMethodResult(GetMethod("PrimitiveMethodNoArgs"), 3, ValueFactory.GetValue(15));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodNoArgs())",
                    "        .Returns(5)",
                    "        .Returns(10)",
                    "        .Returns(15);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultRef()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode resNode = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodNoArgs"), 1, resNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodNoArgs())",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[resNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeMethodResultRefSkipped()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode resNode = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodNoArgs"), 5, resNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodNoArgs())",
                    "        .Returns(null)",
                    "        .Returns(null)",
                    "        .Returns(null)",
                    "        .Returns(null)",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[resNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeMethodResultsRefMultiple()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode res1Node = model.HeapInfo.InitializeObject(ObjectTS);
            IObjectHeapNode res2Node = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodNoArgs"), 1, res1Node.Location);
            objNode.SetMethodResult(GetMethod("RefMethodNoArgs"), 2, res2Node.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            locationNames.Should().HaveCount(3);
            locationNames[objNode.Location].Should().Be("argClass1");

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "object object2 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodNoArgs())",
                    // there are multiple topological sorts and here it is sorted this way...
                    "        .Returns(object2)",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
        }

        [Fact]
        public void ArrangeMethodResultPrimitiveWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodArgs"), 1, ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(10);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultPrimitiveSkippedWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodArgs"), 3, ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(0)",
                    "        .Returns(0)",
                    "        .Returns(10);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultsPrimitiveMultipleWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("PrimitiveMethodArgs"), 1, ValueFactory.GetValue(5));
            objNode.SetMethodResult(GetMethod("PrimitiveMethodArgs"), 2, ValueFactory.GetValue(10));
            objNode.SetMethodResult(GetMethod("PrimitiveMethodArgs"), 3, ValueFactory.GetValue(15));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.PrimitiveMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(5)",
                    "        .Returns(10)",
                    "        .Returns(15);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeMethodResultRefWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode resNode = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodArgs"), 1, resNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[resNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeMethodResultRefSkippedWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode resNode = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodArgs"), 3, resNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(null)",
                    "        .Returns(null)",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(2);
            locationNames[objNode.Location].Should().Be("argClass1");
            locationNames[resNode.Location].Should().Be("object1");
        }

        [Fact]
        public void ArrangeMethodResultsRefMultipleWithArgs()
        {
            Model model = new Model();
            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode res1Node = model.HeapInfo.InitializeObject(ObjectTS);
            IObjectHeapNode res2Node = model.HeapInfo.InitializeObject(ObjectTS);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), objNode.Location);

            objNode.SetMethodResult(GetMethod("RefMethodArgs"), 1, res1Node.Location);
            objNode.SetMethodResult(GetMethod("RefMethodArgs"), 2, res2Node.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            locationNames.Should().HaveCount(3);
            locationNames[objNode.Location].Should().Be("argClass1");

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "object object2 = new object();",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    // there are multiple topological sorts and here it is sorted this way...
                    "        .Returns(object2)",
                    "        .Returns(object1);",
                    "}",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 0;"));
        }
        #endregion Method Results

        #region Static Fields
        [Fact]
        public void ArrangeStaticFieldPublicRef()
        {
            Model model = new Model();

            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ObjectTS);

            model.SetValue(new StaticFieldVariable(GetField("MagicObject")), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "MoqTemplateTests.ArgClass.MagicObject = object1;",
                    "",
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeStaticFieldPrivateRef()
        {
            Model model = new Model();

            IObjectHeapNode objNode = model.HeapInfo.InitializeObject(ObjectTS);

            model.SetValue(new StaticFieldVariable(GetField("PrivateMagicObject")), objNode.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateMagicObject\", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, object1);",
                    "",
                    "// Arrange input model heap",
                    "object object1 = new object();",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(1);
        }

        [Fact]
        public void ArrangeStaticFieldPublicPrimitive()
        {
            Model model = new Model();

            model.SetValue(new StaticFieldVariable(GetField("MagicNumber")), ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "MoqTemplateTests.ArgClass.MagicNumber = 10;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().BeEmpty();
        }

        [Fact]
        public void ArrangeStaticFieldPrivatePrimitive()
        {
            Model model = new Model();

            model.SetValue(new StaticFieldVariable(GetField("PrivateMagicNumber")), ValueFactory.GetValue(10));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateMagicNumber\", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 10);",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().BeEmpty();
        }

        [Fact]
        public void ArrangeStaticFieldPublicString()
        {
            Model model = new Model();

            model.SetValue(new StaticFieldVariable(GetField("MagicString")), new StringValue("Hello world!"));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "MoqTemplateTests.ArgClass.MagicString = \"Hello world!\";",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().BeEmpty();
        }

        [Fact]
        public void ArrangeStaticFieldPrivateString()
        {
            Model model = new Model();

            model.SetValue(new StaticFieldVariable(GetField("PrivateMagicString")), new StringValue("Hello world!"));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateMagicString\", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, \"Hello world!\");",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().BeEmpty();
        }
        #endregion Static Fields

        #region Circular Dependencies
        [Fact]
        public void ArrangeCircularDependency()
        {
            Model model = new Model();

            IObjectHeapNode objNode1 = model.HeapInfo.InitializeObject(ArgClassTS);
            IObjectHeapNode objNode2 = model.HeapInfo.InitializeObject(ArgClassTS);

            objNode1.SetField(GetField("PublicRefField"), objNode2.Location);
            objNode2.SetField(GetField("PublicRefField"), objNode1.Location);

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange input model heap",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "MoqTemplateTests.ArgClass argClass2 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "argClass1.PublicRefField = argClass2;",
                    "argClass2.PublicRefField = argClass1;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = null;",
                    "MoqTemplateTests.ArgClass refArg = null;",
                    "int primitiveArg = 0;"));
            locationNames.Should().HaveCount(2);

        }
        #endregion Circular Dependencies

        #region Mix Of All
        [Fact]
        public void ArrangeComplexHeap()
        {
            Model model = new Model();
            IObjectHeapNode refArgNode = model.HeapInfo.InitializeObject(ArgClassTS);
            IArrayHeapNode primitiveArrNode = model.HeapInfo.InitializeArray(IntTS, 8);
            primitiveArrNode.SetElement(1, ValueFactory.GetValue(3));
            primitiveArrNode.SetElement(5, ValueFactory.GetValue(42));
            primitiveArrNode.SetElement(7, ValueFactory.GetValue(-86));

            IArrayHeapNode refArrNode = model.HeapInfo.InitializeArray(ArgClassTS, 3);
            refArrNode.SetElement(0, refArgNode.Location);
            refArrNode.SetElement(2, Location.Null);

            refArgNode.SetField(GetField("PublicRefField"), primitiveArrNode.Location);
            refArgNode.SetMethodResult(GetMethod("RefMethodArgs"), 2, refArrNode.Location);


            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[0]), new StringValue("Hello world!"));
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[1]), refArgNode.Location);
            model.SetValue(new MethodArgumentVariable(TestedMethod.ResolveMethodDefThrow().Parameters[2]), ValueFactory.GetValue(42));

            model.SetValue(new StaticFieldVariable(GetField("PrivateMagicString")), new StringValue("Hello world!"));

            TestWriter output = new TestWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Should().Be(
                string.Join(Environment.NewLine,
                    "// Arrange static fields",
                    "typeof(MoqTemplateTests.ArgClass).GetField(\"PrivateMagicString\", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, \"Hello world!\");",
                    "",
                    "// Arrange input model heap",
                    "int[] intArray1 = new int[8];",
                    "intArray1[1] = 3;",
                    "intArray1[5] = 42;",
                    "intArray1[7] = -86;",
                    "MoqTemplateTests.ArgClass argClass1 = new Mock<MoqTemplateTests.ArgClass>().Object;",
                    "MoqTemplateTests.ArgClass[] argClassArray1 = new MoqTemplateTests.ArgClass[3];",
                    "argClass1.PublicRefField = intArray1;",
                    "{",
                    "    Mock<MoqTemplateTests.ArgClass> mock = Mock.Get(argClass1);",
                    "    mock.SetupSequence(o => o.RefMethodArgs(It.IsAny<string>(), It.IsAny<MoqTemplateTests.ArgClass>(), It.IsAny<int>()))",
                    "        .Returns(null)",
                    "        .Returns(argClassArray1);",
                    "}",
                    "argClassArray1[0] = argClass1;",
                    "argClassArray1[2] = null;",
                    "",
                    "// Arrange method arguments",
                    "string strArg = \"Hello world!\";",
                    "MoqTemplateTests.ArgClass refArg = argClass1;",
                    "int primitiveArg = 42;"));
            locationNames.Should().HaveCount(3);
        }
        #endregion Mix Of All
    }
}

