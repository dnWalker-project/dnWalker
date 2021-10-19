using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Concolic.Parameters
{
    public struct MyStruct
    { 
        public Int32 Value { get; set; }
        public MyItem Item { get; set; }
    }


    public class MyItem
    {
        public Int32 Id { get; set; }
    }

    public class MyClass
    {
        public Double Value { get; set; }
        public MyItem Item { get; set; }
    }

    public interface IMyInterface
    {
        Int32 Count { get; }
        Double SetValue(Int32 index);
    }
}
