using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests.TestTypes
{
    public class MethodsExporter
    {
        // no method args
        public void NonGenericClass_NonGenericMethod_NoArgs()
        {
            new GlobalClass().Method();
        }
        public void NonGenericClass_GenericMethod_NoArgs()
        {
            new GlobalClass().Method<int>();
        }

        public void GenericClass_NonGenericMethod_NoArgs()
        {
            new GlobalClass1<string>().Method();
        }

        public void GenericClass_GenericMethod_NoArgs()
        {
            new GlobalClass1<string>().Method<int>();
        }

        // single arg - non generic class
        public void NonGenericClass_NonGenericMethod_Arg()
        {
            new GlobalClass().Method(5);
        }

        public void NonGenericClass_GenericMethod_Concrete_Arg_NonAmbigous()
        {
            new GlobalClass().Method<string>(5);
        }

        public void NonGenericClass_GenericMethod_Concrete_Arg_Ambigous()
        {
            new GlobalClass().Method<int>(5);
        }

        public void NonGenericClass_GenericMethod_Generic_Arg()
        {
            new GlobalClass().Method<string>("hello world");
        }

        // single arg - generic class
        public void GenericClass_NonGenericMethod_Arg()
        {
            new GlobalClass1<char[]>().Method(5);
        }

        public void GenericClass_GenericMethod_Concrete_Arg_NonAmbigous()
        {
            new GlobalClass1<char[]>().Method<string>(5);
        }

        public void GenericClass_GenericMethod_Concrete_Arg_Ambigous()
        {
            new GlobalClass1<char[]>().Method<int>(5);
        }

        public void GenericClass_GenericMethod_Generic_Arg()
        {
            new GlobalClass1<char[]>().Method<string>("hello world");
        }

        public void GenericClass_NonGenericMethod_ClassGeneric_Arg()
        {
            new GlobalClass1<List<string>>().Method(new List<string>());
        }

        // multiple arg - generic class
        public void GenericClass_GenericMethod_MethodClassGeneric_Arg()
        {
            new GlobalClass1<List<string>>().Method<int>(0, new List<string>());
        }

        public void GenericClass_GenericMethod_MethodClassGeneric_Concrete_Arg()
        {
            new GlobalClass1<List<string>>().Method<int>(0, new List<string>(), 0);
        }
    }
}
