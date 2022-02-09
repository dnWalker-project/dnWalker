using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TypeSystem.Tests
{
    public class MethodSignatureTests : TestBase
    {
        private readonly IMethodTranslator _translator;

        public MethodSignatureTests()
        {
            _translator = new MethodTranslator(DefinitionProvider);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", "Method")]
        public void Test_Name(string fullName, string name)
        {
            MethodSignature ms = _translator.FromString(fullName);
            ms.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        [InlineData("System.String dnWalker.TypeSystem.Tests.TestTypes.AbstractClass::StringMethod()", true)]
        public void Test_IsAbstract(string fullName, bool isAbstract)
        {
            MethodSignature ms = _translator.FromString(fullName);
            ms.IsAbstract.Should().Be(isAbstract);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        //[InlineData("System.Void GlobalClass::Method<V>()", true)]
        [InlineData("System.Void GlobalClass::Method<System.String>()", true)]
        public void Test_IsGeneric(string fullName, bool isGeneric)
        {
            MethodSignature ms = _translator.FromString(fullName);
            ms.IsGeneric.Should().Be(isGeneric);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        //[InlineData("System.Void GlobalClass::Method<V>()", false)]
        [InlineData("System.Void GlobalClass::Method<System.String>()", true)]
        public void Test_IsGenericInstance(string fullName, bool isGenericInstance)
        {
            MethodSignature ms = _translator.FromString(fullName);
            ms.IsGenericInstance.Should().Be(isGenericInstance);
        }
    }
}
