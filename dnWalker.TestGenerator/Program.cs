using CommandLine;

using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TestGenerator.XUnit;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dnWalker.TestGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dnWalker.TestGenerator");

            // parse args and setup configuration
            const string sutPath = "../../../../dnWalker.TestGenerator.SUT/bin/Debug/net5.0/dnWalker.TestGenerator.SUT.dll";

            //Assembly sutAssembly = AppDomain.CurrentDomain.Load(System.IO.File.ReadAllBytes(sutPath));
            Assembly sutAssembly = Assembly.LoadFrom(sutPath);


            ExplorationIterationData iterationData = new ExplorationIterationData(new ParameterStore(), 0, null, null, string.Empty, string.Empty);

            ExplorationData explorationData = new ExplorationData(new ExplorationIterationData[] { iterationData }, "dnWalker.TestGenerator.SUT", sutPath, "NoMethodJustYet", true);

            ParameterStore ps = iterationData.InputParameters;

            ObjectParameter p = new ObjectParameter("dnWalker.TestGenerator.SUT.ClassWithManyFields", "x");

            p.SetField("I1", new Int32Parameter() { Value = 10 });
            p.SetField("I3", new Int32Parameter() { Value = 15 });
            p.SetField("D1", new DoubleParameter() { Value = 0.5 });
            p.SetField("D3", new DoubleParameter() { Value = -28.64 });

            ps.AddParameter(p);

            XUnitTestClassWriter testClassWriter = new XUnitTestClassWriter(new TestGeneratorContext(sutAssembly, explorationData, iterationData), "Tests", "NoMethodJustYetTests");
            testClassWriter.DeclareObjectInitializationMethods();

            testClassWriter.WriteTo(File.OpenWrite("NoMethodJustYetTests.cs"));

            // run the test generator 

            //TestData testData = new TestData();
            //testData.MethodName = "Examples.Concolic.Simple.Branches.SingleBranching";
            //testData.Result = new Int32Parameter("expectedResult", 0);
            //testData.MethodArguments.Add(new Int32Parameter("x", 5));

            //using (StreamWriter writer = new StreamWriter("test.cs"))
            //{
            //    CodeWriter codeWriter = new CodeWriter(writer);

            //    ITestSuitContext testSuitContext = new xUnit.XUnitTestSuitContext();
            //    testSuitContext.WriteTest(codeWriter, testData);
            //}


        }

    }
}
