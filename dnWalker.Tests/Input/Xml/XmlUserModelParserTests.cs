using dnlib.DotNet;

using dnWalker.Input;
using dnWalker.Input.Xml;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Xunit.Abstractions;

namespace dnWalker.Tests.Input.Xml
{
    public class XmlUserModelParserTests : DnlibTestBase
    {
        public class TestClass
        {
            public TestClass? Other;

            public int IntField;
            public string? StringProperty { get; }
            public bool BoolMethod() { return false; }

            public static string StringStaticField = "hello world";
            public static TestClass? ReferenceStaticField = null;

            public void TestMethod(int intArg, int[] arrayArg, string strArg)
            {

            }
        }

        private readonly XmlUserModelParser Parser;


        private readonly TypeDef TestTypeDef;
        private readonly TypeSig TestTypeSig;
        private readonly MethodDef TestMethod;

        public XmlUserModelParserTests(ITestOutputHelper textOutput) : base(textOutput)
        {
            Parser = new XmlUserModelParser(DefinitionProvider);
            TestTypeDef = DefinitionProvider.GetTypeDefinition("dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass");
            TestTypeSig = TestTypeDef.ToTypeSig();
            TestMethod = TestTypeDef.FindMethod(nameof(TestClass.TestMethod));
        }

        [Fact]
        public void ParseNoModel()
        {

            const string XmlText =
@"<UserModels>
</UserModels>";
            XElement xml = XElement.Parse(XmlText);

            IList<UserModel> models = Parser.ParseModelCollection(xml);

            models.Should().BeEmpty();
        }

        [Fact]
        public void ParseEmptyModel()
        {

            const string XmlText =
@"<UserModels>
	<SharedData/>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"" />
</UserModels>";
            XElement xml = XElement.Parse(XmlText);

            IList<UserModel> models = Parser.ParseModelCollection(xml);

            models.Should().HaveCount(1);
            models[0].Method.Should().BeEquivalentTo(TestMethod);
        }

        [Fact]
        public void ParseSharedData()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Literal Id=""MyString"" Type=""System.String"">Hello world!</Literal>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"" />
</UserModels>";

            XElement xml = XElement.Parse(XmlText);

            IList<UserModel> models = Parser.ParseModelCollection(xml);

            models[0].Data.Should().ContainKey("MyString");
            ((UserLiteral)models[0].Data["MyString"]).Value.Should().Be("Hello world!");
        }

        [Fact]
        public void ParseUserObject()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Object Id=""MyObject"" Type=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass"">
            <IntField>5</IntField>
            <StringProperty>Hello world!</StringProperty>
            <BoolMethod>true</BoolMethod>
            <BoolMethod>false</BoolMethod>
            <StringProperty Invocation=""3"">Hello world!</StringProperty>
            <BoolMethod Invocation=""5"">false</BoolMethod>
        </Object>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"" />
</UserModels>";

            XElement xml = XElement.Parse(XmlText);

            IList<UserModel> models = Parser.ParseModelCollection(xml);

            models[0].Data.TryGetValue("MyObject", out UserData? uValue).Should().BeTrue();
            UserObject obj = (UserObject)uValue!;


            // int field
            FieldDef intField = TestTypeDef.FindField(nameof(TestClass.IntField));
            obj.Fields[intField].Should().BeEquivalentTo(new UserLiteral() { Value = "5" });
            
            // bool method
            MethodDef boolMethod = TestTypeDef.FindMethod(nameof(TestClass.BoolMethod));
            obj.MethodResults[(boolMethod, 0)].Should().BeEquivalentTo(new UserLiteral() { Value = "true" });
            obj.MethodResults[(boolMethod, 1)].Should().BeEquivalentTo(new UserLiteral() { Value = "false" });
            obj.MethodResults.Should().NotContainKey((boolMethod, 2));
            obj.MethodResults.Should().NotContainKey((boolMethod, 3));
            obj.MethodResults.Should().NotContainKey((boolMethod, 4));
            obj.MethodResults[(boolMethod, 5)].Should().BeEquivalentTo(new UserLiteral() { Value = "false" });
            obj.MethodResults.Should().NotContainKey((boolMethod, 6));

            // string property
            MethodDef stringPropery = TestTypeDef.FindProperty(nameof(TestClass.StringProperty)).GetMethod;
            obj.MethodResults[(stringPropery, 0)].Should().BeEquivalentTo(new UserLiteral() { Value = "Hello world!" });
            obj.MethodResults.Should().NotContainKey((stringPropery, 1));
            obj.MethodResults.Should().NotContainKey((stringPropery, 2));
            obj.MethodResults[(stringPropery, 3)].Should().BeEquivalentTo(new UserLiteral() { Value = "Hello world!" });
            obj.MethodResults.Should().NotContainKey((stringPropery, 4));
        }

        [Fact]
        public void ParseUserArray()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Array Id=""MyArray"" ElementType=""System.String"" Length=""10"">
            <Literal Index=""2"">null</Literal>
            <Literal>foo</Literal>
            <Literal Index=""7"">bar</Literal>
        </Array>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"" />
