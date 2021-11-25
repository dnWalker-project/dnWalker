using dnlib.DotNet;

using dnWalker.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Parameter = dnWalker.Parameters.Parameter;

namespace dnWalker.Concolic.Parameters
{
    public static class ParameterFactory
    {

        public static Parameter CreateParameter(TypeSig parameterType, string localName)
        {
            // primitive, basic values 
            if (parameterType.IsCorLibType && parameterType.IsPrimitive)
            {
                return CreatePrimitiveValueParameter(parameterType, localName);
            }
            // TODO we are working with value types - TODO
            else if (parameterType.IsValueType)
            {
                var td = parameterType.TryGetTypeDefOrRef();// .ResolveTypeDefThrow();

                throw new NotSupportedException("Not yet supported custom value types...");
            }
            // Array of reference types
            else if (parameterType.IsArray)
            {
                var arraySig = parameterType.ToArraySig();
                return CreateArrayParamter(arraySig, localName);
            }
            // SZArray of reference types
            else if (parameterType.IsSZArray)
            {
                var arraySig = parameterType.ToSZArraySig();
                return CreateArrayParamter(arraySig, localName);
            }
            // Array of value types
            else if (parameterType.IsValueArray)
            {
                // https://github.com/0xd4d/dnlib/blob/8b143447a4dac36dfa02249f8a32136d19d049a4/src/DotNet/TypeSig.cs#L33  undocumented and should not be used...
                throw new NotSupportedException("ValueArrays are not supported.");
            }
            // Class or Interface
            else if (parameterType.IsClassSig)
            {
                var classSig = parameterType.ToClassSig();
                var typeDef = classSig.TryGetTypeDefOrRef().ResolveTypeDefThrow();

                if (typeDef.IsAbstract && typeDef.IsClass)
                {
                    // TODO: return CreateAbstractObjectParamter(classSig);

                    throw new NotSupportedException("Not yet supported abstract classes...");
                }
                else if (typeDef.IsClass)
                {
                    return CreateObjectParameter(classSig, localName);
                }
                else if (typeDef.IsInterface)
                {
                    return CreateInterfaceParameter(classSig, localName);
                }
                else
                {
                    throw new Exception("Unexpected type");
                }
            }


            throw new Exception("Could not resolve provided parameter type: " + parameterType.FullName);
        }

        private static InterfaceParameter CreateInterfaceParameter(ClassSig type, string localName)
        {
            return new InterfaceParameter(type.FullName, localName);
        }

        private static ObjectParameter CreateObjectParameter(ClassSig type, string localName)
        {
            return new ObjectParameter(type.FullName, localName);
        }

        private static ArrayParameter CreateArrayParamter(ArraySigBase arrayType, string localName)
        {
            var elementType = arrayType.Next;

            return new ArrayParameter(elementType.FullName, localName);
        }

        private static PrimitiveValueParameter CreatePrimitiveValueParameter(TypeSig type, string localName)
        {
            if (type.IsPrimitive)
            {
                switch (type.FullName)
                {
                    //case TypeNames.StringTypeName: return new Int32Parameter();
                    //case TypeNames.ObjectTypeName: return new Int32Parameter();

                    case TypeNames.BooleanTypeName: return new BooleanParameter(localName);
                    case TypeNames.CharTypeName: return new CharParameter(localName);
                    case TypeNames.ByteTypeName: return new ByteParameter(localName);
                    case TypeNames.SByteTypeName: return new SByteParameter(localName);
                    case TypeNames.Int16TypeName: return new Int16Parameter(localName);
                    case TypeNames.Int32TypeName: return new Int32Parameter(localName);
                    case TypeNames.Int64TypeName: return new Int64Parameter(localName);
                    case TypeNames.UInt16TypeName: return new UInt16Parameter(localName);
                    case TypeNames.UInt32TypeName: return new UInt32Parameter(localName);
                    case TypeNames.UInt64TypeName: return new UInt64Parameter(localName);
                    case TypeNames.SingleTypeName: return new SingleParameter(localName);
                    case TypeNames.DoubleTypeName: return new DoubleParameter(localName);

                    default: throw new Exception("Unexpected primitive value parameter.");
                }
            }
            else
            {
                throw new Exception("Provided type is NOT primitive!");
            }
        }
    }
}
