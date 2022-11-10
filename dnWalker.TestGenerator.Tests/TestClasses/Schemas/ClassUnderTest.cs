using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Tests.TestClasses.Schemas
{

    public class ClassUnderTest
    {
        public static void StaticMethod(int i, string s) { }
        public void InstanceMethod(int i, string s) { }

        public static int IntStaticMethod() { return 0; }
        public static int IntStaticMethodPrimitiveArgs(int i, double dbl) { return 0; }
        public static int IntStaticMethodStringArgs(string str) { return 0; }

        public int IntInstanceMethod() { return 0; }
        public int IntInstanceMethodPrimitiveArgs(int i, double dbl) { return 0; }
        public int IntInstanceMethodStringArgs(string str) { return 0; }

        public static string StringStaticMethod() { return "hello world!"; }
        public string StringInstanceMethod() { return "hello world!"; }

        public static DataClass ObjectReferenceStaticMethod(int i, DataClass refArg) { return new DataClass(); }
        public DataClass ObjectReferenceInstanceMethod(int i, DataClass refArg) { return new DataClass(); }

        public static DataClass[] ArrayReferenceStaticMethod(int i, DataClass refArg, DataClass[] arrArg) { return new DataClass[] { }; }
        public DataClass[] ArrayReferenceInstanceMethod(int i, DataClass refArg, DataClass[] arrArg) { return new DataClass[] { }; }
    }

    public class DataClass
    {
        public string StringField;
        private string HiddenStringField;

        public object RefField;
        private object HiddenRefField;

        public int PrimitiveField;
        private int HiddenPrimitiveField;
    }
}
