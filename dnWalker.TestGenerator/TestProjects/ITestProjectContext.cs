using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestProjects
{
    public static class TargetFrameworks
    {
        public const string NET5 = "net5.0";
        public const string NET6 = "net6.0";
        public const string NETStandard20 = "netstandard2.0";
        public const string NETStandard21 = "netstandard2.1";
        public const string NETCore31 = "netcoreapp3.1";
        public const string NETFramework = "net48";

        public static string Framework(string subversion)
        {
            return "net" + subversion;
        }

        public static string Standard(string subversion)
        {
            return "netstandard" + subversion;
        }

        public static string CoreApp(string subversion)
        {
            return "netcoreapp" + subversion;
        }
    }


    public class PackageReference
    {
        public static PackageReference Create(string name, Version version)
        {
            return new PackageReference(name, version);
        }

        public PackageReference(string name, Version version)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public PackageReference Include(params string[] assets)
        {
            IncludeAssets.AddRange(assets);

            return this;
        }

        public PackageReference Exclude(params string[] assets)
        {
            ExcludeAssets.AddRange(assets);

            return this;
        }

        public PackageReference Private(params string[] assets)
        {
            PrivateAssets.AddRange(assets);

            return this;
        }

        public string Name { get; }
        public Version Version { get; }

        public List<string> IncludeAssets { get; } = new List<string>();
        public List<string> PrivateAssets { get; } = new List<string>();
        public List<string> ExcludeAssets { get; } = new List<string>();


    }

    /// <summary>
    /// Provides information about a test project.
    /// </summary>
    public interface ITestProjectContext
    {
        bool NullableEnable { get; }

        string? ProjectName { get; }

        string? SolutionDirectory { get; }

        List<string> Targets { get; }
        List<PackageReference> PackageReferencies { get; }

        List<string> ProjectReferencies { get; }

        List<Guid> Services { get; }
    }
}
