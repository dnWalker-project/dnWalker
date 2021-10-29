using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public class ObjectParameter : ReferenceTypeParameter
    {
        public ObjectParameter(string fullTypeName, string name) : base(fullTypeName, name)
        {
        }

        private readonly Dictionary<string, Parameter> _fields = new Dictionary<string, Parameter>();

        public override void Initialize(CodeWriter codeWriter)
        {
            // declare the variable & create the object using its constructor
            // TODO: find the best constructor based on the fields in use...
            codeWriter.WriteVariableDeclaration(FullTypeName, Name);

            using (IDisposable privateObjectScope = codeWriter.BeginCodeBlock())
            {
                // create the PrivateObject instance
                codeWriter.WriteVariableDeclaration("var", "privateObject", $"new PrivateObject({Name})");

                foreach (KeyValuePair<string, Parameter> field in _fields)
                {
                    //codeWriter.WriteVariableDeclaration(field.Value.FullTypeName, field.Key, $"default({field.Value.FullTypeName})");

                    using (IDisposable initializeBlockField = codeWriter.BeginCodeBlock())
                    {
                        field.Value.Initialize(codeWriter);

                    }
                }

            }
        }
    }
}
