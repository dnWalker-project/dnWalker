using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static class ParameterContextExtensions
    {
        public static IParameter CreateParameter(this IParameterSet context, TypeSig type)
        {
            static IObjectParameter CreateObjectParameter(ClassSig type, IParameterSet context)
            {
                return context.CreateObjectParameter(type.FullName);
            }

            static IArrayParameter CreateArrayParamter(ArraySigBase arrayType, IParameterSet context)
            {
                var elementType = arrayType.Next;

                return context.CreateArrayParameter(elementType.FullName);
            }

            static IPrimitiveValueParameter CreatePrimitiveValueParameter(TypeSig type, IParameterSet context)
            {
                if (type.IsPrimitive)
                {
                    switch (type.FullName)
                    {
                        case "System.Boolean": return context.CreateBooleanParameter();
                        case "System.Byte": return context.CreateByteParameter();
                        case "System.SByte": return context.CreateSByteParameter();
                        case "System.Int16": return context.CreateInt16Parameter();
                        case "System.Int32": return context.CreateInt32Parameter();
                        case "System.Int64": return context.CreateInt64Parameter();
                        case "System.UInt16": return context.CreateUInt16Parameter();
                        case "System.UInt32": return context.CreateUInt32Parameter();
                        case "System.UInt64": return context.CreateUInt64Parameter();
                        case "System.Char": return context.CreateCharParameter();
                        case "System.Single": return context.CreateSingleParameter();
                        case "System.Double": return context.CreateDoubleParameter();

                        default: throw new Exception("Unexpected primitive value parameter.");
                    }
                }
                else
                {
                    throw new Exception("Provided type is NOT primitive!");
                }
            }


            // primitive, basic values 
            if (type.IsCorLibType && type.IsPrimitive)
            {
                return CreatePrimitiveValueParameter(type, context);
            }
            // TODO we are working with value types - TODO
            else if (type.IsValueType)
            {
                ITypeDefOrRef td = type.TryGetTypeDefOrRef();// .ResolveTypeDefThrow();

                throw new NotSupportedException("Not yet supported custom value types...");
            }
            // Array of reference types
            else if (type.IsArray)
            {
                var arraySig = type.ToArraySig();
                return CreateArrayParamter(arraySig, context);
            }
            // SZArray of reference types
            else if (type.IsSZArray)
            {
                var arraySig = type.ToSZArraySig();
                return CreateArrayParamter(arraySig, context);
            }
            // Array of value types
            else if (type.IsValueArray)
            {
                // https://github.com/0xd4d/dnlib/blob/8b143447a4dac36dfa02249f8a32136d19d049a4/src/DotNet/TypeSig.cs#L33  undocumented and should not be used...
                throw new NotSupportedException("ValueArrays are not supported.");
            }
            // Class or Interface
            else if (type.IsClassSig)
            {
                var classSig = type.ToClassSig();
                var typeDef = classSig.TryGetTypeDefOrRef().ResolveTypeDefThrow();

                return CreateObjectParameter(classSig, context);
            }


            throw new Exception("Could not resolve provided parameter type: " + type.FullName);
        }
    }
}
