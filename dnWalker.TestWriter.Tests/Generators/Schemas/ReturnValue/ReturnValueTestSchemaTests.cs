using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.ReturnValue
{
    file class TestClass
    {
        public int ReturnInteger() { return 1; }
        public static int StaticReturnInteger() { return 1; }


        public string ReturnString() { return "Hello worl"; }
        public static int StaticReturnString() { return 1; }
    }

    public class ReturnValueTestSchemaTests : TestSchemaTestBase
    {
        public ReturnValueTestSchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }


    }
}
