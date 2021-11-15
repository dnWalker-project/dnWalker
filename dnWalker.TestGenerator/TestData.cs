using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public class TestData
    {
        public string? MethodName { get; set; }

        public IList<Parameter> MethodArguments { get; } = new List<Parameter>();

        public Parameter? Result { get; set; }

        public IEnumerable<string> GetNamespaces()
        {
            List<string> ns = new List<string>();


            

            return ns;
        }
    }
}
