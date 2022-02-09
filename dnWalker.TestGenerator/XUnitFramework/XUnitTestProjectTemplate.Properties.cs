using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XUnitFramework
{
    internal partial class XUnitTestProjectTemplate
    {
        private ITestProjectContext? _context;

        public ITestProjectContext Context
        {
            get
            {
                return _context ?? throw new InvalidOperationException("The context is not set.");
            }
        }

        public string FallbackTargetFramework
        {
            get;
            set;
        } = "net6.0";

        // TODO: do not hardcode, instead set using a configuration
        // TODO: somehow check for latest versions? - use wildcards? '*' ?
        public List<PackageReference> BasePackages { get; } = new List<PackageReference>() 
        {
            PackageReference.Create("Microsoft.NET.Test.Sdk", new Version("17.0.0")),
            PackageReference.Create("xunit", new Version("2.4.1")),
            PackageReference.Create("xunit.runner.visualstudio", new Version("2.4.3"))
                .Include("runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive")
                .Private("all"),
            PackageReference.Create("coverlet.collector", new Version("3.1.0"))
                .Include("runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive")
                .Private("all"),
            
            PackageReference.Create("FluentAssertions", new Version("6.3.0")),
            PackageReference.Create("Moq", new Version("4.16.1")),
            PackageReference.Create("PrivateObjectExtensions", new Version("1.4.0")),
        };

        public List<Guid> BaseServices { get; } = new List<Guid>();

        public string GenerateContent(ITestProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            return TransformText();
        }

    }
}
