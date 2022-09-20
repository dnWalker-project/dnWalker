using dnlib.DotNet;
using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Tests.Xml
{
    public class XmlModelSerializerTests
    {
        public class TestClass
        {
            public void TestMethod(double dblArg, int intArg, TestClass other, String strArg)
            {

            }

            public Double PrimitiveField;
            public TestClass? RefField;
            public Double[]? PrimitiveArrField;
            public TestClass[]? RefArrField;
            public String? StringField;

            public Double MockedMethod()
            {
                return Math.Ceiling(PrimitiveField);
            }
        }


        private readonly ModuleDef _testModule;
        private readonly TypeDef _testType;
        private readonly MethodDef _testMethod;

        public XmlModelSerializerTests()
        {
            ModuleContext ctx = ModuleDef.CreateModuleContext();
            _testModule = ModuleDefMD.Load(typeof(ModelExtensionsFormulaTests).Module, ctx);
            _testType = _testModule.FindThrow("dnWalker.Symbolic.Tests.XmlModelSerializerTests+TestClass", true);
            _testMethod = _testType.FindMethod("TestMethod");
        }


    }
}
