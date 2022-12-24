﻿using System;
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
    }
}
