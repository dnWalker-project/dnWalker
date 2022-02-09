using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.TypeSystem.Tests.TestTypes;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TypeSystem.Tests
{
    public class MethodParserTests : TestBase
    {
        public static IMethod GetMethod(IList<Instruction> body)
        {
            return (body.FirstOrDefault(i => i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt)?.Operand as IMethod) ?? throw new Exception("Could not find a method invocation!");
        }

        [Theory]
        [InlineData(nameof(MethodsExporter.NonGenericClass_NonGenericMethod_NoArgs))]
        [InlineData(nameof(MethodsExporter.NonGenericClass_GenericMethod_NoArgs))]
        [InlineData(nameof(MethodsExporter.GenericClass_NonGenericMethod_NoArgs))]
        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_NoArgs))]

        [InlineData(nameof(MethodsExporter.NonGenericClass_NonGenericMethod_Arg))]
        [InlineData(nameof(MethodsExporter.NonGenericClass_GenericMethod_Concrete_Arg_NonAmbigous))]
        [InlineData(nameof(MethodsExporter.NonGenericClass_GenericMethod_Concrete_Arg_Ambigous))]
        [InlineData(nameof(MethodsExporter.NonGenericClass_GenericMethod_Generic_Arg))]

        [InlineData(nameof(MethodsExporter.GenericClass_NonGenericMethod_Arg))]
        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_Concrete_Arg_NonAmbigous))]
        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_Concrete_Arg_Ambigous))]
        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_Generic_Arg))]
        [InlineData(nameof(MethodsExporter.GenericClass_NonGenericMethod_ClassGeneric_Arg))]

        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_MethodClassGeneric_Arg))]
        [InlineData(nameof(MethodsExporter.GenericClass_GenericMethod_MethodClassGeneric_Concrete_Arg))]
        public void Test_MethodParsing(string methodName)
        {
            const string MethodsExporterName = "dnWalker.TypeSystem.Tests.TestTypes.MethodsExporter";

            TypeDef methodsExporterTD = DefinitionProvider.GetTypeDefinition(MethodsExporterName);
            MethodDef invokingMethod = methodsExporterTD.FindMethod(methodName);


            IMethod method = GetMethod(invokingMethod.Body.Instructions);
            string invokedMethodName = method.FullName;

            IMethod parsedMethod = new MethodParser(DefinitionProvider).Parse(invokedMethodName);

            MethodEqualityComparer.CompareDeclaringTypes.Equals(method, parsedMethod).Should().BeTrue();
        }
    }
}