</UserModels>";


            XElement xml = XElement.Parse(XmlText);

            IList<UserModel> models = Parser.ParseModelCollection(xml);

            models[0].Data.TryGetValue("MyArray", out UserData? uValue).Should().BeTrue();
            UserArray arr = (UserArray)uValue!;

            arr.Length.Should().Be(10);
            arr.Elements.Should().NotContainKey(0);
            arr.Elements.Should().NotContainKey(1);
            arr.Elements[2].Should().BeEquivalentTo(new UserLiteral("null"));
            arr.Elements[3].Should().BeEquivalentTo(new UserLiteral("foo"));
            arr.Elements.Should().NotContainKey(4);
            arr.Elements.Should().NotContainKey(5);
            arr.Elements.Should().NotContainKey(6);
            arr.Elements[7].Should().BeEquivalentTo(new UserLiteral("bar"));
            arr.Elements.Should().NotContainKey(8);
            arr.Elements.Should().NotContainKey(9);
        }

        [Fact]
        public void ParseMethodArguments()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Object Id=""MyObject"" Type=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass"">
            <IntField>5</IntField>
            <StringProperty>Hello world!</StringProperty>
            <BoolMethod>true</BoolMethod>
            <BoolMethod>false</BoolMethod>
            <StringProperty Invocation=""3"">Hello world!</StringProperty>
            <BoolMethod Invocation=""5"">false</BoolMethod>
        </Object>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"">
        <m-this>
            <Reference>MyObject</Reference>
        </m-this>
        <strArg>Hello world!</strArg>
    </UserModel>
</UserModels>";

            XElement xml = XElement.Parse(XmlText);

            UserModel model = Parser.ParseModelCollection(xml)[0];

            model.MethodArguments[TestMethod.Parameters[0]].Should().BeEquivalentTo(new UserReference("MyObject"));
            model.MethodArguments[TestMethod.Parameters[3]].Should().BeEquivalentTo(new UserLiteral("Hello world!"));
        }

        [Fact]
        public void ParseStaticFields()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Object Id=""MyObject"" Type=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass"">
            <IntField>5</IntField>
            <StringProperty>Hello world!</StringProperty>
            <BoolMethod>true</BoolMethod>
            <BoolMethod>false</BoolMethod>
            <StringProperty Invocation=""3"">Hello world!</StringProperty>
            <BoolMethod Invocation=""5"">false</BoolMethod>
        </Object>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"">
        <dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.StringStaticField>Hello world!</dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.StringStaticField>
        <dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.ReferenceStaticField>
            <Reference>MyObject</Reference>
        </dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.ReferenceStaticField>
    </UserModel>
</UserModels>";

            XElement xml = XElement.Parse(XmlText);

            UserModel model = Parser.ParseModelCollection(xml)[0];

            FieldDef stringStaticField = TestTypeDef.FindField(nameof(TestClass.StringStaticField));
            FieldDef referenceStaticField = TestTypeDef.FindField(nameof(TestClass.ReferenceStaticField));

            model.StaticFields[stringStaticField].Should().BeEquivalentTo(new UserLiteral("Hello world!"));
            model.StaticFields[referenceStaticField].Should().BeEquivalentTo(new UserReference("MyObject"));
        }

        [Fact]
        public void ParseMethodArgumentsAndStaticFields()
        {
            const string XmlText =
@"<UserModels>
	<SharedData>
        <Object Id=""MyObject"" Type=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass"">
            <IntField>5</IntField>
            <StringProperty>Hello world!</StringProperty>
            <BoolMethod>true</BoolMethod>
            <BoolMethod>false</BoolMethod>
            <StringProperty Invocation=""3"">Hello world!</StringProperty>
            <BoolMethod Invocation=""5"">false</BoolMethod>
        </Object>
    </SharedData>
	<UserModel EntryPoint=""dnWalker.Tests.Input.Xml.XmlUserModelParserTests/TestClass.TestMethod"">
        <m-this>
            <Reference>MyObject</Reference>
        </m-this>
        <strArg>Hello world!</strArg>
        <dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.StringStaticField>Hello world!</dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.StringStaticField>
        <dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.ReferenceStaticField>
            <Reference>MyObject</Reference>
        </dnWalker.Tests.Input.Xml.XmlUserModelParserTests-TestClass.ReferenceStaticField>
    </UserModel>
</UserModels>";

            XElement xml = XElement.Parse(XmlText);

            UserModel model = Parser.ParseModelCollection(xml)[0];

            model.MethodArguments[TestMethod.Parameters[0]].Should().BeEquivalentTo(new UserReference("MyObject"));
            model.MethodArguments[TestMethod.Parameters[3]].Should().BeEquivalentTo(new UserLiteral("Hello world!"));

            FieldDef stringStaticField = TestTypeDef.FindField(nameof(TestClass.StringStaticField));
            FieldDef referenceStaticField = TestTypeDef.FindField(nameof(TestClass.ReferenceStaticField));

            model.StaticFields[stringStaticField].Should().BeEquivalentTo(new UserLiteral("Hello world!"));
            model.StaticFields[referenceStaticField].Should().BeEquivalentTo(new UserReference("MyObject"));
        }
    }
}

