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

using static dnWalker.TestGenerator.Templates.MethodInvocationData;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class MethodInvocationTemplateTests : TemplateTestBase
    {
        public class MethodInvocationTemplateTest : TemplateBase
        {
            public override string TransformText()
            {
                if (_data.Instance == null)
                {
                    WriteStaticMethodInvocation(_data);
                }
                else
                {
                    WriteInstanceMethodInvocation(_data);
                }

                return base.TransformText();
            }

            private readonly MethodInvocationData _data;

            public MethodInvocationTemplateTest(MethodInvocationData data)
            {
                _data = data;
            }
        }


        [Theory]
        [MemberData(nameof(StaticMethodInvocations))]
        [MemberData(nameof(InstanceMethodInvocations))]
        public void Test_Method_StaticAndInstance_PositionalAndOptional(MethodInvocationData mid, string expected)
        {
            string result = new MethodInvocationTemplateTest(mid).TransformText().Trim();
            result.Should().Be(expected);
        }

        public static TheoryData<MethodInvocationData, string> StaticMethodInvocations()
        {
            IDefinitionProvider definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(MethodInvocationTemplateTests).Assembly));
            IMethodTranslator translator = new MethodTranslator(definitionProvider);

            TheoryData<MethodInvocationData, string> data = new TheoryData<MethodInvocationData, string>();

            // non generic class
            string result = "TestClass.StaticNonGenericMethod()";
            MethodInvocationData mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticNonGenericMethod()"));
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethod<string>()";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticGenericMethod<System.String>()"));
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithPositionalArguments(6, \"some data\")";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticNonGenericMethodWithPositionalArguments(System.Int32,System.String)"))
                .Positional("6")
                .Positional("\"some data\"");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithPositionalArguments<bool, int>(true, 5)";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticGenericMethodWithPositionalArguments<System.Boolean,System.Int32>(System.Boolean,System.Int32)"))
                .Positional("true")
                .Positional("5");
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithOptionalArguments(data: \"some data\")";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticNonGenericMethodWithOptionalArguments(System.Int32,System.String)"))
                .Optional("data", "\"some data\"");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithOptionalArguments<bool, int>(data: 5)";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticGenericMethodWithOptionalArguments<System.Boolean,System.Int32>(System.Boolean,System.Int32)"))
                .Optional("data", "5");
            data.Add(mid, result);

            result = "TestClass.StaticNonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticNonGenericMethodWithPositionalAndOptionalArguments(System.Int32,System.String)"))
                .Positional("1")
                .Optional("message", "\"Hello World!\"");
            data.Add(mid, result);

            result = "TestClass.StaticGenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            mid = Builder.GetStatic(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::StaticGenericMethodWithPositionalAndOptionalArguments<System.String>(System.String,System.String)"))
                .Positional("\"Good Bye!\"")
                .Optional("message", "\"Hello World!\"");
            data.Add(mid, result);

            // generic class
            //TypeDef genTestClassTD = definitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.GenericTestClass`1");
            
            //result = "GenericTestClass<NetTcpStyleUriParser>.StaticNonGenericMethod()";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric<NetTcpStyleUriParser>());
            //data.Add(mid, result);

            //result = "GenericTestClass<List<string>>.StaticGenericMethod<char>()";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric<List<string>, char>());
            //data.Add(mid, result);

            //result = "GenericTestClass<int>.StaticNonGenericMethodWithPositionalArguments(6, \"some data\")";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_Positional<int>())
            //    .Positional("6")
            //    .Positional("\"some data\"");
            //data.Add(mid, result);

            //result = "GenericTestClass<bool[]>.StaticGenericMethodWithPositionalArguments<double, int>(3.0, 5)";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_Positional<bool[], double, int>())
            //    .Positional("3.0")
            //    .Positional("5");
            //data.Add(mid, result);

            //result = "GenericTestClass<bool>.StaticNonGenericMethodWithOptionalArguments(data: \"some data\")";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_Optional<bool>())
            //    .Optional("data", "\"some data\"");
            //data.Add(mid, result);

            //result = "GenericTestClass<bool>.StaticGenericMethodWithOptionalArguments<char[], int>(data: 5)";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_Optional<bool, char[], int>())
            //    .Optional("data", "5");
            //data.Add(mid, result);

            //result = "GenericTestClass<int>.StaticNonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticNonGeneric_PositionalAndOptional<int>())
            //    .Positional("1")
            //    .Optional("message", "\"Hello World!\"");
            //data.Add(mid, result);

            //result = "GenericTestClass<bool>.StaticGenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            //mid = Builder.GetStatic(GenericTestClassMembers.GetStaticGeneric_PositionalAndOptional<bool, string>())
            //    .Positional("\"Good Bye!\"")
            //    .Optional("message", "\"Hello World!\"");
            //data.Add(mid, result);

            return data;
        }

        public static TheoryData<MethodInvocationData, string> InstanceMethodInvocations()
        {
            IDefinitionProvider definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(MethodInvocationTemplateTests).Assembly));
            IMethodTranslator translator = new MethodTranslator(definitionProvider);

            TheoryData<MethodInvocationData, string> data = new TheoryData<MethodInvocationData, string>();

            // non generic class

            string result = "instance.NonGenericMethod()";
            MethodInvocationData mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::NonGenericMethod()"), "instance");
            data.Add(mid, result);

            result = "instance.GenericMethod<string>()";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::GenericMethod<System.String>()"), "instance");
            data.Add(mid, result);

            result = "instance.NonGenericMethodWithPositionalArguments(6, \"some data\")";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::NonGenericMethodWithPositionalArguments(System.Int32,System.String)"), "instance")
                .Positional("6")
                .Positional("\"some data\"");
            data.Add(mid, result);

            result = "instance.GenericMethodWithPositionalArguments<bool, int>(true, 5)";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::GenericMethodWithPositionalArguments<System.Boolean,System.Int32>(System.Boolean,System.Int32)"), "instance")
                .Positional("true")
                .Positional("5");
            data.Add(mid, result);

            result = "instance.NonGenericMethodWithOptionalArguments(data: \"some data\")";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::NonGenericMethodWithOptionalArguments(System.Int32,System.String)"), "instance")
                .Optional("data", "\"some data\"");
            data.Add(mid, result);

            result = "instance.GenericMethodWithOptionalArguments<bool, int>(data: 5)";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::GenericMethodWithOptionalArguments<System.Boolean,System.Int32>(System.Boolean,System.Int32)"), "instance")
                .Optional("data", "5");
            data.Add(mid, result);

            result = "instance.NonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::NonGenericMethodWithPositionalAndOptionalArguments(System.Int32,System.String)"), "instance")
                .Positional("1")
                .Optional("message", "\"Hello World!\"");
            data.Add(mid, result);

            result = "instance.GenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            mid = Builder.GetInstance(translator.FromString("System.Void dnWalker.TestGenerator.Tests.Templates.TestClass::GenericMethodWithPositionalAndOptionalArguments<System.String>(System.String,System.String)"), "instance")
                .Positional("\"Good Bye!\"")
                .Optional("message", "\"Hello World!\"");
            data.Add(mid, result);


            // generic class
            //result = "instance.NonGenericMethod()";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetNonGeneric<NetTcpStyleUriParser>(), "instance");
            //data.Add(mid, result);

            //result = "instance.GenericMethod<char>()";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetGeneric<List<string>, char>(), "instance");
            //data.Add(mid, result);

            //result = "instance.NonGenericMethodWithPositionalArguments(6, \"some data\")";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetNonGeneric_Positional<int>(), "instance")
            //    .Positional("6")
            //    .Positional("\"some data\"");
            //data.Add(mid, result);

            //result = "instance.GenericMethodWithPositionalArguments<double, int>(3.0, 5)";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetGeneric_Positional<bool[], double, int>(), "instance")
            //    .Positional("3.0")
            //    .Positional("5");
            //data.Add(mid, result);

            //result = "instance.NonGenericMethodWithOptionalArguments(data: \"some data\")";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetNonGeneric_Optional<bool>(), "instance")
            //    .Optional("data", "\"some data\"");
            //data.Add(mid, result);

            //result = "instance.GenericMethodWithOptionalArguments<char[], int>(data: 5)";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetGeneric_Optional<bool, char[], int>(), "instance")
            //    .Optional("data", "5");
            //data.Add(mid, result);

            //result = "instance.NonGenericMethodWithPositionalAndOptionalArguments(1, message: \"Hello World!\")";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetNonGeneric_PositionalAndOptional<int>(), "instance")
            //    .Positional("1")
            //    .Optional("message", "\"Hello World!\"");
            //data.Add(mid, result);

            //result = "instance.GenericMethodWithPositionalAndOptionalArguments<string>(\"Good Bye!\", message: \"Hello World!\")";
            //mid = Builder.GetInstance(GenericTestClassMembers.GetGeneric_PositionalAndOptional<bool, string>(), "instance")
            //    .Positional("\"Good Bye!\"")
            //    .Optional("message", "\"Hello World!\"");
            //data.Add(mid, result);

            return data;
        }
    }
}
