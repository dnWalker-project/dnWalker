using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestFrameworks;
using dnWalker.TestGenerator.TestProjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XunitProvider
{
    /// <summary>
    /// Implementation of <see cref="ITestFramework"/> for xUnit.net.
    /// </summary>
    public class XunitFramework : ITestFramework
    {
        private static readonly PackageReference[] _references = new[]
        {
            PackageReference.Create("Microsoft.NET.Test.Sdk", new Version("17.0.0")),
            PackageReference.Create("xunit", new Version("2.4.1")),
            PackageReference.Create("xunit.runner.visualstudio", new Version("2.4.3"))
                .Include("runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive")
                .Private("all"),
            PackageReference.Create("coverlet.collector", new Version("3.1.0"))
                .Include("runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive")
                .Private("all"),
        };


        public ITestClassWriter CreateClassWriter(ITemplateProvider templateProvider)
        {
            return new XunitTestClassWriter(this, templateProvider);
        }

        public void InitializeClassContext(ITestClassContext classContext)
        {
            classContext.Usings.Add("Xunit");
        }

        public void InitializeProjectContext(ITestProjectContext projectContext)
        {
            foreach (var pr in _references)
            {
                projectContext.PackageReferencies.Add(pr);
            }
        }
    }
}
