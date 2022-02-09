using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TypeSystem.Tests
{
    public class TypeSignatureTests : TestBase
    {
        private readonly ITypeTranslator _translator;

        public TypeSignatureTests()
        {
            _translator = new TypeTranslator(DefinitionProvider);
        }

        [Theory]
        [InlineData("GlobalClass", "")]
        [InlineData("GlobalClass1`1<System.Boolean>", "")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", "dnWalker.TypeSystem.Tests.TestTypes")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", "dnWalker.TypeSystem.Tests.TestTypes")]
        public void Test_Namespace(string typeName, string namespaceName)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.Namespace.Should().Be(namespaceName);
        }

        [Theory]
        [InlineData("GlobalClass", "GlobalClass")]
        [InlineData("GlobalClass1`1<System.Int32>", "GlobalClass1`1")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", "NamespaceClass")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", "NamespaceClass1`1")]
        public void Test_Name(string typeName, string name)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("GlobalClass")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass")]
        public void Test_CreateSZArray(string typeName)
        {
            TypeSignature ts = _translator.FromString(typeName);
            TypeSignature tsArray = ts.CreateArray();

            tsArray.FullName.Should().Be(ts.ToString() + "[]");
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", false)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.ITestInterface", true)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", false)]
        public void Test_IsInterface(string typeName, bool isInterface)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.IsInterface.Should().Be(isInterface);
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", false)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.AbstractClass", true)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.ITestInterface", true)]
        public void Test_IsAbstract(string typeName, bool isAbstract)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.IsAbstract.Should().Be(isAbstract);
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", false)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", true)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1", true)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1/NestedClass", true)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1/NestedClass1`1", true)]
        public void Test_IsGeneric(string typeName, bool isGeneric)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.IsGeneric.Should().Be(isGeneric);
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass", false)]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", true)]
        public void Test_IsGenericInstance(string typeName, bool isGeneric)
        {
            TypeSignature ts = _translator.FromString(typeName);

            ts.IsGeneric.Should().Be(isGeneric);
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", "System.String")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass2`2<System.String, System.Boolean>", "System.String", "System.Boolean")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass2`2<System.Collections.Generic.List`1<System.String>, System.Boolean>", "System.Collections.Generic.List`1<System.String>", "System.Boolean")]
        public void Test_GetGenericParameters(string typeName, params string[] genericParams)
        {
            TypeSignature ts = _translator.FromString(typeName);

            TypeSignature[] genTS = genericParams.Select(s => _translator.FromString(s)).ToArray();

            ts.GetGenericParameters().Should().BeEquivalentTo(genTS);
        }

        [Theory]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1", "dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass1`1<System.String>", "System.String")]
        [InlineData("dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass2`2", "dnWalker.TypeSystem.Tests.TestTypes.NamespaceClass2`2<System.String,System.Boolean>", "System.String", "System.Boolean")]
        public void Test_CreateGenericInstance(string typeName, string genericInstanceName, params string[] genericParams)
        {
            TypeSignature ts = _translator.FromString(typeName);

            TypeSignature[] genTS = genericParams.Select(s => _translator.FromString(s)).ToArray();

            TypeSignature genericInstance = ts.CreateGenericInstance(genTS);

            genericInstance.FullName.Should().Be(genericInstanceName);
        }
    }
}
