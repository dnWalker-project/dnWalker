using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;
using dnWalker.TestWriter.Generators.Schemas.ReturnValue;
using dnWalker.TestWriter.TestModels;

using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;


namespace dnWalker.TestWriter.Tests.Generators
{
    public class GeneratorEnvironementTests : TestWriterTestBase
    {
        [AddPackage("arrange package1")]
        [AddPackage("arrange package2")]
        [AddNamespace("arrange.ns1")]
        [AddNamespace("arrange.ns2")]
        public abstract class DummyArrangePrimitives : IArrangePrimitives
        {
            public abstract bool TryWriteArrangeCreateInstance(ITestContext testContext, IWriter output, string symbol);
            public abstract bool TryWriteArrangeInitializeField(ITestContext testContext, IWriter output, string symbol, IField field, string literal);
            public abstract bool TryWriteArrangeInitializeStaticField(ITestContext testContext, IWriter output, IField field, string literal);
            public abstract bool TryWriteArrangeInitializeMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<string> literals);
            public abstract bool TryWriteArrangeInitializeConstrainedMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<KeyValuePair<Expression, string>> constrainedLiterals, string fallbackLiteral);
            public abstract bool TryWriteArrangeInitializeStaticMethod(ITestContext testContext, IWriter output, IMethod method, IReadOnlyList<string> literals);
            public abstract bool TryWriteArrangeInitializeArrayElement(ITestContext testContext, IWriter output, string symbol, int index, string literal);
        }

        [AddPackage("act package1")]
        [AddPackage("act package2")]
        [AddNamespace("act.ns1")]
        [AddNamespace("act.ns2")]
        public abstract class DummyActPrimitives : IActPrimitives
        {
            public abstract bool TryWriteAct(ITestContext context, IWriter output, string? returnSymbol = null);
            public abstract bool TryWriteActDelegate(ITestContext context, IWriter output, string? returnSymbol = null, string delegateSymbol = "act");
        }

        [AddPackage("assert package1")]
        [AddPackage("assert package2")]
        [AddNamespace("assert.ns1")]
        [AddNamespace("assert.ns2")]
        public abstract class DummyAssertPrimitives : IAssertPrimitives
        {
            public abstract bool TryWriteAssertNull(ITestContext context, IWriter output, string symbol);
            public abstract bool TryWriteAssertNotNull(ITestContext context, IWriter output, string symbol);
            public abstract bool TryWriteAssertEqual(ITestContext context, IWriter output, string actual, string expected);
            public abstract bool TryWriteAssertNotEqual(ITestContext context, IWriter output, string actual, string expected);
            public abstract bool TryWriteAssertSame(ITestContext context, IWriter output, string actual, string expected);
            public abstract bool TryWriteAssertNotSame(ITestContext context, IWriter output, string actual, string expected);
            public abstract bool TryWriteAssertExceptionThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType);
            public abstract bool TryWriteAssertExceptionNotThrown(ITestContext context, IWriter output, string delegateSymbol, string exceptionType);
            public abstract bool TryWriteAssertNoExceptionThrown(ITestContext context, IWriter output, string delegateSymbol);
            public abstract bool TryWriteAssertOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType);
            public abstract bool TryWriteAssertNotOfType(ITestContext context, IWriter output, string objectSymbol, string expectedType);
            public abstract bool TryWriteAssertEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol);
            public abstract bool TryWriteAssertNotEquivalent(ITestContext context, IWriter output, string objectSymbol, string expetedSymbol);
        }

        [AddPackage("framework package1")]
        [AddPackage("framework package2")]
        public abstract class DummyTestFramework : ITestFramework
        {
            public abstract TestProject CreateTestProject(string name, IEnumerable<ConcolicExploration> explorations);
            public abstract TestClass CreateTestClass(TestProject testProject, TestGroup testGroup, ConcolicExploration exploration);
            public abstract TestMethod CreateTestMethod(TestClass testClass, ConcolicExplorationIteration explorationIteration, ITestSchema testSchema);
        }

        public class DummyClass
        {
            public int X;

            public int Method(StringWriter writer, Uri uri) => X;
        }

        public GeneratorEnvironementTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void GenerateTestClassUsingsTests()
        {
            ITestFramework testFramework = new Xunit.XunitFramework();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchemaProvider testSchemaProvider = new MergedTestSchemaProvider
                (
                    new ITestSchemaProvider[]
                    {
                        new ExceptionSchemaProvider(),
                        new ReturnValueSchemaProvider()
                    });

            GeneratorEnvironment env = new GeneratorEnvironment(testTemplate, testSchemaProvider);

            TypeDef theType = GetType(typeof(DummyClass));
            MethodDef theMethod = GetMethod(typeof(DummyClass), nameof(DummyClass.Method));

            ConcolicExploration concolicExploration = GetExploration(theMethod,
                null, new Action<ConcolicExplorationIteration.Builder>[]
                {
                    b =>
                    {
                        Model im = new Model();

                        IObjectHeapNode thisNode = im.HeapInfo.InitializeObject(GetType(typeof(DummyClass)).ToTypeSig());
                        IObjectHeapNode writerNode = im.HeapInfo.InitializeObject(GetType(typeof(StringWriter)).ToTypeSig());
                        IObjectHeapNode uriNode = im.HeapInfo.InitializeObject(GetType(typeof(Uri)).ToTypeSig());

                        thisNode.SetField(theType.FindField(nameof(DummyClass.X)), ValueFactory.GetValue(5));

                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[0]), thisNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[1]), writerNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[2]), uriNode.Location);

                        b.InputModel= im;

                        Model om = im.Clone();
                        om.SetValue(new ReturnValueVariable(theMethod), ValueFactory.GetValue(5));
                        b.OutputModel = om;
                    }
                });
            TestProject testProject = testFramework.CreateTestProject("test", new[] { concolicExploration });

            TestClass testClass = env.GenerateTestClass(testFramework, testProject, concolicExploration);

            testClass.Usings.Should().Contain(new[]
            {
                // test framework
                "Xunit",
                // part of the template - not used in this case but the generator is not smart enough to know it yet...
                "Moq",
                // the tested data
                "System",
                "System.IO",
                "dnWalker.TestWriter.Tests.Generators"
            });
        }

        [Fact]
        public void GenerateTestClassMethodsTests()
        {
            ITestFramework testFramework = new Xunit.XunitFramework();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchemaProvider testSchemaProvider = new MergedTestSchemaProvider
                (
                    new ITestSchemaProvider[]
                    {
                        new ExceptionSchemaProvider(),
                        new ReturnValueSchemaProvider()
                    });

            GeneratorEnvironment env = new GeneratorEnvironment(testTemplate, testSchemaProvider);

            TypeDef theType = GetType(typeof(DummyClass));
            MethodDef theMethod = GetMethod(typeof(DummyClass), nameof(DummyClass.Method));

            ConcolicExploration concolicExploration = GetExploration(theMethod,
                null, new Action<ConcolicExplorationIteration.Builder>[]
                {
                    b =>
                    {
                        Model im = new Model();

                        IObjectHeapNode thisNode = im.HeapInfo.InitializeObject(GetType(typeof(DummyClass)).ToTypeSig());
                        IObjectHeapNode writerNode = im.HeapInfo.InitializeObject(GetType(typeof(StringWriter)).ToTypeSig());
                        IObjectHeapNode uriNode = im.HeapInfo.InitializeObject(GetType(typeof(Uri)).ToTypeSig());

                        thisNode.SetField(theType.FindField(nameof(DummyClass.X)), ValueFactory.GetValue(5));

                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[0]), thisNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[1]), writerNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[2]), uriNode.Location);

                        b.InputModel= im;

                        Model om = im.Clone();
                        om.SetValue(new ReturnValueVariable(theMethod), ValueFactory.GetValue(5));
                        b.OutputModel = om;
                    },
                    b =>
                    {
                        Model im = new Model();

                        IObjectHeapNode thisNode = im.HeapInfo.InitializeObject(GetType(typeof(DummyClass)).ToTypeSig());
                        IObjectHeapNode writerNode = im.HeapInfo.InitializeObject(GetType(typeof(StringWriter)).ToTypeSig());
                        IObjectHeapNode uriNode = im.HeapInfo.InitializeObject(GetType(typeof(Uri)).ToTypeSig());

                        thisNode.SetField(theType.FindField(nameof(DummyClass.X)), ValueFactory.GetValue(4));

                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[0]), thisNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[1]), writerNode.Location);
                        im.SetValue((IRootVariable)Variable.MethodArgument(theMethod.Parameters[2]), uriNode.Location);

                        b.InputModel= im;

                        Model om = im.Clone();
                        b.OutputModel = om;

                        b.Exception = GetType(typeof(InvalidOperationException)).ToTypeSig();
                    }
                });

            TestProject testProject = testFramework.CreateTestProject("test", new[] { concolicExploration });
            TestClass testClass = env.GenerateTestClass(testFramework, testProject, concolicExploration);

            // 2 iterations and 1 test per iteration - exception and then return value
            testClass.Methods.Should().HaveCount(2);
        }

        [Fact]
        public void GenerateTestProject_IncludedPackages_FromPrimitives()
        {
            IArrangePrimitives arrPrimMock = Mock.Of<DummyArrangePrimitives>();
            IActPrimitives actPrimMock = Mock.Of<DummyActPrimitives>();
            IAssertPrimitives assPrimMock = Mock.Of<DummyAssertPrimitives>();

            ITestSchemaProvider testSchemaProvider = Mock.Of<ITestSchemaProvider>();
            Mock.Get(testSchemaProvider)
                .Setup(prv => prv.GetSchemas(It.IsAny<ConcolicExplorationIteration>()))
                .Returns(Array.Empty<ITestSchema>());

            GeneratorEnvironment genEnv = new GeneratorEnvironment(new TestTemplate(new[] { arrPrimMock }, new[] { actPrimMock }, new[] {assPrimMock}), testSchemaProvider);

            ITestFramework testFramework = Mock.Of<ITestFramework>();
            Mock.Get(testFramework)
                .Setup(tf => tf.CreateTestProject(It.IsAny<string>(), It.IsAny<IEnumerable<ConcolicExploration>>()))
                .Returns(() => new TestProject());

            TestProject testProject = genEnv.GenerateTestProject(testFramework, GetExploration(GetMethod(typeof(DummyClass), "Method")));

            testProject.Packages.Select(pr => pr.Name).Should().Contain(
                new string[] 
                {
                    "arrange package1",
                    "arrange package2",
                    "act package1",
                    "act package2",
                    "assert package1",
                    "assert package2"
                });
        }
        [Fact]
        public void GenerateTestProject_IncludedPackages_FromFramework()
        {
            IArrangePrimitives arrPrimMock = Mock.Of<IArrangePrimitives>();
            IActPrimitives actPrimMock = Mock.Of<IActPrimitives>();
            IAssertPrimitives assPrimMock = Mock.Of<IAssertPrimitives>();

            ITestSchemaProvider testSchemaProvider = Mock.Of<ITestSchemaProvider>();
            Mock.Get(testSchemaProvider)
                .Setup(prv => prv.GetSchemas(It.IsAny<ConcolicExplorationIteration>()))
                .Returns(Array.Empty<ITestSchema>());

            GeneratorEnvironment genEnv = new GeneratorEnvironment(new TestTemplate(new[] { arrPrimMock }, new[] { actPrimMock }, new[] { assPrimMock }), testSchemaProvider);

            ITestFramework testFramework = Mock.Of<DummyTestFramework>();
            Mock.Get(testFramework)
                .Setup(tf => tf.CreateTestProject(It.IsAny<string>(), It.IsAny<IEnumerable<ConcolicExploration>>()))
                .Returns(() => new TestProject());

            TestProject testProject = genEnv.GenerateTestProject(testFramework, GetExploration(GetMethod(typeof(DummyClass), "Method")));

            testProject.Packages.Select(pr => pr.Name).Should().Contain(
                new string[]
                {
                    "framework package1",
                    "framework package2"
                });
        }
    }
}
