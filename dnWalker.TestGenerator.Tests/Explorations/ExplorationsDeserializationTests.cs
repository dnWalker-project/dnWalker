using dnWalker.Parameters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Explorations
{
    public class ExplorationsDeserializationTests
    {
        [Theory]
        [InlineData("MyAssembly", "../my_assembly.dll", "my_super_duper_method", true)]
        public void Test_NoIterationsDesiralization(string assemblyName, string assemblyFileName, string methodName, bool isStatic)
        {
            string xml = string.Format(ExplorationXML.Exploration_NoIterations_Format, assemblyName, assemblyFileName, methodName, isStatic);
            ExplorationData.FromXml(XElement.Parse(xml)).Should().Match<ExplorationData>(e => 
                            e.AssemblyName == assemblyName && 
                            e.AssemblyFileName == e.AssemblyFileName && 
                            e.MethodSignature == methodName && 
                            e.IsStatic == isStatic && 
                            e.Iterations != null && 
                            e.Iterations.Length == 0);

        }

        [Theory]
        [InlineData(5)]
        public void Test_Iteration_Without_Parameters(int number)
        {
            string xml = string.Format(ExplorationXML.Iteration_NoInputArgs_Format, number);
            ExplorationIterationData.FromXml(XElement.Parse(xml)).Should().Match<ExplorationIterationData>(i => i.IterationNumber == number);

        }

        [Theory]
        [InlineData(5, "x", -3.14)]
        public void Test_Iteration_With_PrimitiveParameters(int number, string pName, double pValue)
        {
            string xml = string.Format(ExplorationXML.Iteration_PrimitiveInputArgs_Format, number, pValue - 1, pName, pValue);
            Parameter p;
            ExplorationIterationData.FromXml(XElement.Parse(xml)).Should().Match<ExplorationIterationData>(i => 
                            i.IterationNumber == number && 
                            i.PathConstraint == Expression.GreaterThan(Expression.Parameter(typeof(double), pName), Expression.Constant(pValue - 1)).ToString() && 
                            i.Parameters.TryGetParameter(pName, out p) &&
                            ((DoubleParameter)p).Value == pValue);

        }
    }
}
