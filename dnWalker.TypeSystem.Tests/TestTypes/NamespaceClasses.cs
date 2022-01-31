using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests.TestTypes
{
    public partial class NamespaceClass
    {
        public void Method() { }
    }

    public partial class NamespaceClass1<T>
    {
        public void Method() { }
    }

    public partial class NamespaceClass2<T1, T2>
    {
        public void Method() { }
    }
}
