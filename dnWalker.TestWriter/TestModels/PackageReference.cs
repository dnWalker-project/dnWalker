namespace dnWalker.TestWriter.TestModels
{
    public class PackageReference
    {
        public string? Name { get; set; }
        public PackageVersion? Version { get; set; }
        public IList<string> IncludeAssets { get; } = new List<string>();
        public IList<string> PrivateAssets { get; } = new List<string>();
        public IList<string> ExcludeAssets { get; } = new List<string>();

        public static PackageReference Create(string name, string version)
        {
            return new PackageReference { Name = name, Version = PackageVersion.Parse(version) };
        }

        public PackageReference Include(params string[] assets)
        {
            foreach(string a in assets) 
            {
                IncludeAssets.Add(a); 
            }
            return this;
        }

        public PackageReference Exclude(params string[] assets)
        {
            foreach (string a in assets)
            {
                ExcludeAssets.Add(a);
            }
            return this;
        }

        public PackageReference Private(params string[] assets)
        {
            foreach (string a in assets)
            {
                PrivateAssets.Add(a);
            }
            return this;
        }
    }
}