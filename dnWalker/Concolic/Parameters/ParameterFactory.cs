using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static class ParameterFactory
    {
        public static Parameter CreateParameter(String name, ITypeDefOrRef parameterType)
        {
            Parameter p = CreateParameter(parameterType);
            p.Name = name;
            return p;
        }

        private static Parameter CreateParameter(ITypeDefOrRef parameterType)
        {
            if (parameterType.IsPrimitive) return CreatePrimitiveValueParameter(parameterType);


            if (parameterType.IsTypeDef)
            {
                TypeDef td = parameterType.ResolveTypeDefThrow();

                if (td.IsClass)
                {
                    return CreateObjectParameter(td);
                }
                else if (td.IsInterface)
                {
                    return CreateInterfaceParameter(td);
                }
                // TODO: how to resolve it??
                // else if (td.IsArray)
                // {
                // }
                else if (td.IsValueType)
                {
                    throw new NotSupportedException("Not yet supported custom value types...");
                }
                else
                {
                    throw new Exception("Unexpected type, supports classes and interfaces");
                }
            }

            throw new Exception("Could not resolve provided parameter type: " + parameterType.FullName);
        }

        private static InterfaceParameter CreateInterfaceParameter(ITypeDefOrRef typeDefOrRef)
        {
            return new InterfaceParameter(typeDefOrRef);
        }

        private static ObjectParameter CreateObjectParameter(ITypeDefOrRef typeDefOrRef)
        {
            return new ObjectParameter(typeDefOrRef);
        }

        private static PrimitiveValueParameter CreatePrimitiveValueParameter(ITypeDefOrRef typeDefOrRef)
        {
            if (typeDefOrRef.IsPrimitive)
            {
                switch (typeDefOrRef.FullName)
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
