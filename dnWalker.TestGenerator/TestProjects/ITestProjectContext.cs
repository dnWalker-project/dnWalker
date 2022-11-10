using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestProjects
{
    /// <summary>
    /// Provides information about a test project.
    /// </summary>
    public interface ITestProjectContext
    {
        bool NullableEnable { get; }

        string? ProjectName { get; }

        string? SolutionDirectory { get; }

        IList<string> Targets { get; }
        IList<PackageReference> PackageReferencies { get; }

        IList<string> ProjectReferencies { get; }

        IList<Guid> Services { get; }
    }
}
