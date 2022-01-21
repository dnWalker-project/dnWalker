using dnWalker.TestGenerator.Templates;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using static dnWalker.TestGenerator.Templates.MethodInvocationData;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class MethodInvocationTemplateTests
    {
        [Theory]
        [MemberData(nameof(StaticMethodInvocations))]
        public void Test_StaticNonGenericMethod_PositionalAndOptional(MethodInvocationData mid, string expected)
        {
            StringBuilder sb = new StringBuilder();
            new StaticMethodInvocation(mid).WriteTo(sb);
            sb.ToString().Should().Be(expected);
        }

        public static TheoryData<MethodInvocationData, string> StaticMethodInvocations()
        {
            TheoryData<MethodInvocationData, string> data = new TheoryData<MethodInvocationData, string>();

            // non generic class
            string result = "TestClass.StaticNonGenericMethod()";
            MethodInvocationData mid = Builder.GetStatic(TestClassMembers.GetStaticNonGeneric());
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethod<string>()";
            mid = Builder.GetStatic(TestClassMembers.GetStaticGeneric<string>());
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithPositionalArguments(6, \"some data\")";
            mid = Builder.GetStatic(TestClassMembers.GetStaticNonGeneric_Positional())
                .Positional<RawCode>("6")
                .Positional<RawCode>("\"some data\"");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithPositionalArguments<bool, int>(true, 5)";
            mid = Builder.GetStatic(TestClassMembers.GetStaticGeneric_Positional<bool, int>())
                .Positional<RawCode>("true")
                .Positional<RawCode>("5");
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithOptionalArguments(data: \"some data\")";
            mid = Builder.GetStatic(TestClassMembers.GetStaticNonGeneric_Optional())
                .Optional<StringLiteral>("data", "some data");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithOptionalArguments<bool, int>(data: 5)";
            mid = Builder.GetStatic(TestClassMembers.GetStaticGeneric_Optional<bool, int>())
                .Optional<RawCode>("data", "5");
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            mid = Builder.GetStatic(TestClassMembers.GetStaticNonGeneric_PositionalAndOptional())
                .Positional<RawCode>("1")
                .Optional<StringLiteral>("message", "Hello World!");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            mid = Builder.GetStatic(TestClassMembers.GetStaticGeneric_PositionalAndOptional<string>())
                .Positional<StringLiteral>("Good Bye!")
                .Optional<StringLiteral>("message", "Hello World!");
            data.Add(mid, result);


            // generic class
            result = "GenericTestClass<NetTcpStyleUriParser>.StaticNonGenericMethod()";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric<NetTcpStyleUriParser>());
            data.Add(mid, result);

            result = "GenericTestClass<List<string>>.StaticGenericMethod<char>()";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric<List<string>, char>());
            data.Add(mid, result);

            result = "GenericTestClass<int>.StaticNonGenericMethodWithPositionalArguments(6, \"some data\")";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_Positional<int>())
                .Positional<RawCode>("6")
                .Positional<RawCode>("\"some data\"");
            data.Add(mid, result);

            result = "GenericTestClass<bool[]>.StaticGenericMethodWithPositionalArguments<double, int>(3.0, 5)";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_Positional<bool[], double, int>())
                .Positional<RawCode>("3.0")
                .Positional<RawCode>("5");
            data.Add(mid, result);

            result = "GenericTestClass<bool>.StaticNonGenericMethodWithOptionalArguments(data: \"some data\")";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_Optional<bool>())
                .Optional<StringLiteral>("data", "some data");
            data.Add(mid, result);

            result = "GenericTestClass<bool>.StaticGenericMethodWithOptionalArguments<char[], int>(data: 5)";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_Optional<bool, char[], int>())
                .Optional<RawCode>("data", "5");
            data.Add(mid, result);

            result = "GenericTestClass<int>.StaticNonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_PositionalAndOptional<int>())
                .Positional<RawCode>("1")
                .Optional<StringLiteral>("message", "Hello World!");
            data.Add(mid, result);

            result = "GenericTestClass<bool>.StaticGenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_PositionalAndOptional<bool, string>())
                .Positional<StringLiteral>("Good Bye!")
                .Optional<StringLiteral>("message", "Hello World!");
            data.Add(mid, result);

            return data;
        }
    }
}
