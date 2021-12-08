using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class MethodSignatureTests
    {
        [Theory]
        [InlineData("System.Void", "My.Namespace.MyClass", "MyMethod", new string[] { "System.Boolean", "System.Int32" })]
        [InlineData("System.Void", "My.Namespace.MyClass", "MyMethod", new string[] { "System.Boolean" })]
        [InlineData("System.Void", "My.Namespace.MyClass", "MyMethod", new string[] {})]
        public void Test_Constructor(string returnType, string declaringType, string methodName, string[] argumentTypes)
        {
            MethodSignature ms = new MethodSignature(returnType, declaringType, methodName, argumentTypes);

            ms.ReturnTypeFullName.Should().Be(returnType);
            ms.DeclaringTypeFullName.Should().Be(declaringType);
            ms.MethodName.Should().Be(methodName);
            ms.ArgumentTypeFullNames.Should().BeEquivalentTo(argumentTypes);
        }

        [Theory]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod(System.Boolean,System.Int32)")]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod(System.Boolean)")]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod()")]
        public void Test_ToString(string msString)
        {
            MethodSignature ms = msString;
            ms.ToString().Should().Be(msString);
        }

        [Theory]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod(System.Boolean,System.Int32)", "System.Void", "My.Namespace.MyClass", "MyMethod", new string[] { "System.Boolean", "System.Int32" })]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod(System.Boolean)", "System.Void", "My.Namespace.MyClass", "MyMethod", new string[] { "System.Boolean" })]
        [InlineData("System.Void My.Namespace.MyClass::MyMethod()", "System.Void", "My.Namespace.MyClass", "MyMethod", new string[] { })]
        public void Test_FromString(string msString, string returnType, string declaringType, string methodName, string[] argumentTypes)
        {
            MethodSignature ms = msString;

            ms.ReturnTypeFullName.Should().Be(returnType);
            ms.DeclaringTypeFullName.Should().Be(declaringType);
            ms.MethodName.Should().Be(methodName);
            ms.ArgumentTypeFullNames.Should().BeEquivalentTo(argumentTypes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("System.Void My.Namespace.MyClass(System.String,System.Int32)")]
        // verify that parts are valid full typanames?? [InlineData("System.Void My.Namespace.MyClass(System.String::System.Int32)")]
        public void Test_FromString_InvalidString_Throws(string msString)
        {
            Assert.Throws<FormatException>(() => MethodSignature.Parse(msString));
        }
    }
}
