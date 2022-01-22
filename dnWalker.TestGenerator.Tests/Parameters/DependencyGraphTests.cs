using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;

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
        [Fact]
        public void Test_Build_FromPrimitiveValues()
        {
            IParameterContext context = new ParameterContext();
            // five Int32 parameters, independent !!
            context.CreateInt32Parameter();
            context.CreateInt32Parameter();
            context.CreateInt32Parameter();
            context.CreateInt32Parameter();
            context.CreateInt32Parameter();

            DependencyGraph dependencyGraph = DependencyGraph.Build(context);
            dependencyGraph.GetDependencies().Should().HaveCount(5);
            dependencyGraph.GetDependencies().Should().BeEquivalentTo(dependencyGraph.GetRootDependencies());
        }

        [Fact]
        public void Test_Build_NoComplex()
        {
            IParameterContext context = new ParameterContext();

            var i1 = context.CreateInt32Parameter();
            var i2 = context.CreateInt32Parameter();
            var a = context.CreateArrayParameter(nameof(Int32));
            a.SetItem(0, i1);
            a.SetItem(1, i2);

            var o = context.CreateObjectParameter("MyClass");
            o.SetField("myField", i2);

            // i1 -> a <- i2 -> o

            DependencyGraph dependencyGraph = DependencyGraph.Build(context);
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
            IParameterContext context = new ParameterContext();
            var o1 = context.CreateObjectParameter("MyClass");
            var o2 = context.CreateObjectParameter("MyClass");
            var o3 = context.CreateObjectParameter("MyClass");
            var o4 = context.CreateObjectParameter("MyClass");

            o1.SetField("MyField", o2);
            o2.SetField("MyField", o3);
            o3.SetField("MyField", o1);
            o3.SetField("OtherField", o4);

            DependencyGraph dependencyGraph = DependencyGraph.Build(context);
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
            IParameterContext context = new ParameterContext();

            var i1 = context.CreateInt32Parameter();
            var i2 = context.CreateInt32Parameter();
            var a = context.CreateArrayParameter(nameof(Int32));
            a.SetItem(0, i1);
            a.SetItem(1, i2);

            var o = context.CreateObjectParameter("MyClass");
            o.SetField("myField", i2);

            // i1 -> a <- i2 -> o

            DependencyGraph dependencyGraph = DependencyGraph.Build(context);

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
