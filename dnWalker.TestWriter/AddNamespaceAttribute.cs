using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    public class AddNamespaceAttribute : TestWriterAttribute
    {
        public string NamespaceName { get; }

        public AddNamespaceAttribute(string namespaceName)
        {
            NamespaceName = namespaceName;
        }
    }
}
