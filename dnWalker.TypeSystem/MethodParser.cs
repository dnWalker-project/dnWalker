using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class MethodParser : IMethodParser
    {
        private static readonly TypeEqualityComparer _typeCmp = TypeEqualityComparer.Instance;


        // [Return Type] [DeclaringType]::[Method Name][<[Generic Parameters]>]([Parameters])

        private readonly IDefinitionProvider _definitionProvider;
        private readonly TypeParser _typeParser;

        public MethodParser(IDefinitionProvider definitionProvider, TypeParser typeParser)
        {
            _definitionProvider = definitionProvider;
            _typeParser = typeParser;
        }

        public MethodParser(IDefinitionProvider definitionProvider)
        {
            _definitionProvider = definitionProvider;
            _typeParser = new TypeParser(definitionProvider);
        }

        public IMethod Parse(ReadOnlySpan<char> span)
        {

            // first - get the method name and declaring type
            GetSpans(span, out var retTypeSpan, out var declTypeSpan, out var methodNameSpan, out var genParamSpan, out var paramSpan);

            string methodName = new string(methodNameSpan);

            // resolve the declaring type
            TypeSig declTypeSig = _typeParser.Parse(declTypeSpan);
            ITypeDefOrRef declType = declTypeSig.ToTypeDefOrRef();
            TypeDef declTypeDef = declType.ResolveTypeDef();

            // get the method generic parameters
            TypeSig[] methodGenParams = genParamSpan.Length > 0 ? _typeParser.ParseTypeArray(genParamSpan) : Array.Empty<TypeSig>();

            // get the method parameters
            TypeSig[] methodParams = paramSpan.Length > 0 ? _typeParser.ParseTypeArray(paramSpan) : Array.Empty<TypeSig>();

            TypeSig retTypeSig = _typeParser.Parse(retTypeSpan);

            // get all of the method which may match
            MethodDef[] methods = declTypeDef.FindMethods(methodName).ToArray();
            if (methods.Length == 0)
            {
                throw new Exception($"Could not find the method with specified name: {methodName}");
            }

            IList<TypeSig> declTypeGenParams = declTypeSig.IsGenericInstanceType ? declTypeSig.ToGenericInstSig().GenericArguments : Array.Empty<TypeSig>();
            
            // check whether we have to handle generics
            if (methodGenParams.Length == 0 && declTypeGenParams.Count() == 0)
            {
                // no need to create generic method instance
                foreach (MethodDef method in methods)
                {
                    if (VerifySignature(method.MethodSig, retTypeSig, methodParams))
                    {
                        // TODO: create the reference iff the type is non generic
                        return new MemberRefUser(_definitionProvider.Context.MainModule, methodName, method.MethodSig, declType);
                    }
                }

                throw new Exception("Could not find a method which matches the signature.");
            }
            else
            {
                foreach (MethodDef method in methods)
                {
                    if (VerifyGenericSignature(method.MethodSig, methodParams, declTypeGenParams, methodGenParams))
                    {
                        // get method reference of the declaring type generic instance
                        IMethodDefOrRef methodRef = new MemberRefUser(_definitionProvider.Context.MainModule, methodName, method.MethodSig, declType);

                        if (methodGenParams.Length > 0)
                        {
                            // create generic instantiation of the method reference
                            GenericInstMethodSig genericMethodSig = new GenericInstMethodSig(methodGenParams);
                            return new MethodSpecUser(methodRef, genericMethodSig);
                        }
                        return methodRef;
                    }
                }

                throw new Exception("Could not find a method which matches the signature.");
            }
        }

        private bool VerifySignature(MethodSig methodSig, TypeSig parsedReturnParam, IList<TypeSig> parsedParamSigs)
        {
            IList<TypeSig> paramSigs = methodSig.GetParams();
            if (paramSigs.Count != parsedParamSigs.Count)
            {
                return false;
            }

            if (!_typeCmp.Equals(methodSig.RetType, parsedReturnParam))
            {
                return false;
            }

            for (int i = 0; i < parsedParamSigs.Count; ++i)
            {
                if (!_typeCmp.Equals(paramSigs[i], parsedParamSigs[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool VerifyGenericSignature(MethodSig methodSig, IList<TypeSig> parsedParamSig, IList<TypeSig> declTypeGenParams, IList<TypeSig> methodGenParams)
        {
            if (methodSig.GetParamCount() != parsedParamSig.Count)
            {
                return false;
            }
            else if (methodSig.GetGenParamCount() != methodGenParams.Count)
            {
                return false;
            }
            else
            {
                IList<TypeSig> methodParams = methodSig.Params;
                for (int i = 0; i < methodParams.Count; ++i)
                {
                    TypeSig pSig = methodParams[i];
                    if (pSig.IsGenericTypeParameter)
                    {
                        // a generic parameter of the declaring type
                        GenericVar genericVar = pSig.ToGenericVar();

                        // get the index of the generic variable within the declaration type generic parameters
                        int index = (int)genericVar.Number;

                        // get the instance of the generic type from the parsed declaring type
                        TypeSig genericVarInstance = declTypeGenParams[index];

                        // check that the generic var instance is same as the parsed parameter
                        if (!_typeCmp.Equals(genericVarInstance, parsedParamSig[i]))
                        {
                            return false;
                        }
                    }
                    else if (pSig.IsGenericMethodParameter)
                    {
                        // a generic parameter of the method
                        GenericMVar genericMVar = pSig.ToGenericMVar();


                        // get the index of the generic variable within the method generic parameters
                        int index = (int)genericMVar.Number;

                        // get the instance of the generic type from the parsed method generic parameters
                        TypeSig genericMVarInstance = methodGenParams[index];
                        if (!_typeCmp.Equals(genericMVarInstance, parsedParamSig[i]))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // a normal non generic parameter
                        if (!_typeCmp.Equals(pSig, parsedParamSig[i]))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void GetSpans(
            ReadOnlySpan<char> src,
            out ReadOnlySpan<char> retType,
            out ReadOnlySpan<char> declType,
            out ReadOnlySpan<char> methodName,
            out ReadOnlySpan<char> methodGenArgs,
            out ReadOnlySpan<char> rest)
        {
            rest = src;

            int idx = rest.IndexOf(' ');
            retType = rest.Slice(0, idx);
            rest = rest.Slice(idx + 1);

            idx = rest.IndexOf("::");
            declType = rest.Slice(0, idx);
            rest = rest.Slice(idx + 2); // :: - two symbols


            idx = rest.IndexOf('(');
            methodName = rest.Slice(0, idx);
            rest = rest.Slice(idx + 1, rest.Length - idx - 2); // cut the trailing ')'

            // split the method name
            idx = methodName.IndexOf('<');
            if (idx < 0)
            {
                // the method is not generic
                methodGenArgs = methodName.Slice(0, 0);
            }
            else
            {
                methodGenArgs = methodName.Slice(idx + 1, methodName.Length - idx - 2);
                methodName = methodName.Slice(0, idx);
            }
        }
    }
}
