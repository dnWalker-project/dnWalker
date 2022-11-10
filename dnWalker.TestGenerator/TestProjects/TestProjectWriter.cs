using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestProjects
{
    internal class TestProjectWriter : ITestProjectWriter
    {
        private readonly TestProjectTemplate _template = new TestProjectTemplate();

        public void WriteProject(TextWriter output, ITestProjectContext context)
        {
            string result = _template.GenerateContent(context);
            output.WriteLine(result);
        }
    }
}
