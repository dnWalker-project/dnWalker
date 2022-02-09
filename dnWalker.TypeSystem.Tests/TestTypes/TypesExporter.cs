using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem.Tests.TestTypes
{
    /// <summary>
    /// Provides method which instantiate interesting generic types
    /// </summary>
    public class TypesExporter
    {
        public GlobalClass GlobalClassField;
        public GlobalClass1<int> GlobalClassGen1_SimpleField;
        public GlobalClass1<List<int>> GlobalClassGen1_ComplexField;
        public GlobalClass1<Dictionary<int, List<String>>> GlobalClassGen1_Complex2Field;
        public GlobalClass[] GlobalClassArrayField;

        public NamespaceClass NamespaceClassField;
        public NamespaceClass1<int> NamespaceClassGen1_SimpleField;
        public NamespaceClass1<List<int>> NamespaceClassGen1_ComplexField;
        public NamespaceClass1<Dictionary<int, List<String>>> NamespaceClassGen1_Complex2Field;
        public NamespaceClass[] NamespaceClassArrayField;

        public GlobalClass.NestedClass GlobalNestedField;
        public GlobalClass1<int>.NestedClass GlobalGenericNestedField;
        public GlobalClass1<int>.NestedClass1<string> GlobalGenericNestedGenericField;

        public NamespaceClass.NestedClass NamespaceNestedField;
        public NamespaceClass1<int>.NestedClass NamespaceGenericNestedField;
        public NamespaceClass1<int>.NestedClass1<string> NamespaceGenericNestedGenericField;
    }
}
