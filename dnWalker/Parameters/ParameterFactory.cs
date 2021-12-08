using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static class ParameterFactory
    {
        public static IParameter CreateParameter(TypeSig parameterType)
        {
            // primitive, basic values 
            if (parameterType.IsCorLibType && parameterType.IsPrimitive)
            {
                return CreatePrimitiveValueParameter(parameterType);
            }
            // TODO we are working with value types - TODO
            else if (parameterType.IsValueType)
            {
                ITypeDefOrRef td = parameterType.TryGetTypeDefOrRef();// .ResolveTypeDefThrow();

                throw new NotSupportedException("Not yet supported custom value types...");
            }
            // Array of reference types
            else if (parameterType.IsArray)
            {
                var arraySig = parameterType.ToArraySig();
                return CreateArrayParamter(arraySig);
            }
            // SZArray of reference types
            else if (parameterType.IsSZArray)
            {
                var arraySig = parameterType.ToSZArraySig();
                return CreateArrayParamter(arraySig);
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
                    return CreateObjectParameter(classSig);
                }
                else if (typeDef.IsInterface)
                {
                    return CreateInterfaceParameter(classSig);
                }
                else
                {
                    throw new Exception("Unexpected type");
                }
            }


            throw new Exception("Could not resolve provided parameter type: " + parameterType.FullName);
        }

        private static IInterfaceParameter CreateInterfaceParameter(ClassSig type)
        {
            return new InterfaceParameter(type.FullName);
        }

        private static IObjectParameter CreateObjectParameter(ClassSig type)
        {
            return new ObjectParameter(type.FullName);
        }

        private static IArrayParameter CreateArrayParamter(ArraySigBase arrayType)
        {
            var elementType = arrayType.Next;

            return new ArrayParameter(elementType.FullName);
        }

        private static IPrimitiveValueParameter CreatePrimitiveValueParameter(TypeSig type)
        {
            if (type.IsPrimitive)
            {
                switch (type.FullName)
                {
                    //case TypeNames.StringTypeName: return new Int32Parameter();
                    //case TypeNames.ObjectTypeName: return new Int32Parameter();

                    case TypeNames.BooleanTypeName: return new BooleanParameter();
                    case TypeNames.CharTypeName: return new CharParameter();
                    case TypeNames.ByteTypeName: return new ByteParameter();
                    case TypeNames.SByteTypeName: return new SByteParameter();
                    case TypeNames.Int16TypeName: return new Int16Parameter();
                    case TypeNames.Int32TypeName: return new Int32Parameter();
                    case TypeNames.Int64TypeName: return new Int64Parameter();
                    case TypeNames.UInt16TypeName: return new UInt16Parameter();
                    case TypeNames.UInt32TypeName: return new UInt32Parameter();
                    case TypeNames.UInt64TypeName: return new UInt64Parameter();
                    case TypeNames.SingleTypeName: return new SingleParameter();
                    case TypeNames.DoubleTypeName: return new DoubleParameter();

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
