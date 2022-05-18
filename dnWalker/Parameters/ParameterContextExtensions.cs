//using dnlib.DotNet;

//using dnWalker.TypeSystem;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Parameters
//{
//    public static class ParameterContextExtensions
//    {
//        public static IParameter CreateParameter(this IParameterSet set, TypeSig type)
//        {
//            static IObjectParameter CreateObjectParameter(ClassSig type, IParameterSet set)
//            {
//                return set.CreateObjectParameter(new TypeSignature(type.ToTypeDefOrRef()));
//            }

//            static IArrayParameter CreateArrayParamter(ArraySigBase arrayType, IParameterSet set)
//            {
//                var elementType = arrayType.Next;

//                return set.CreateArrayParameter(new TypeSignature(elementType.ToTypeDefOrRef()));
//            }

//            static IPrimitiveValueParameter CreatePrimitiveValueParameter(TypeSig type, IParameterSet set)
//            {
//                if (type.IsPrimitive)
//                {
//                    switch (type.FullName)
//                    {
//                        case "System.Boolean": return set.CreateBooleanParameter();
//                        case "System.Byte": return set.CreateByteParameter();
//                        case "System.SByte": return set.CreateSByteParameter();
//                        case "System.Int16": return set.CreateInt16Parameter();
//                        case "System.Int32": return set.CreateInt32Parameter();
//                        case "System.Int64": return set.CreateInt64Parameter();
//                        case "System.UInt16": return set.CreateUInt16Parameter();
//                        case "System.UInt32": return set.CreateUInt32Parameter();
//                        case "System.UInt64": return set.CreateUInt64Parameter();
//                        case "System.Char": return set.CreateCharParameter();
//                        case "System.Single": return set.CreateSingleParameter();
//                        case "System.Double": return set.CreateDoubleParameter();

//                        default: throw new Exception("Unexpected primitive value parameter.");
//                    }
//                }
//                else
//                {
//                    throw new Exception("Provided type is NOT primitive!");
//                }
//            }


//            // primitive, basic values 
//            if (type.IsCorLibType && type.IsPrimitive)
//            {
//                return CreatePrimitiveValueParameter(type, set);
//            }
//            // TODO we are working with value types - TODO
//            else if (type.IsValueType)
//            {
//                ITypeDefOrRef td = type.TryGetTypeDefOrRef();// .ResolveTypeDefThrow();

//                throw new NotSupportedException("Not yet supported custom value types...");
//            }
//            // Array of reference types
//            else if (type.IsArray)
//            {
//                var arraySig = type.ToArraySig();
//                return CreateArrayParamter(arraySig, set);
//            }
//            // SZArray of reference types
//            else if (type.IsSZArray)
//            {
//                var arraySig = type.ToSZArraySig();
//                return CreateArrayParamter(arraySig, set);
//            }
//            // Array of value types
//            else if (type.IsValueArray)
//            {
//                // https://github.com/0xd4d/dnlib/blob/8b143447a4dac36dfa02249f8a32136d19d049a4/src/DotNet/TypeSig.cs#L33  undocumented and should not be used...
//                throw new NotSupportedException("ValueArrays are not supported.");
//            }
//            // Class or Interface
//            else if (type.IsClassSig)
//            {
//                var classSig = type.ToClassSig();
//                var typeDef = classSig.TryGetTypeDefOrRef().ResolveTypeDefThrow();

//                return CreateObjectParameter(classSig, set);
//            }


//            throw new Exception("Could not resolve provided parameter type: " + type.FullName);
//        }
//    }
//}
