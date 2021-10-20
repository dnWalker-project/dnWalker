using dnlib.DotNet;

using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Parameter = dnWalker.Concolic.Parameters.Parameter;

namespace dnWalker.Tests.Concolic.Parameters
{
    public class ParameterFactory_ClassParameterTests : dnlibTypeTestBase
    {
        [Fact]
        public void Test_ParameterFor_MyClass_Is_ObjectParameter()
        {
            Parameter p = ParameterFactory.CreateParameter(GetType(typeof(MyClass)), "myClass");

            Assert.IsType<ObjectParameter>(p);
        }
    }

    public class ParameterFactory_InterfaceParameterTests : dnlibTypeTestBase
    {
        [Fact]
        public void Test_ParameterFor_MyInterface_Is_ObjectParameter()
        {
            Parameter p = ParameterFactory.CreateParameter(GetType(typeof(IMyInterface)), "myInterface");

            Assert.IsType<InterfaceParameter>(p);
        }
    }

    public class ParameterFactory_ArrayParameterTests : dnlibTypeTestBase
    {
        //[Fact(Skip = "Dont know how to properly get array type and how to process it...")]
        [Fact]
        public void Test_ParameterFor_Array_Of_Interfaces_Is_ArrayParameter()
        {
            TypeSig type = GetType(typeof(IMyInterface[]));

            Parameter p = ParameterFactory.CreateParameter(type, "myArray");

            Assert.IsType<ArrayParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Array_Of_Objects_Is_ArrayParameter()
        {
            Parameter p = ParameterFactory.CreateParameter(GetType(typeof(MyClass[])), "myArray");

            Assert.IsType<ArrayParameter>(p);
        }

        [Fact]
        public void Test_ParameterFor_Array_Of_Numbers_Is_ArrayParameter()
        {
            Parameter p = ParameterFactory.CreateParameter(GetType(typeof(Double[])), "myArray");

            Assert.IsType<ArrayParameter>(p);
        }
    }
}
