using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class MethodTranslator : IMethodTranslator
    {
        private readonly MethodParser _parser;

        public MethodTranslator(IDefinitionProvider definitionProvider)
        {
            _parser = new MethodParser(definitionProvider);
        }

        public string GetString(MethodSignature value)
        {
            return value.Method.FullName;
        }

        public MethodSignature FromString(string str)
        {
            return new MethodSignature(_parser.Parse(str));
        }
    }
}
