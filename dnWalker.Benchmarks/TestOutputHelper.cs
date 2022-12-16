using dnWalker.Explorations;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.TestModels;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;
using dnWalker.TestWriter.Generators.Schemas.ChangedArray;
using dnWalker.TestWriter.Generators.Schemas.ChangedObject;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;
using dnWalker.TestWriter.Generators.Schemas.ReturnValue;
using dnWalker.TestWriter.Moq;
using dnWalker.TestWriter.Xunit;
using dnWalker.TestWriter;
using dnWalker.TestWriter.TestWriters;

namespace dnWalker.Benchmarks
{
    public class TestOutputHelper
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly string _baseOutput;


        private readonly ITestTemplate _testTemplate;
        private readonly ITestSchemaProvider _testSchemaProvider;
        private readonly ITestFramework _testFramework;

        public TestOutputHelper(IDefinitionProvider definitionProvider, string baseOutput)
        {
            Directory.CreateDirectory(baseOutput);

            _definitionProvider = definitionProvider;
            _baseOutput = baseOutput;

            _testTemplate = new TestTemplate(
                new IArrangePrimitives[]
                {
                                new MoqArrange(),
                                new SimpleArrangePrimitives()
                },
                new IActPrimitives[]
                {
                                new SimpleActWriter(),
                },
                        new IAssertPrimitives[]
                        {
                                new XunitAssertions(),
                                new SimpleAssertPrimitives()
                });

            _testSchemaProvider = new MergedTestSchemaProvider(new ITestSchemaProvider[]
            {
                new ChangedObjectSchemaProvider(),
                new ChangedArraySchemaProvider(),
                new ReturnValueSchemaProvider(),
                new ExceptionSchemaProvider(),
            });

            _testFramework = new XunitFramework();
        }

        public TestClass GenerateTests(ConcolicExploration concolicExploration)
        {
            GeneratorEnvironment env = new GeneratorEnvironment(_testTemplate!, _testSchemaProvider!);

            TestProject testProject = env.GenerateTestProject(_testFramework!, concolicExploration);
            TestClass tc = env.GenerateTestClass(_testFramework!, testProject, concolicExploration);

            using (ITestClassWriter testClassWriter = new TestClassWriter(Path.Combine(_baseOutput, tc.Name + ".cs")))
            {
                testClassWriter.Write(tc);
            }

            return tc;
        }
    }
}
