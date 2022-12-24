using dnWalker.TestWriter.TestModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    public class AddPackageAttribute : TestWriterAttribute
    {
        public string PackageName { get; }
        public string? Version { get; set; }

        public string[]? IncludeAssets { get; set; }
        public string[]? PrivateAssets { get; set; }
        public string[]? ExcludeAssets { get; set; }

        public AddPackageAttribute(string packageName)
        {
            PackageName = packageName;
        }

        public PackageReference GetPackageReference()
        {
            PackageReference pr = new PackageReference()
            {
                Name = PackageName,
                Version = Version ?? "*"
            };

            if (IncludeAssets != null)
            {
                foreach (string a in IncludeAssets)
                {
                    pr.IncludeAssets.Add(a);
                }
            }

            if (PrivateAssets != null)
            {
                foreach (string a in PrivateAssets)
                {
                    pr.PrivateAssets.Add(a);
                }
            }

            if (ExcludeAssets != null)
            {
                foreach (string a in ExcludeAssets)
                {
                    pr.ExcludeAssets.Add(a);
                }
            }

            return pr;
        }
    }
}
