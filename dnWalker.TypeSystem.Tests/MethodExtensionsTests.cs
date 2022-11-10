using dnlib.DotNet;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TypeSystem.Tests
{
    public class MethodExtensionsTests : TestBase
    {
        private readonly IMethodParser _methodParser;

        public MethodExtensionsTests()
        {
            _methodParser = new MethodParser(DefinitionProvider);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", "Method")]
        public void Test_Name(string fullName, string name)
        {
            IMethod ms = _methodParser.Parse(fullName);
            ms.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        [InlineData("System.String dnWalker.TypeSystem.Tests.TestTypes.AbstractClass::StringMethod()", true)]
        public void Test_IsAbstract(string fullName, bool isAbstract)
        {
            IMethod ms = _methodParser.Parse(fullName);
            ms.ResolveMethodDefThrow().IsAbstract.Should().Be(isAbstract);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        //[InlineData("System.Void GlobalClass::Method<V>()", true)]
        [InlineData("System.Void GlobalClass::Method<System.String>()", true)]
        public void Test_IsGeneric(string fullName, bool isGeneric)
        {
            IMethod ms = _methodParser.Parse(fullName);
            ms.IsGenericMethod().Should().Be(isGeneric);
        }

        [Theory]
        [InlineData("System.Void GlobalClass::Method()", false)]
        //[InlineData("System.Void GlobalClass::Method<V>()", false)]
        [InlineData("System.Void GlobalClass::Method<System.String>()", true)]
        public void Test_IsGenericInstance(string fullName, bool isGenericInstance)
        {
            IMethod ms = _methodParser.Parse(fullName);
            ms.IsGenericMethod().Should().Be(isGenericInstance);
        }
    }
}
