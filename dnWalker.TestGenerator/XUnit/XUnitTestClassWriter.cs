using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.CodeDom;
using dnWalker.Parameters;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace dnWalker.TestGenerator.XUnit
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "The code will most certainly not run on iOS")]
    public class XUnitTestClassWriter
    {
        private readonly TestGeneratorContext _context;

        private CodeCompileUnit _unit;
        private CodeNamespace _namespace;
        private CodeTypeDeclaration _class;

        private void DeclareObjectInitializionMethod(string typeName, IEnumerable<string> fieldsToInitialize)
        {
            Type type = AppDomain.CurrentDomain.GetType(typeName) ?? throw new ArgumentException("Cannot find specified type.");

            _namespace.Imports.Add(new CodeNamespaceImport(type.Namespace));

            List<KeyValuePair<string, Type>> fieldInfos = fieldsToInitialize
                .Where(f => f != ReferenceTypeParameter.IsNullParameterName && f != ArrayParameter.LengthParameterName)
                .Select(f => KeyValuePair.Create(f, type.GetField(f)?.FieldType ?? throw new Exception("Cannot find specified field.")))
                .ToList();

            CodeTypeReference privateObjectType = new CodeTypeReference("Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject");

            CodeMemberMethod initializationMethod = new CodeMemberMethod();
            initializationMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            initializationMethod.Name = $"Initialize_{type.Name}";
            initializationMethod.ReturnType = new CodeTypeReference(type);
            initializationMethod.Parameters.AddRange(fieldInfos
                .Select(f =>
                {
                    CodeParameterDeclarationExpression p = new CodeParameterDeclarationExpression(f.Value, f.Key);
                    p.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(OptionalAttribute))));
                    return p;
                })
                .ToArray());

            initializationMethod.Statements.Add(new CodeVariableDeclarationStatement(type, "instance", new CodeObjectCreateExpression(type)));
            initializationMethod.Statements.Add(new CodeVariableDeclarationStatement(privateObjectType, "po", new CodeObjectCreateExpression(privateObjectType, new CodeVariableReferenceExpression("instance"))));

            CodeMethodReferenceExpression setFieldMethod = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("po"), "SetField");

            foreach (KeyValuePair<string, Type> fieldInfo in fieldInfos)
            {
                string fieldName = fieldInfo.Key;

                initializationMethod.Statements.Add(new CodeMethodInvokeExpression(setFieldMethod, new CodeSnippetExpression($"\"{fieldName}\""), new CodeVariableReferenceExpression(fieldName)));
            }

            _class.Members.Add(initializationMethod);
        }

        public void DeclareObjectInitializationMethods()
        {
            // pass through each iteration data input parameters & find out what fields are at most specified

            Dictionary<string, HashSet<string>> initializedFields = new Dictionary<string, HashSet<string>>();

            foreach (ObjectParameter objectParameter in _context.IterationData.InputParameters.GetAllParameters().OfType<ObjectParameter>())
            {
                foreach (string fieldName in objectParameter.GetKnownFields().Select(p => p.Key))
                {
                    if (!initializedFields.TryGetValue(objectParameter.TypeName, out HashSet<string>? fields))
                    {
                        fields = new HashSet<string>();
                        initializedFields[objectParameter.TypeName] = fields;
                    }

                    fields.Add(fieldName);
                }
            }

            foreach (KeyValuePair<string, HashSet<string>> typeFields in initializedFields)
            {
                DeclareObjectInitializionMethod(typeFields.Key, typeFields.Value);
            }
        }

        public void DeclareArrayInitializationMethods()
        {
            // pass through each iteration data input parameters & find out what arrays and items are at most specified 
        }

        public void DeclareInterfaceInitializationMethods()
        {

        }

        public XUnitTestClassWriter(TestGeneratorContext context, string namespaceName, string className)
        {
            _context = context;

            _unit = new CodeCompileUnit();
            _namespace = new CodeNamespace(namespaceName);
            _unit.Namespaces.Add(_namespace);

            _class = new CodeTypeDeclaration(className);
            _namespace.Types.Add(_class);

            _namespace.Imports.Add(new CodeNamespaceImport("System"));
            _namespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            _namespace.Imports.Add(new CodeNamespaceImport("System.Linq"));

            _namespace.Imports.Add(new CodeNamespaceImport("xUnit"));
            _namespace.Imports.Add(new CodeNamespaceImport("FluentAssertions"));

            _namespace.Imports.Add(new CodeNamespaceImport("Microsoft.VisualStudio.TestTools.UnitTesting"));
        }

        public void WriteTo(Stream output, IDictionary<string, string>? options = null)
        {

            CSharpCodeProvider codeProvider = options == null ? new CSharpCodeProvider() : new CSharpCodeProvider(options);

            using (StreamWriter sw = new StreamWriter(output))
            using (IndentedTextWriter tw = new IndentedTextWriter(sw, "    "))
            {
                codeProvider.GenerateCodeFromCompileUnit(_unit, tw, new CodeGeneratorOptions());
            }
        }
    }
}
