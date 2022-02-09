using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
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
    public class ComplexDependency_ArrangeTemplateTests : TemplateTestBase
    {
        private class ComplexDependencyArrangeTemplate : TemplateBase
        {
            private readonly ComplexDependency _dependency;

            public ComplexDependencyArrangeTemplate(ComplexDependency dependency)
            {
                _dependency = dependency;
            }

            public override string TransformText()
            {
                WriteArrangeComplesDepencency(_dependency);

                return base.TransformText().Trim();
            }
        }


        private readonly MethodSignature ConcreteClass_GetPeer;

        private readonly TypeSignature ConcreteClassType;


        public ComplexDependency_ArrangeTemplateTests()
        {
            MethodTranslator methodTranslator = new MethodTranslator(DefinitionProvider);
            TypeTranslator typeTranslator = new TypeTranslator(DefinitionProvider);

            ConcreteClassType = typeTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ConcreteClass");

            ConcreteClass_GetPeer = methodTranslator.FromString("dnWalker.TestGenerator.Tests.Templates.ConcreteClass dnWalker.TestGenerator.Tests.Templates.ConcreteClass::GetPeer()");
        }

        [Fact]
        public void Test_FieldOfElementOf()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter objParam = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            IArrayParameter arrParam = set.CreateArrayParameter(ConcreteClassType, isNull: false, length: 1);

            objParam.SetField("_array", arrParam);
            arrParam.SetItem(0, objParam);

            ComplexDependency cd = new ComplexDependency(new SimpleDependency[] {new SimpleDependency(objParam), new SimpleDependency(arrParam)});

            string result = new ComplexDependencyArrangeTemplate(cd).TransformText();

            const string expected =
@"// Arrange variables:
//     var_0x00000001
//     var_0x00000002
// Create the instance of var_0x00000001
ConcreteClass var_0x00000001 = new ConcreteClass();

// Create the instance of var_0x00000002
ConcreteClass[] var_0x00000002 = new ConcreteClass[1];

// Initialize the instance of var_0x00000001
var_0x00000001.SetPrivate(""_array"", var_0x00000002);

// Initialize the instance of var_0x00000002
var_0x00000002[0] = var_0x00000001;";

            result.Should().Be(expected);
        }

        [Fact]
        public void Test_FieldOfSelf()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter objParam = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            objParam.SetField("_peer", objParam);


            ComplexDependency cd = new ComplexDependency(new SimpleDependency[] { new SimpleDependency(objParam) });

            string result = new ComplexDependencyArrangeTemplate(cd).TransformText();

            const string expected =
@"// Arrange variables:
//     var_0x00000001
// Create the instance of var_0x00000001
ConcreteClass var_0x00000001 = new ConcreteClass();

// Initialize the instance of var_0x00000001
var_0x00000001.SetPrivate(""_peer"", var_0x00000001);";

            result.Should().Be(expected);
        }

        [Fact]
        public void Test_MethodResultOfSelf()
        {
            IParameterContext context = new ParameterContext(DefinitionProvider);
            IParameterSet set = new ParameterSet(context);

            IObjectParameter objParam = set.CreateObjectParameter(ConcreteClassType, isNull: false);
            objParam.SetMethodResult(ConcreteClass_GetPeer, 0, objParam);


            ComplexDependency cd = new ComplexDependency(new SimpleDependency[] { new SimpleDependency(objParam) });

            string result = new ComplexDependencyArrangeTemplate(cd).TransformText();

            const string expected =
@"// Arrange variables:
//     var_0x00000001
// Create the instance of var_0x00000001
Mock<ConcreteClass> mock_var_0x00000001 = new Mock<ConcreteClass>();
ConcreteClass var_0x00000001 = mock_var_0x00000001.Object;

// Initialize the instance of var_0x00000001
mock_var_0x00000001.SetupSequence(o => o.GetPeer())
    .Returns(var_0x00000001);";

            result.Should().Be(expected);
        }
    }
}
