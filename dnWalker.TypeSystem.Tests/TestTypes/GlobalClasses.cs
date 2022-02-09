using dnWalker.TypeSystem.Tests.TestTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GlobalClass
{
    public void Method() { }
    public void Method<V>() { }

    public void Method(int i) { }
    public void Method<V>(int i) { }
    public void Method<V>(V arg1) { }
}

public partial class GlobalClass1<T>
{
    public void Method() { }
    public void Method<V>() { }

    public void Method(int i) { }
    public void Method<V>(int i) { }
    public void Method<V>(V arg1) { }

    public void Method(T arg1) { }
    public void Method<V>(V arg1, T arg2) { }
    public void Method<V>(V arg1, T arg2, int i) { }
}

public partial class GlobalClass2<T1,T2>
{
    public void Method() { }
    public void Method<V>() { }
}
