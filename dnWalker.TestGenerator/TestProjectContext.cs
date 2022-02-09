using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
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

        public List<string> Targets
        {
            get;
        } = new List<string>();

        public List<PackageReference> PackageReferencies
        {
            get;
        } = new List<PackageReference>();

        public List<string> ProjectReferencies
        {
            get;
        } = new List<string>();

        public List<Guid> Services
        {
            get;
        } = new List<Guid>();

        public bool NullableEnable
        {
            get; set;
        } = true;
    }
}
