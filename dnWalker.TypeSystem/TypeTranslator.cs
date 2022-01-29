using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class TypeTranslator : ITypeTranslator
    {
        private readonly TypeParser _typeParser;

        public TypeTranslator(IDefinitionProvider definitionProvider)
        {
            _typeParser = new TypeParser(definitionProvider);
        }

        public string GetString(TypeSignature value)
        {
            return value.Type.FullName;
        }

        public TypeSignature FromString(string str)
        {
            TypeSig typeSig = _typeParser.Parse(str);

            return new TypeSignature(typeSig.ToTypeDefOrRef());
        }
    }
}
