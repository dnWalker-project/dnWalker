using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    internal class TypeParser
    {
        private const char BeginGenCount = '`';
        private const char BeginGenArray = '<';
        private const char EndGenArray = '>';
        private const char SeparateGenParams = ',';
        private const char BeginNestedTypeName = '/';
        private const string ArrayIdentifier = "[]";

        private readonly IDefinitionProvider _definitionProvider;


        public TypeParser(IDefinitionProvider definitionProvider)
        {
            _definitionProvider = definitionProvider;
        }

        public TypeSig Parse(ReadOnlySpan<char> span)
        {
            if (span.EndsWith(ArrayIdentifier))
            {
                TypeSig elementType = Parse(span.Slice(0, span.Length - 2));
                SZArraySig arraySig = new SZArraySig(elementType);
                return arraySig;
            }

            // TODO: handle multi dimensional arrays

            int genParamsStart = span.IndexOf(BeginGenArray);
            if (genParamsStart == -1)
            {
                string typeName = new string(span).Trim();
                TypeSig resultSig = _definitionProvider.GetTypeDefinition(typeName).ToTypeSig();
                return resultSig;
            }

            ClassOrValueTypeSig genType = _definitionProvider.GetTypeDefinition(new string(span.Slice(0, genParamsStart)).Trim())
                .ToTypeSig()
                .ToClassOrValueTypeSig();

            ReadOnlySpan<char> genParamSpan = span.Slice(genParamsStart + 1, span.Length - genParamsStart - 2);

            TypeSig[] genParams = ParseTypeArray(genParamSpan);


            return genType.CreateGenericTypeSig(genParams);
        }

        public TypeSig[] ParseTypeArray(ReadOnlySpan<char> typeArray)
        {
            List<int> separators = GetSeparators(typeArray);
            TypeSig[] types = new TypeSig[separators.Count + 1];

            for (int i = 0; i < separators.Count; ++i)
            {
                ReadOnlySpan<char> singleType = typeArray.Slice(0, separators[i]);
                types[i] = Parse(singleType);
                typeArray = typeArray.Slice(separators[i] + 1);
            }

            types[^1] = Parse(typeArray);
            return types;
        }

        private List<int> GetSeparators(ReadOnlySpan<char> span)
        {
            List<int> separators = new List<int>();

            int offset = -1;
            int lvl = 0;
            for (int i = 0; i < span.Length; ++i)
            {
                char c = span[i];

                if (c == BeginGenArray)
                {
                    // a nested gen parameter array
                    lvl++;
                }
                else if (c == EndGenArray)
                {
                    // end of the nested gen parameter array
                    lvl--;
                }
                else if (c == SeparateGenParams && lvl == 0)
                {
                    separators.Add(i - offset - 1);
                    offset = i;
                }
            }

            return separators;
        }
    }
}
