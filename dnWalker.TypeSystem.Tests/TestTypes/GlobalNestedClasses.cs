using dnWalker.TypeSystem.Tests.TestTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GlobalClass
{
    public class NestedClass
    { }
    public class NestedClass1<U>
    { }
    public class NestedClass2<U1,U2>
    { }
}

public partial class GlobalClass1<T>
{
    public class NestedClass
    { }
    public class NestedClass1<U>
    { }
    public class NestedClass2<U1, U2>
    { }
}

public partial class GlobalClass2<T1, T2>
{
    public class NestedClass
    { }
    public class NestedClass1<U>
    { }
    public class NestedClass2<U1, U2>
    { }
}
