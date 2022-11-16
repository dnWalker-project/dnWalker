using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestModels
{
    public class TestGroup
    {
        public string? Name { get; set; }

        public IList<TestClass> TestClasses { get; } = new List<TestClass>();
    }
}
