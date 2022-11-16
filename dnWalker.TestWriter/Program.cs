// See https://aka.ms/new-console-template for more information
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Symbolic.Xml;
using dnWalker.TestWriter;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.TestWriters;
using dnWalker.TypeSystem;

using System.Xml.Linq;

// TODO refactor and make it more readable, right now, just to verify the idea works

Console.WriteLine("dnWalker.TestWriter");
if (args.Length != 3)
{
    Console.WriteLine("Expected exactly 3 arguments, path/to/test/assembly path/to/exploration/data output/directory");
    return -1;
}

string testAssembly = args[0];
string dataFile = args[1];
string outputDir = Path.GetFullPath(args[2])!;

// initialize the domain
IDomain domain = Domain.LoadFromFile(testAssembly);
IDefinitionProvider definitionProvider = new DefinitionProvider(domain);

// load the data
TypeParser typeParser = new TypeParser(definitionProvider);
MethodParser methodParser= new MethodParser(definitionProvider, typeParser);

XmlExplorationDeserializer deserializer = new XmlExplorationDeserializer(typeParser, methodParser, new XmlModelDeserializer(typeParser, methodParser));

ConcolicExploration data = deserializer.FromXml(XElement.Load(dataFile));

// initialize environment
ExtensionsHelper extensionsHelper = new ExtensionsHelper();

ITestFramework testFramework = extensionsHelper.CreateTestFramework();
ITestTemplate testTemplate = extensionsHelper.CreateTestTemplate();
ITestSchemaProvider testSchemaProvider = extensionsHelper.CreateTestSchemaProvider();

GeneratorEnvironment environment = new GeneratorEnvironment(testTemplate, testSchemaProvider);

// generate the tests
TestProject testProject = testFramework.CreateTestProject(Path.GetDirectoryName(outputDir)!);

environment.GenerateTestClass(testFramework, testProject, data);

using (ITestProjectWriter testProjectWriter = new TestProjectWriter(outputDir, path => new TestClassWriter(path)))
{
    testProjectWriter.Write(testProject);
}

return 0;
