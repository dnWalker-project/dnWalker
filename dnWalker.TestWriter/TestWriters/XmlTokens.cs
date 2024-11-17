using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestWriters
{
    internal class XmlTokens
    {
        internal const string Sdk = nameof(Sdk);
        internal const string Project = nameof(Project);
        internal const string SdkValue = "Microsoft.NET.Sdk";
        internal const string PropertyGroup = nameof(PropertyGroup);
        internal const string TargetFramework = nameof(TargetFramework);
        internal const string IsPackable = nameof(IsPackable);
        internal const string ItemGroup = nameof(ItemGroup);
        internal const string PackageReference = nameof(PackageReference);
        internal const string Include = nameof(Include);
        internal const string Version = nameof(Version);
        internal const string IncludeAssets = nameof(IncludeAssets);
        internal const string ExcludeAssets = nameof(ExcludeAssets);
        internal const string PrivateAssets = nameof(PrivateAssets);

        internal const string FallbackTargetFrameworkValue = "net8.0";
        internal const string FallbackIsPackableValue = "false";
    }
}
