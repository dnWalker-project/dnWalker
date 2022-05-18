//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Tests.Parameters
//{
//    public struct MyStruct
//    {
//        private int _value;
//        private MyItem _item;

//        public int Value
//        {
//            get
//            {
//                return _value;
//            }

//            set
//            {
//                _value = value;
//            }
//        }

//        public MyItem Item
//        {
//            get
//            {
//                return _item;
//            }

//            set
//            {
//                _item = value;
//            }
//        }
//    }


//    public class MyItem
//    {
//        private int _id;

//        public int Id
//        {
//            get
//            {
//                return _id;
//            }

//            set
//            {
//                _id = value;
//            }
//        }
//    }

//    public class MyClass
//    {
//        private double _value;
//        private MyItem _item;

//        public double Value
//        {
//            get
//            {
//                return _value;
//            }

//            set
//            {
//                _value = value;
//            }
//        }

//        public MyItem Item
//        {
//            get
//            {
//                return _item;
//            }

//            set
//            {
//                _item = value;
//            }
//        }
//    }

//    public interface IMyInterface
//    {
//        int Count { get; }
//        double SetValue(int index);

//        MyClass GetMyClass();
//    }
//}
