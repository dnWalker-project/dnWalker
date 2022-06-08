using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestClasses.Schemas;
using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.TestClasses.Schemas
{
    public class ExceptionSchemaTests : TestSchemaTestBase
    {
        private readonly TypeSig NullReferenceExceptionSig;


        public ExceptionSchemaTests()
        {
            NullReferenceExceptionSig = DefinitionProvider.GetTypeDefinition("System.NullReferenceException").ToTypeSig();
        }

        [Fact]
        public void StaticNoExceptionThrown()
        {
            Model inputModel = new Model();
            
            IMethod testedMethod = StaticMethod;


            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = inputModel.Clone(),
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = testedMethod
            }.Build();

            GenerateTestClass(new ExceptionSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange method arguments",
                "int i = 0;",
                "string s = null;",
                "Action staticMethod = () => ClassUnderTest.StaticMethod(i, s);",
                "staticMethod.Should().NotThrow();"
                ));
        }

        [Fact]
        public void StaticExceptionThrown()
        {
            Model inputModel = new Model();

            IMethod testedMethod = StaticMethod;


            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = NullReferenceExceptionSig,
                InputModel = inputModel,
                OutputModel = inputModel.Clone(),
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = testedMethod
            }.Build();

            GenerateTestClass(new ExceptionSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange method arguments",
                "int i = 0;",
                "string s = null;",
                "Action staticMethod = () => ClassUnderTest.StaticMethod(i, s);",
                "staticMethod.Should().Throw<NullReferenceException>();"
                ));
        }

        [Fact]
        public void InstanceNoExceptionThrown()
        {
            IMethod testedMethod = InstanceMethod;

            Model inputModel = new Model();
            IObjectHeapNode testInstanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);

            inputModel.SetValue(new MethodArgumentVariable(testedMethod.ResolveMethodDefThrow().Parameters[0]), testInstanceNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = inputModel.Clone(),
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = testedMethod
            }.Build();

            GenerateTestClass(new ExceptionSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "string s = null;",
                "Action instanceMethod = () => objectUnderTest.InstanceMethod(i, s);",
                "instanceMethod.Should().NotThrow();"
                ));
        }

        [Fact]
        public void InstanceExceptionThrown()
        {
            IMethod testedMethod = InstanceMethod;

            Model inputModel = new Model();
            IObjectHeapNode testInstanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);

            inputModel.SetValue(new MethodArgumentVariable(testedMethod.ResolveMethodDefThrow().Parameters[0]), testInstanceNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = NullReferenceExceptionSig,
                InputModel = inputModel,
                OutputModel = inputModel.Clone(),
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = testedMethod
            }.Build();

            GenerateTestClass(new ExceptionSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "string s = null;",
                "Action instanceMethod = () => objectUnderTest.InstanceMethod(i, s);",
                "instanceMethod.Should().Throw<NullReferenceException>();"
                ));
        }
    }
}
