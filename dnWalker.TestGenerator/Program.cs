using CommandLine;

using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dnWalker.TestGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dnWalker.TestGenerator");

            // parse args and setup configuration

            // run the test generator 

            TestData testData = new TestData();
            testData.MethodName = "Examples.Concolic.Simple.Branches.SingleBranching";
            testData.Result = new Int32Parameter("expectedResult", 0);
            testData.MethodArguments.Add(new Int32Parameter("x", 5));

            using (StreamWriter writer = new StreamWriter("test.cs"))
            {
                CodeWriter codeWriter = new CodeWriter(writer);

                ITestSuitContext testSuitContext = new xUnit.XUnitTestSuitContext();
                testSuitContext.WriteTest(codeWriter, testData);
            }
        }

    }
}
