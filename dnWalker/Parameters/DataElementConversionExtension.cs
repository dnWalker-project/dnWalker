﻿using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        private const string Parameter2DataElement = "PRM2DE";

        public static IDictionary<int, IDataElement> GetParameterToDataElementLookup(this ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(Parameter2DataElement, out IDictionary<int, IDataElement> lookup))
            {
                lookup = new Dictionary<int, IDataElement>();
                cur.PathStore.CurrentPath.SetPathAttribute(Parameter2DataElement, lookup);
            }

            return lookup;
        }

        public static IDataElement AsDataElement(this IParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<int, IDataElement> lookup = GetParameterToDataElementLookup(cur);
            int id = parameter.Id;

            if (!lookup.TryGetValue(id, out IDataElement dataElement))
            {
                dataElement = CreateDataElementAndSetParameter(parameter, cur);
                lookup[id] = dataElement;
            }

            return dataElement;
        }

        private static IDataElement CreateDataElementAndSetParameter(IParameter parameter, ExplicitActiveState cur)
        {
            IDataElement dataElement;

            switch (parameter)
            {
                case IBooleanParameter p: dataElement = CreateDataElement(p); break;
                case ICharParameter p: dataElement = CreateDataElement(p); break;
                case IByteParameter p: dataElement = CreateDataElement(p); break;
                case ISByteParameter p: dataElement = CreateDataElement(p); break;
                case IInt16Parameter p: dataElement = CreateDataElement(p); break;
                case IInt32Parameter p: dataElement = CreateDataElement(p); break;
                case IInt64Parameter p: dataElement = CreateDataElement(p); break;
                case IUInt16Parameter p: dataElement = CreateDataElement(p); break;
                case IUInt32Parameter p: dataElement = CreateDataElement(p); break;
                case IUInt64Parameter p: dataElement = CreateDataElement(p); break;
                case ISingleParameter p: dataElement = CreateDataElement(p); break;
                case IDoubleParameter p: dataElement = CreateDataElement(p); break;

                case IObjectParameter p: dataElement = CreateDataElement(p, cur); break;
                case IInterfaceParameter p: dataElement = CreateDataElement(p, cur); break;
                case IStringParameter p: dataElement = CreateDataElement(p); break;
                case IArrayParameter p: dataElement = CreateDataElement(p, cur); break;
                default: throw new NotSupportedException();
            }

            dataElement.SetParameter(parameter, cur);

            return dataElement;
        }

        private static IDataElement CreateDataElement(IArrayParameter arrayParameter, ExplicitActiveState cur)
        {
            if (!arrayParameter.GetIsNull())
            {
                // initialize fake null reference
                // cannot use ObjectReference.Null, because we need a unique data element
                // in order to use cur.PathStore.CurrentPath... object attributes access
                ObjectReference objectReference = new ObjectReference(0);
                return objectReference;
            }
            else
            {
                // get ITypeOrDef
                ITypeDefOrRef elementType = cur.DefinitionProvider.GetTypeDefinition(arrayParameter.ElementTypeName);

                // create ObjectReference
                ObjectReference arrayReference = cur.DynamicArea.AllocateArray(cur.DynamicArea.DeterminePlacement(false), elementType, arrayParameter.GetLength());
                return arrayReference;
            }
        }

        private static IDataElement CreateDataElement(IInterfaceParameter interfaceParameter, ExplicitActiveState cur)
        {
            if (!interfaceParameter.GetIsNull())
            {
                // initialize fake null reference
                // cannot use ObjectReference.Null, because we need a unique data element
                // in order to use cur.PathStore.CurrentPath... object attributes access
                ObjectReference objectReference = new ObjectReference(0);
                return objectReference;
            }
            else
            {
                // get ITypeOrDef
                ITypeDefOrRef type = cur.DefinitionProvider.GetTypeDefinition(interfaceParameter.TypeName);

                // create ObjectReference
                ObjectReference interfaceReference = cur.DynamicArea.AllocateObject(cur.DynamicArea.DeterminePlacement(false), type);
                return interfaceReference;
            }
        }

        private static IDataElement CreateDataElement(IObjectParameter objectParameter, ExplicitActiveState cur)
        {
            if (!objectParameter.GetIsNull())
            {
                // initialize fake null reference
                // cannot use ObjectReference.Null, because we need a unique data element
                // in order to use cur.PathStore.CurrentPath... object attributes access
                ObjectReference objectReference = new ObjectReference(0);
                return objectReference;
            }
            else
            {
                // get ITypeOrDef
                ITypeDefOrRef type = cur.DefinitionProvider.GetTypeDefinition(objectParameter.TypeName);

                // create ObjectReference
                ObjectReference objectReference = cur.DynamicArea.AllocateObject(cur.DynamicArea.DeterminePlacement(false), type);
                return objectReference;
            }
        }

        private static IDataElement CreateDataElement(IStringParameter stringParameter)
        {
            string value = stringParameter.CreateString();
            if (value == null)
            {
                return ObjectReference.Null;
            }
            else
            {
                return new ConstantString(value);
            }
        }

        private static IDataElement CreateDataElement<TValue>(IPrimitiveValueParameter<TValue> parameter) where TValue : struct
        {
            IDataElement dataElement;

            Type valueType = typeof(TValue);

            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                    dataElement = new Int4(Convert.ToInt32(((IPrimitiveValueParameter)parameter).Value));
                    break;

                case TypeCode.UInt32:
                    dataElement = new UnsignedInt4(Convert.ToUInt32(((IPrimitiveValueParameter)parameter).Value));
                    break;

                case TypeCode.Int64:
                    dataElement = new Int8(Convert.ToInt64(((IPrimitiveValueParameter)parameter).Value));
                    break;

                case TypeCode.UInt64:
                    dataElement = new UnsignedInt8(Convert.ToUInt64(((IPrimitiveValueParameter)parameter).Value));
                    break;

                case TypeCode.Single:
                    dataElement = new Float4(Convert.ToSingle(((IPrimitiveValueParameter)parameter).Value));
                    break;

                case TypeCode.Double:
                    dataElement = new Float8(Convert.ToDouble(((IPrimitiveValueParameter)parameter).Value));
                    break;

                default:
                    throw new NotSupportedException();
            }
            return dataElement;
        }
    }
}
