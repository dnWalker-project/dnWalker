using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests.TestTypes
{
    public partial class NamespaceClass
    {
        public class NestedClass
        { }
        public class NestedClass1<U>
        { }
        public class NestedClass2<U1, U2>
        { }
    }

    public partial class NamespaceClass1<T>
    {
        public class NestedClass
        { }
        public class NestedClass1<U>
        { }
        public class NestedClass2<U1, U2>
        { }
    }

    public partial class NamespaceClass2<T1, T2>
    {
        public class NestedClass
        { }
        public class NestedClass1<U>
        { }
        public class NestedClass2<U1, U2>
        { }
    }

}
