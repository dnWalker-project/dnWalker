using dnlib.DotNet;

using dnWalker.TypeSystem.Tests.TestTypes;

using FluentAssertions;

using System;

using Xunit;

namespace dnWalker.TypeSystem.Tests
{
    public class TypeParserTests : TestBase
    {
        [Theory]
        [InlineData(nameof(TypesExporter.GlobalClassField))]
        [InlineData(nameof(TypesExporter.GlobalClassGen1_SimpleField))]
        [InlineData(nameof(TypesExporter.GlobalClassGen1_ComplexField))]
        [InlineData(nameof(TypesExporter.GlobalClassGen1_Complex2Field))]
        [InlineData(nameof(TypesExporter.GlobalClassArrayField))]

        [InlineData(nameof(TypesExporter.NamespaceClassField))]
        [InlineData(nameof(TypesExporter.NamespaceClassGen1_SimpleField))]
        [InlineData(nameof(TypesExporter.NamespaceClassGen1_ComplexField))]
        [InlineData(nameof(TypesExporter.NamespaceClassGen1_Complex2Field))]
        [InlineData(nameof(TypesExporter.NamespaceClassArrayField))]

        [InlineData(nameof(TypesExporter.GlobalNestedField))]
        [InlineData(nameof(TypesExporter.GlobalGenericNestedField))]
        [InlineData(nameof(TypesExporter.GlobalGenericNestedGenericField))]

        [InlineData(nameof(TypesExporter.NamespaceNestedField))]
        [InlineData(nameof(TypesExporter.NamespaceGenericNestedField))]
        [InlineData(nameof(TypesExporter.NamespaceGenericNestedGenericField))]
        public void Test_TypeParsing(string fieldName)
        {
            const string TypesExporterName = "dnWalker.TypeSystem.Tests.TestTypes.TypesExporter";

            TypeDef typesExporterTD = DefinitionProvider.GetTypeDefinition(TypesExporterName);
            FieldDef field = typesExporterTD.GetField(fieldName);

            string fieldTypeName = field.FieldType.FullName;

            TypeSig parsedFieldType = new TypeParser(DefinitionProvider).Parse(fieldTypeName);

            TypeEqualityComparer.Instance.Equals(parsedFieldType, field.FieldType).Should().BeTrue();
        }
    }
}
