using System;
using System.Collections.Generic;

namespace dnWalker.TestGenerator.TestProjects
{
    public class PackageReference
    {
        private readonly List<string> _includeAssets;
        private readonly List<string> _privateAssets;
        private readonly List<string> _excludeAssets;

        public static PackageReference Create(string name, Version version)
        {
            return new PackageReference(name, version);
        }

        public PackageReference(string name, Version version)
        {
            _includeAssets = new List<string>();
            _privateAssets = new List<string>();
            _excludeAssets = new List<string>();

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public PackageReference Include(params string[] assets)
        {
            _includeAssets.AddRange(assets);

            return this;
        }

        public PackageReference Exclude(params string[] assets)
        {
            _excludeAssets.AddRange(assets);

            return this;
        }

        public PackageReference Private(params string[] assets)
        {
            _privateAssets.AddRange(assets);

            return this;
        }

        public string Name { get; }
        public Version Version { get; }

        public IList<string> IncludeAssets => _includeAssets;
        public IList<string> PrivateAssets => _privateAssets;
        public IList<string> ExcludeAssets => _excludeAssets;


    }
}
