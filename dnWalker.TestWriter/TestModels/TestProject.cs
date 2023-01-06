using dnlib.DotNet;

using dnWalker.Explorations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestModels
{
    public class TestProject
    {
        public string? Name { get; set; }
        
        public string? SoftwareUnderTest { get; set; }

        public IDictionary<string, TestGroup> TestGroups { get; } = new Dictionary<string, TestGroup>(StringComparer.OrdinalIgnoreCase);

        public ICollection<PackageReference> Packages { get; } = new PackageReferenceCollection<PackageReference>();

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    }
}
