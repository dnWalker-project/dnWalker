using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TypeSystem;

using FluentAssertions;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;


namespace dnWalker.TestGenerator.Tests.Parameters
{
    public class DependencyGraphTests
    {
        private class TestClass
        {

        }

        private readonly IDefinitionProvider _definitionProvider;
        private readonly TypeSignature _testClass;

        public DependencyGraphTests()
        {
            _definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(DependencyGraphTests).Assembly));
            _testClass = new TypeSignature(_definitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Parameters.DependencyGraphTests/TestClass"));
        }

        [Fact]
        public void Test_Build_FromPrimitiveValues()
        {
            IParameterContext context = new ParameterContext(_definitionProvider);
            IParameterSet set = new ParameterSet(context);

            // five Int32 parameters, independent !!
            set.CreateInt32Parameter();
            set.CreateInt32Parameter();
            set.CreateInt32Parameter();
            set.CreateInt32Parameter();
            set.CreateInt32Parameter();

            DependencyGraph dependencyGraph = DependencyGraph.Build(set);
            dependencyGraph.GetDependencies().Should().HaveCount(5);
            dependencyGraph.GetDependencies().Should().BeEquivalentTo(dependencyGraph.GetRootDependencies());
        }

        [Fact]
        public void Test_Build_NoComplex()
        {
            IParameterContext context = new ParameterContext(_definitionProvider);
            IParameterSet set = new ParameterSet(context);

            var i1 = set.CreateInt32Parameter();
            var i2 = set.CreateInt32Parameter();
            var a = set.CreateArrayParameter(new TypeSignature(_definitionProvider.BaseTypes.Int32.TypeDefOrRef));
            a.SetItem(0, i1);
            a.SetItem(1, i2);

            var o = set.CreateObjectParameter(_testClass);
            o.SetField("myField", i2);

            // i1 -> a <- i2 -> o

            DependencyGraph dependencyGraph = DependencyGraph.Build(set);
            dependencyGraph.GetDependencies()
                .Should().HaveCount(4);
            
            dependencyGraph.GetDependencies()
                .Should().AllBeOfType<SimpleDependency>();
            
            dependencyGraph.GetRootDependencies()
                .Should().HaveCount(2);

            dependencyGraph.GetRootDependencies()
                .OfType<SimpleDependency>()
                .Select(sd => sd.Parameter)
                .Should().BeEquivalentTo(new IParameter[] { i1, i2 });
        }

        [Fact]
        public void Test_CycleWithTails()
        {
            IParameterContext context = new ParameterContext(_definitionProvider);
            IParameterSet set = new ParameterSet(context);

            var o1 = set.CreateObjectParameter(_testClass);
            var o2 = set.CreateObjectParameter(_testClass);
            var o3 = set.CreateObjectParameter(_testClass);
            var o4 = set.CreateObjectParameter(_testClass);

            o1.SetField("MyField", o2);
            o2.SetField("MyField", o3);
            o3.SetField("MyField", o1);
            o3.SetField("OtherField", o4);

            DependencyGraph dependencyGraph = DependencyGraph.Build(set);
            dependencyGraph.GetDependencies()
                .Should().HaveCount(2);

            dependencyGraph.GetRootDependencies()
                .Should().HaveCount(1);

            dependencyGraph.GetDependencies()
                .OfType<ComplexDependency>()
                .Single()
                .InnerDependencies
                .Should().HaveCount(3);
        }


        [Fact]
        public void Test_Sort_NoComplex()
        {
            IParameterContext context = new ParameterContext(_definitionProvider);
            IParameterSet set = new ParameterSet(context);

            var i1 = set.CreateInt32Parameter();
            var i2 = set.CreateInt32Parameter();
            var a = set.CreateArrayParameter(new TypeSignature(_definitionProvider.BaseTypes.Int32.TypeDefOrRef));
            a.SetItem(0, i1);
            a.SetItem(1, i2);

            var o = set.CreateObjectParameter(_testClass);
            o.SetField("myField", i2);

            // i1 -> a <- i2 -> o

            DependencyGraph dependencyGraph = DependencyGraph.Build(set);

            Dictionary<ParameterRef, SimpleDependency> depLookup = dependencyGraph
                .GetDependencies()
                .OfType<SimpleDependency>()
                .ToDictionary(sd => sd.Parameter.Reference, sd => sd);

            List<Dependency> sorted = dependencyGraph.GetSortedDependencies().ToList();

            sorted.IndexOf(depLookup[i1.Reference]).Should().BeLessThan(sorted.IndexOf(depLookup[a.Reference]));
            sorted.IndexOf(depLookup[i2.Reference]).Should().BeLessThan(sorted.IndexOf(depLookup[a.Reference]));
            sorted.IndexOf(depLookup[i2.Reference]).Should().BeLessThan(sorted.IndexOf(depLookup[o.Reference]));
        }
    }
}
