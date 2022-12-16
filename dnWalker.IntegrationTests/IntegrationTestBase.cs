using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Explorations;
using dnWalker.Input;
using dnWalker.Symbolic;
using dnWalker.Tests.Examples;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.Generators.Schemas.ChangedArray;
using dnWalker.TestWriter.Generators.Schemas.ChangedObject;
using dnWalker.TestWriter.Generators.Schemas.Exceptions;
using dnWalker.TestWriter.Generators.Schemas.ReturnValue;
using dnWalker.TestWriter.Moq;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.TestWriters;
using dnWalker.TestWriter.Xunit;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

using ITestFramework = dnWalker.TestWriter.ITestFramework;

namespace dnWalker.IntegrationTests
{
    public abstract class IntegrationTestBase : ExamplesTestBase, IDisposable
    {
        private readonly string _outDir;
        protected IntegrationTestBase(ITestOutputHelper output) : base(output)
        {
            //_outDir = Path.GetFullPath($"{Random.Shared.Next()}");
            _outDir = Path.GetFullPath($"{Random.Shared.Next()}");
            Directory.CreateDirectory(_outDir);
        }

        public void Dispose()
        {
            Directory.Delete(_outDir, true);
        }

        private ITestTemplate? _testTemplate;
        private ITestSchemaProvider? _testSchemaProvider;
        private ITestFramework? _testFramework;

        [MemberNotNull(nameof(_testTemplate), nameof(_testSchemaProvider), nameof(_testFramework))]
        protected override void Initialize(BuildInfo buildInfo)
        {
            base.Initialize(buildInfo);

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

        protected virtual ExplorationResult Explore<TStrategy>(string methodName)
            where TStrategy : IExplorationStrategy, new()
        {
            IExplorer explorer = CreateExplorer();
            return explorer.Run(DefinitionProvider.GetMethodDefinition(methodName), new TStrategy(), Array.Empty<Constraint>());
        }

        protected virtual ExplorationResult Explore<TStrategy>(string methodName, IEnumerable<Constraint> constraints)
            where TStrategy : IExplorationStrategy, new()
        {
            IExplorer explorer = CreateExplorer();
            return explorer.Run(DefinitionProvider.GetMethodDefinition(methodName), new TStrategy(), constraints);
        }

        protected virtual ExplorationResult Explore<TStrategy>(string methodName, IEnumerable<UserModel> userModels)
            where TStrategy : IExplorationStrategy, new()
        {
            IExplorer explorer = CreateExplorer();
            return explorer.Run(methodName, new TStrategy(), userModels);
        }


        protected virtual ExplorationResult Explore(string methodName)
        {
            return Explore<SmartAllPathsCoverage>(methodName, Array.Empty<Constraint>());
        }
        protected virtual ExplorationResult Explore(string methodName, IEnumerable<UserModel> userModels)
        {
            return Explore<SmartAllPathsCoverage>(methodName, userModels);
        }
        protected virtual ExplorationResult Explore(string methodName, IEnumerable<Constraint> constraints)
        {
            return Explore<SmartAllPathsCoverage>(methodName, constraints);
        }



        protected TestProject GenerateTests(ExplorationResult explorationResult)
        {
            return GenerateTests(explorationResult.ToExplorationData().Build());
        }

        protected virtual TestProject GenerateTests(ConcolicExploration concolicExploration)
        {
            GeneratorEnvironment env = new GeneratorEnvironment(_testTemplate!, _testSchemaProvider!);

            TestProject testProject = env.GenerateTestProject(_testFramework!, concolicExploration);
            env.GenerateTestClass(_testFramework!, testProject, concolicExploration);

            return testProject;
        }

        protected virtual IReadOnlyDictionary<string, string> WriteTests(TestProject testProject)
        {
            string dirName = Path.Combine(_outDir, testProject.Name!);

            using (ITestProjectWriter testProjectWriter = new TestProjectWriter(dirName, s => new TestClassWriter(s)))
            {
                testProjectWriter.WriteTestClasses(testProject);
            }

            Dictionary<string, string> files = new Dictionary<string, string>();

            foreach (string fullPath in Directory.EnumerateFiles(dirName, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(dirName, fullPath);
                files[relativePath] = File.ReadAllText(fullPath);
            }


            return files;
        }
    }
}
