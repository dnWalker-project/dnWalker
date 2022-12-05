using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestModels
{
    public class TestClass
    {
        public IList<string> Usings { get; } = new List<string>();
        public string? Namespace { get; set; }
        public string? Name { get; set; }
        public IList<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();
        public IList<TestMethod> Methods { get; } = new List<TestMethod>();
    }
}
