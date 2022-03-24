using dnlib.DotNet;

using dnWalker.TypeSystem;

using MMC.State;

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

        public static MethodDef FindVirtualMethod(this IDataElement dataElement, MethodDef method, ExplicitActiveState cur)
        {
            const string VirtualMethodLookupKey = "virtual-methods";

            dataElement = dataElement is IManagedPointer ptr ? ptr.Value : dataElement;

            if (!(dataElement is ObjectReference reference))
            {
                throw new NotSupportedException($"ObjectReference expected, '{dataElement?.GetType().FullName}' found.");
            }

            (MethodSig sig, string name) key = (method.MethodSig, method.Name);

            if (!cur.PathStore.CurrentPath.TryGetObjectAttribute(dataElement, VirtualMethodLookupKey, out Dictionary<(MethodSig, string), MethodDef> lookup))
            {
                lookup = new Dictionary<(MethodSig, string), MethodDef>();
                cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, VirtualMethodLookupKey, lookup);
            }

            if (!lookup.TryGetValue(key, out MethodDef result))
            {
                AllocatedObject ao = (AllocatedObject)cur.DynamicArea.Allocations[reference];
                ITypeDefOrRef type = ao.Type;

                foreach (ITypeDefOrRef superType in type.InheritanceEnumerator())
                {
                    TypeDef superTypeDef = superType.ResolveTypeDefThrow();
                    MethodDef candidate = superTypeDef.FindMethod(method.Name, key.sig);

                    if (candidate != null &&
                        candidate.Body != null &&
                        candidate.Body.Instructions.Count > 0)
                    {
                        result = candidate;
                        break;
                    }
                }

                if (result != null)
                {
                    lookup[key] = result;
                }
                else
                {
                    throw new MemberNotFoundException(type.FullName, key.name);
                }
            }

            return result;
        }

        public static bool TryFindVirtualMethod(this IDataElement dataElement, MethodDef method, ExplicitActiveState cur, out MethodDef overload)
        {
            // TODO: change the FindVirtualMethod & TryFindVirtualMethod implementations,
            // so that no try/catch block is necessary
            try
            {
                overload = FindVirtualMethod(dataElement, method, cur);
                return true;
            }
            catch (MemberNotFoundException)
            {
                overload = null;
                return false;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
