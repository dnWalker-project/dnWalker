using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestProjects
{
    public class TestProjectContext : ITestProjectContext
    {
        public string? ProjectName
        {
            get; set;
        }

        public string? SolutionDirectory
        {
            get; set;
        }

        public IList<string> Targets
        {
            get;
        } = new List<string>();

        public IList<PackageReference> PackageReferencies
        {
            get;
        } = new List<PackageReference>();

        public IList<string> ProjectReferencies
        {
            get;
        } = new List<string>();

        public IList<Guid> Services
        {
            get;
        } = new List<Guid>();

        public bool NullableEnable
        {
            get; set;
        } = true;
    }
}
