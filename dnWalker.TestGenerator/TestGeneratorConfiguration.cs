using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public class TestGeneratorConfiguration : ITestGeneratorConfiguration
    {
        public bool PreferLiteralsOverVariables
        {
            get; set;
        }
    }
}
