using dnlib.DotNet;

using dnWalker.TypeSystem;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC.Data
{
    public static class DataElement
    {
        public static IDataElement CreateDataElement<T>(T value, IDefinitionProvider definitionProvider)
        {
            return CreateDataElement((object)value, definitionProvider);
        }

        public static IDataElement CreateDataElement(object value, IDefinitionProvider definitionProvider)
        {
            if (value is null)
            {
                return ObjectReference.Null;
            }

            var type = value.GetType();
            if (type.IsArray)
            {
                var array = value as Array;
                return new ArrayOf(array, definitionProvider.GetTypeDefinition(type.GetElementType().FullName));
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return new Int4((Boolean)value ? 1 : 0);
                case TypeCode.Char: return new Int4((Char)value);
                case TypeCode.SByte: return new Int4((SByte)value);
                case TypeCode.Byte: return new Int4((Byte)value);
                case TypeCode.Int16: return new Int4((Int16)value);
                case TypeCode.UInt16: return new UnsignedInt4((UInt16)value);
                case TypeCode.Int32: return new Int4((Int32)value);
                case TypeCode.UInt32: return new UnsignedInt4((UInt32)value);
                case TypeCode.Int64: return new Int8((Int64)value);
                case TypeCode.UInt64: return new UnsignedInt8((UInt64)value);
                case TypeCode.Single: return new Float4((Single)value);
                case TypeCode.Double: return new Float8((Double)value);
                case TypeCode.String: return new ConstantString(value.ToString());
                default:
                    if (value is IntPtr ip)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(ip.ToInt32(), definitionProvider) : CreateDataElement(ip.ToInt64(), definitionProvider);
                    }
                    if (value is UIntPtr up)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(up.ToUInt32(), definitionProvider) : CreateDataElement(up.ToUInt64(), definitionProvider);
                    }

                    // TODO: handle reference & complex types...
                    var typeName = type.FullName;

                    var typeDef = definitionProvider.GetTypeDefinition(typeName);

                    //throw new NotSupportedException("CreateDataElement for " + value.GetType());
                    return ObjectReference.Null;
            }
        }

        public static IDataElement GetNullValue(TypeSig typeSig)
        {
            if (!typeSig.IsPrimitive)
            {
                return ObjectReference.Null;
            }

            if (typeSig.Module.CorLibTypes.IntPtr == typeSig
                || typeSig.Module.CorLibTypes.Boolean == typeSig
                || typeSig.Module.CorLibTypes.Char == typeSig
                || typeSig.Module.CorLibTypes.Int16 == typeSig
                || typeSig.Module.CorLibTypes.Int32 == typeSig
                || typeSig.Module.CorLibTypes.SByte == typeSig
                || typeSig.Module.CorLibTypes.Byte == typeSig)
            {
                return Int4.Zero;
            }

            if (typeSig.Module.CorLibTypes.Single == typeSig)
            {
                return Float4.Zero;
            }

            if (typeSig.Module.CorLibTypes.Double == typeSig)
            {
                return Float8.Zero;
            }

            if (typeSig.Module.CorLibTypes.UInt16 == typeSig
                || typeSig.Module.CorLibTypes.UInt32 == typeSig)
            {
                return UnsignedInt4.Zero;
            }

            if (typeSig.Module.CorLibTypes.Int64 == typeSig)
            {
                return Int8.Zero;
            }

            if (typeSig.Module.CorLibTypes.UInt64 == typeSig)
            {
                return UnsignedInt8.Zero;
            }

            if (typeSig.Module.CorLibTypes.UIntPtr == typeSig)
            {
                return UnsignedInt8.Zero;
            }

            throw new NotSupportedException("GetNullValue for " + typeSig.FullName);
        }
    }
}
