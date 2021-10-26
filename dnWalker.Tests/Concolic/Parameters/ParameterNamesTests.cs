﻿using dnWalker.Concolic.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ParameterNamesTests
    {
        [Theory]
        [InlineData(null, "NON_NULL")]
        [InlineData("NON_NULL", null)]
        [InlineData(null, null)]
        public void Test_GetAccessor_NullInputs(string baseName, string fullName)
        {
            Assert.Throws<ArgumentNullException>(() => ParameterName.GetAccessor(baseName, fullName));
        }

        [Theory]
        [InlineData("MY_OBJECT", "NOT_MY_OBJECT")]
        [InlineData("MY_OBJECT", "MY_OBJECT_NOT")]
        [InlineData("MY_OBJECT:ITS_FIELD", "MY_OBJECT")]
        [InlineData("ROOT_PARAMETER:ITS_FIELD:AND_A_SUBFIELD", "ROOT_PARAMETER:ITS_FIELD:AND_A_SUBFIELD")]
        public void Test_GetAccessor_Throws_If_BaseName_IsNotPartOf_FullName(string baseName, string fullName)
        {
            Assert.Throws<ArgumentException>(() => ParameterName.GetAccessor(baseName, fullName));
        }

        [Theory]
        [InlineData("", "ROOT_PARAMETER:ITS_FIELD:AND_A_SUBFIELD", "ROOT_PARAMETER")]
        [InlineData("ROOT_PARAMETER", "ROOT_PARAMETER:ITS_FIELD:AND_A_SUBFIELD", "ITS_FIELD")]
        [InlineData("ROOT_PARAMETER:ITS_FIELD", "ROOT_PARAMETER:ITS_FIELD:AND_A_SUBFIELD", "AND_A_SUBFIELD")]
        public void Test_GetAccessor(string baseName, string fullName, string expectedAccessor)
        {
            var accessor = ParameterName.GetAccessor(baseName, fullName);

            accessor.Should().BeEquivalentTo(expectedAccessor);
        }

        [Theory]
        [InlineData("some_object", null)]
        [InlineData(null, "some_field")]
        [InlineData(null, null)]
        [InlineData("", "some_field")]
        [InlineData(" ", "some_field")]
        [InlineData("some_object", "")]
        [InlineData("some_object", " ")]
        public void Test_ConstructField_NullOrEmptyOrWhiteSpaceInputs(string baseName, string fieldName)
        {
            Assert.Throws<ArgumentNullException>(() => ParameterName.ConstructField(baseName, fieldName));
        }

        [Theory]
        [InlineData("some_object", "some_field", "some_object:some_field")]
        [InlineData("some_object:a_sub_object", "some_field", "some_object:a_sub_object:some_field")]
        public void Test_ConstructField(string baseName, string fieldName, string expected)
        {
            var result = ParameterName.ConstructField(baseName, fieldName);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("some_object", null, 1)]
        [InlineData(null, "some_method", 1)]
        [InlineData(null, null, 1)]
        [InlineData("", "some_method", 1)]
        [InlineData(" ", "some_method", 1)]
        [InlineData("some_object", "", 1)]
        [InlineData("some_object", " ", 1)]
        [InlineData("some_object", null, 0)]
        [InlineData(null, "some_method", 0)]
        [InlineData(null, null, 0)]
        [InlineData("", "some_method", 0)]
        [InlineData(" ", "some_method", 0)]
        [InlineData("some_object", "", 0)]
        [InlineData("some_object", " ", 0)]
        public void Test_ConstructMethod_NullOrEmptyOrWhiteSpaceInputs(string baseName, string methodName, int callIndex)
        {
            Assert.Throws<ArgumentNullException>(() => ParameterName.ConstructMethod(baseName, methodName, callIndex));
        }

        [Theory]
        [InlineData("some_object", "some_method", 1, "some_object:some_method|1")]
        [InlineData("some_object:a_sub_object", "some_method", 2, "some_object:a_sub_object:some_method|2")]
        public void Test_ConstructMethod(string baseName, string methodName, int callIndex, string expected)
        {
            var result = ParameterName.ConstructMethod(baseName, methodName, callIndex);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_GetRootName_NullOrEmptyOrWhiteSpaceInput(string fullName)
        {
            Assert.Throws<ArgumentNullException>(() => ParameterName.GetRootName(fullName));
        }

        [Theory]
        [InlineData("ROOT_OBJECT", "ROOT_OBJECT")]
        [InlineData("ROOT_OBJECT:SOME_FIELD", "ROOT_OBJECT")]
        [InlineData("ROOT_OBJECT:SOME_METHOD|1", "ROOT_OBJECT")]
        public void Test_GetRootName(string fullName, string expected)
        {
            var result = ParameterName.GetRootName(fullName);

            result.Should().BeEquivalentTo(expected);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("IamNotAMethodBecausIContainADelimiter:my_method|1")]
        public void Test_TryParseMethodName_NullOrEmptyOrWhiteSpaceOrNonMethodNameInput(string methodNameWithCallIndex)
        {
            var result = ParameterName.TryParseMethodName(methodNameWithCallIndex, out var methodName, out var callIndex);
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData("my_method|1", "my_method", 1)]
        public void Test_TryParseMethodName(string methodNameWithCallIndex, string expectedMethodName, int expectedCallIndex)
        {
            var result = ParameterName.TryParseMethodName(methodNameWithCallIndex, out var methodName, out var callIndex);
            result.Should().BeTrue();
            methodName.Should().BeEquivalentTo(expectedMethodName);
            callIndex.Should().Be(expectedCallIndex);
        }
    }
}