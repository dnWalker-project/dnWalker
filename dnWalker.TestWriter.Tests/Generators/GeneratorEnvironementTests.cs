using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;
using dnWalker.TestWriter.Generators.Schemas.ReturnValue;
using dnWalker.TestWriter.TestModels;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;


namespace dnWalker.TestWriter.Tests.Generators
{
    public class GeneratorEnvironementTests : TestWriterTestBase
    {
        private class DummyClass
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
    }
}
