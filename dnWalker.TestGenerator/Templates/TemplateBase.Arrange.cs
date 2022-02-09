using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteArrange(IReadOnlyParameterSet set)
        {
            DependencyGraph dependencyGraph = DependencyGraph.Build(set);

            Dependency[] dependencies = dependencyGraph.GetSortedDependencies().ToArray();

            foreach (Dependency dependency in dependencies)
            {
                switch (dependency)
                {
                    case SimpleDependency simple: WriteArrangeSimpleDepencency(simple); break;
                    case ComplexDependency complex: WriteArrangeComplesDepencency(complex); break;
                    
                    // in future - more dependency types, for example some kind of file system or interprocess/network IO
                    default: throw new NotSupportedException("Unexpected dependency.");
                }

                WriteLine(String.Empty);
            }
        }
    }
}
