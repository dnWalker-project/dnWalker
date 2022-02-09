using dnWalker.Parameters;
using dnWalker.TestGenerator.Templates;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class MixedDependency_ArrangeTemplateTests : TemplateTestBase
    {
        private class MixedDependencyArrangeTemplate : TemplateBase 
        {
            private readonly IReadOnlyParameterSet _set;

            public MixedDependencyArrangeTemplate(IReadOnlyParameterSet set)
            {
                _set = set;
            }

            public override string TransformText()
            {
                WriteArrange(_set);

                return base.TransformText().Trim();
            }
        }


        private readonly MethodSignature ConcreteClass_GetPeer;

        private readonly TypeSignature ConcreteClassType;

        public MixedDependency_ArrangeTemplateTests()
        {
            MethodTranslator methodTranslator = new MethodTranslator(DefinitionProvider);
            TypeTranslator typeTranslator = new TypeTranslator(DefinitionProvider);

            ConcreteClassType = typeTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ConcreteClass");

            ConcreteClass_GetPeer = methodTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ConcreteClass dnWalker.TestGenerator.Tests.Templates.ConcreteClass::GetPeer()");
        }

        [Fact]
        public void Test_SimpleRoot_ComplexMiddle_SimpleSink()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter root = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            IObjectParameter c1 = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            IObjectParameter c2 = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            IObjectParameter sink = set.CreateObjectParameter(ConcreteClassType, isNull: false);

            // c1 is dependent on root
            c1.SetField("_peer", root);

            // c1 and c2 form a complex dependency - they are dependent on each other
            // c1 is dependent on c2 via method result - first invocation
            c1.SetMethodResult(ConcreteClass_GetPeer, 0, c2);

            // c2 is dependent on c1 via method result - second invocation
            c2.SetMethodResult(ConcreteClass_GetPeer, 1, c1);

            // sink is dependent on c2
            sink.SetField("_peer", c2);

            string result = new MixedDependencyArrangeTemplate(set).TransformText();

            const string expected =
@"// Arrange variable: var_0x00000001
ConcreteClass var_0x00000001 = new ConcreteClass();

// Arrange variables:
//     var_0x00000002
//     var_0x00000003
// Create the instance of var_0x00000002
Mock<ConcreteClass> mock_var_0x00000002 = new Mock<ConcreteClass>();
ConcreteClass var_0x00000002 = mock_var_0x00000002.Object;

// Create the instance of var_0x00000003
Mock<ConcreteClass> mock_var_0x00000003 = new Mock<ConcreteClass>();
ConcreteClass var_0x00000003 = mock_var_0x00000003.Object;

// Initialize the instance of var_0x00000002
mock_var_0x00000002.SetupSequence(o => o.GetPeer())
    .Returns(var_0x00000003);
var_0x00000002.SetPrivate(""_peer"", var_0x00000001);

// Initialize the instance of var_0x00000003
mock_var_0x00000003.SetupSequence(o => o.GetPeer())
    .Returns(null)
    .Returns(var_0x00000002);


// Arrange variable: var_0x00000004
ConcreteClass var_0x00000004 = new ConcreteClass();
var_0x00000004.SetPrivate(""_peer"", var_0x00000003);";

            result.Should().Be(expected);
        }


        // TODO: more complex tests?
    }
}
