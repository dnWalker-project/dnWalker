using dnlib.DotNet;

using dnWalker.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Parameter = dnWalker.Parameters.Parameter;

namespace dnWalker.Tests.Parameters
{
    public class ParameterFactory_ClassParameterTests : dnlibTypeTestBase
    {
        [Fact]
        public void Test_ParameterFor_MyClass_Is_ObjectParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(MyClass)));

            p.Should().BeOfType<ObjectParameter>();
        }
    }

    public class ParameterFactory_InterfaceParameterTests : dnlibTypeTestBase
    {
        [Fact]
        public void Test_ParameterFor_MyInterface_Is_ObjectParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(IMyInterface)));

            p.Should().BeOfType<InterfaceParameter>();
        }
    }

    public class ParameterFactory_ArrayParameterTests : dnlibTypeTestBase
    {

        [Fact]
        public void Test_ParameterFor_Array_Of_Interfaces_Is_ArrayParameter()
        {
            var type = GetType(typeof(IMyInterface[]));

            var p = ParameterFactory.CreateParameter(type);

            p.Should().BeOfType<ArrayParameter>();
        }

        [Fact]
        public void Test_ParameterFor_Array_Of_Objects_Is_ArrayParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(MyClass[])));

            p.Should().BeOfType<ArrayParameter>();
        }

        [Fact]
        public void Test_ParameterFor_Array_Of_Numbers_Is_ArrayParameter()
        {
            var p = ParameterFactory.CreateParameter(GetType(typeof(double[])));

            p.Should().BeOfType<ArrayParameter>();
        }
    }
}
