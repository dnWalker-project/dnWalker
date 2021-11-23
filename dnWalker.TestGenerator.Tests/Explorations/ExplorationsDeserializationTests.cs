using dnWalker.Parameters;
using dnWalker.TestGenerator.Explorations.Xml;

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
        private const string Exploration_NoIterations_Format = "<Exploration AssemblyName=\"{0}\" AssemblyFileName=\"{1}\" MethodSignature=\"{2}\" IsStatic=\"{3}\" />";
        private const string Iteration_NoInputArgs_Format = "<Iteration IterationNumber=\"{0}\">\r\n\t<ParameterStore />\r\n</Iteration>";
        private const string Iteration_PrimitiveInputArgs_Format = "<Iteration IterationNumber=\"{0}\" PathConstraint=\"(x > {1})\">\r\n\t<ParameterStore>\r\n\t\t<PrimitiveValue Type=\"System.Double\" Name=\"{2}\">{3}</PrimitiveValue>\r\n\t</ParameterStore>\r\n</Iteration>";


        [Theory]
        [InlineData("MyAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "../my_assembly.dll", "System.Void my_namespace.my_class::my_super_duper_method()", true)]
        public void Test_NoIterationsDesiralization(string assemblyName, string assemblyFileName, string methodName, bool isStatic)
        {
            string xml = string.Format(Exploration_NoIterations_Format, assemblyName, assemblyFileName, methodName, isStatic);
            XElement.Parse(xml).ToExplorationData().Should().Match<ExplorationData>(e => 
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
            string xml = string.Format(Iteration_NoInputArgs_Format, number);
            XElement.Parse(xml).ToExplorationIterationData().Should().Match<ExplorationIterationData>(i => i.IterationNumber == number);

        }

        [Theory]
        [InlineData(5, "x", -3.14)]
        public void Test_Iteration_With_PrimitiveParameters(int number, string pName, double pValue)
        {
            string xml = string.Format(Iteration_PrimitiveInputArgs_Format, number, pValue - 1, pName, pValue);
            Parameter p;
            XElement.Parse(xml).ToExplorationIterationData().Should().Match<ExplorationIterationData>(i => 
                            i.IterationNumber == number && 
                            i.PathConstraint == Expression.GreaterThan(Expression.Parameter(typeof(double), pName), Expression.Constant(pValue - 1)).ToString() && 
                            i.ParameterStore.TryGetParameter(pName, out p) &&
                            ((DoubleParameter)p).Value == pValue);

        }
    }
}
