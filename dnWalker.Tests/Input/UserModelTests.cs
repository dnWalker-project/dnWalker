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
    public class UserModelTests : DnlibTestBase
    {
        public class TestClass
        {
            public TestClass? Other;

            public int Field;
            public string? Property { get; }
            public bool Method() { return false; }

            public static string StaticField = "hello world";

            public void TestMethod(int intArg, int[] arrayArg, string strArg)
            {

            }
        }

        private readonly TypeDef TestTypeDef;
        private readonly TypeSig TestTypeSig;
        private readonly MethodDef TestMethod;

        public UserModelTests(ITestOutputHelper textOutput) : base(textOutput)
        {
            TestTypeDef = DefinitionProvider.GetTypeDefinition("dnWalker.Tests.Input.UserModelTests/TestClass");
            TestTypeSig = TestTypeDef.ToTypeSig();
            TestMethod = TestTypeDef.FindMethod(nameof(TestClass.TestMethod));
        }


        [Fact]
        public void BuildEmpty()
        {
            UserModel userModel = new UserModel();

            IModel model = userModel.Build();

            model.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void BuildWithStatidFields()
        {
            const string StaticFieldValue = "\"static field value\"";

            UserModel userModel = new UserModel();

            FieldDef staticField = TestTypeDef.FindField(nameof(TestClass.StaticField));
            userModel.StaticFields[staticField] = new UserLiteral(StaticFieldValue);

            IModel model = userModel.Build();

            model.IsEmpty().Should().BeFalse();
            model.TryGetValue(Variable.StaticField(staticField), out IValue? value).Should().BeTrue();
            ((StringValue)value!).Content.Should().Be(StaticFieldValue.Trim('"'));
        }

        [Fact]
        public void BuildWithMethodArgs()
        {
            const int IntArgValue = -111;

            UserModel userModel = new UserModel();
            Parameter intArgParameter = TestMethod.Parameters.First(p => p.Name == "intArg");
            userModel.MethodArguments[intArgParameter] = new UserLiteral(IntArgValue.ToString());

            IModel model = userModel.Build();

            model.IsEmpty().Should().BeFalse();
            model.TryGetValue(Variable.MethodArgument(intArgParameter), out IValue? value).Should().BeTrue();
            ((PrimitiveValue<int>)value!).Value.Should().Be(IntArgValue);
        }

        [Fact]
        public void BuildWithMethodArgsAndStaticFields()
        {
            const string StaticFieldValue = "\"static field value\"";
            const int IntArgValue = -111;

            UserModel userModel = new UserModel();

            FieldDef staticField = TestTypeDef.FindField(nameof(TestClass.StaticField));
            userModel.StaticFields[staticField] = new UserLiteral(StaticFieldValue);

            Parameter intArgParameter = TestMethod.Parameters.First(p => p.Name == "intArg");
            userModel.MethodArguments[intArgParameter] = new UserLiteral(IntArgValue.ToString());


            IModel model = userModel.Build();

            model.IsEmpty().Should().BeFalse();
            model.TryGetValue(Variable.StaticField(staticField), out IValue? value).Should().BeTrue();
            ((StringValue)value!).Content.Should().Be(StaticFieldValue.Trim('"'));

            model.TryGetValue(Variable.MethodArgument(intArgParameter), out value).Should().BeTrue();
            ((PrimitiveValue<int>)value!).Value.Should().Be(IntArgValue);
        }

        [Fact]
        public void BuildWithMethodArgsAndStaticFieldsAndTopLevelReferences()
        {
            const string StaticFieldValue = "\"static field value\"";
            const int IntArgValue = -111;

            UserModel userModel = new UserModel();

            userModel.Data["MyString"] = new UserLiteral(StaticFieldValue) { Type = DefinitionProvider.BaseTypes.String };
            userModel.Data["MyIntArray"] = new UserLiteral(null) { Type = new SZArraySig(DefinitionProvider.BaseTypes.Int32) };

            FieldDef staticField = TestTypeDef.FindField(nameof(TestClass.StaticField));
            userModel.StaticFields[staticField] = new UserReference("MyString");

            Parameter intArgParameter = TestMethod.Parameters.First(p => p.Name == "intArg");
            userModel.MethodArguments[intArgParameter] = new UserLiteral(IntArgValue.ToString());

            Parameter arrayArgParameter = TestMethod.Parameters.First(p => p.Name == "arrayArg");
            userModel.MethodArguments[arrayArgParameter] = new UserReference("MyIntArray");


            IModel model = userModel.Build();

            model.IsEmpty().Should().BeFalse();
            model.TryGetValue(Variable.StaticField(staticField), out IValue? value).Should().BeTrue();
            ((StringValue)value!).Content.Should().Be(StaticFieldValue.Trim('"'));

            model.TryGetValue(Variable.MethodArgument(intArgParameter), out value).Should().BeTrue();
            ((PrimitiveValue<int>)value!).Value.Should().Be(IntArgValue);

            model.TryGetValue(Variable.MethodArgument(arrayArgParameter), out value).Should().BeTrue();
            ((Location)value!).Should().Be(Location.Null);
        }

        [Fact]
        public void BuildWithMethodArgsAndStaticFieldsAndUpstreamReferences()
        {

            UserModel userModel = new UserModel();

            FieldDef otherField = TestTypeDef.FindField(nameof(TestClass.Other));
            Parameter thisArg = TestMethod.Parameters[0];

            UserObject theInstance = new UserObject { Id = "thisInstance" };
            theInstance.Fields[otherField] = new UserReference("thisInstance");

            userModel.MethodArguments[thisArg] = theInstance;

            IModel model = userModel.Build();

            model.TryGetValue(Variable.MethodArgument(thisArg), out IValue? value).Should().BeTrue();
            Location thisLocation = (Location)value!;

            model.HeapInfo.TryGetNode(thisLocation, out IHeapNode? node).Should().BeTrue();
            IObjectHeapNode objectNode = (IObjectHeapNode)node!;

            objectNode.TryGetField(otherField, out value).Should().BeTrue();
            value.Should().Be(thisLocation);
        }
    }
}
