using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.TestClasses.Schemas
{
    public abstract class TestSchemaTestBase
    {
        protected readonly IMethod StaticMethod;
        protected readonly IMethod InstanceMethod;
        protected readonly TypeSig TestClassSig;
        protected readonly IDefinitionProvider DefinitionProvider;

        protected TestSchemaTestBase()
        {
            DefinitionProvider = TestUtils.DefinitionProvider;

            TypeDef testClassTD = DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.TestClasses.Schemas.ClassUnderTest");
            TestClassSig = testClassTD.ToTypeSig();

            StaticMethod = testClassTD.FindMethods("StaticMethod").First();
            InstanceMethod = testClassTD.FindMethods("InstanceMethod").First();

        }
    }
}
