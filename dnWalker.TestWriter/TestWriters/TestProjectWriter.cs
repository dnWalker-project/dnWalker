using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.TestWriter.TestWriters
{
    public class TestProjectWriter : ITestProjectWriter
    {
        private readonly string _directory;
        private readonly Func<string, ITestClassWriter> _classWriterProvider;

        public TestProjectWriter(string directory, Func<string, ITestClassWriter> classWriterProvider)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException($"'{nameof(directory)}' cannot be null or whitespace.", nameof(directory));
            }

            _directory = directory;
            _classWriterProvider = classWriterProvider ?? throw new ArgumentNullException(nameof(classWriterProvider));
        }

        public void Write(TestProject testProject)
        {

            Directory.CreateDirectory(_directory);

            // write the project file
            WriteProjectFile(testProject);

            // foreach each group create its subdirectory & write its test classes
            foreach ((string path, TestGroup group) in testProject.TestGroups)
            {
                WriteTestGroup(path, group);
            }
        }

        private void WriteTestGroup(string path, TestGroup group)
        {
            string groupDirectoryPath = Path.Combine(_directory, path);
            Directory.CreateDirectory(groupDirectoryPath);

            foreach (TestClass testClass in group.TestClasses) 
            {
                WriteTestClass(Path.Combine(groupDirectoryPath, GetTestClassFileName(testClass.Name)), testClass);
            }
        }

        private void WriteTestClass(string fileLocation, TestClass testClass)
        {
            using (ITestClassWriter classWriter = _classWriterProvider(fileLocation)) 
            {
                classWriter.Write(testClass);
            }
        }

        private string GetTestClassFileName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            return $"{name}.cs";
        }

        private void WriteProjectFile(TestProject testProject)
        {
            string projectFileLocation = Path.Combine(_directory, $"{testProject.Name}.csproj");
            using (TextWriter output = new StreamWriter(projectFileLocation))
            using (CsProjWriter writer = new CsProjWriter(output))
            {
                writer.Write(testProject);
            }
        }

        public void Dispose()
        {
        }
    }
}
