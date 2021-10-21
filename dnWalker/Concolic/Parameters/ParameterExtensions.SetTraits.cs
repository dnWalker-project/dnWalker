using dnlib.DotNet;

using MMC;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {
        public static ParameterStore SetTraits(this ParameterStore store, IDefinitionProvider definitionProvider, IDictionary<String, Object> data)
        {
            KeyValuePair<String, Object>[] sortedData = new SortedDictionary<String, Object>(data).ToArray();

            // sortedData should be in order:
            // PARAM_NAME_1:SUB_FIELD_1:SUB_FIELD_1.2
            // PARAM_NAME_1:SUB_FIELD_2
            // PARAM_NAME_3:SUB_FIELD_3:SUB_FIELD_3.1SUB_FIELD_3.1.1:SUB_FIELD_3.1.1
            // PARAM_NAME_3:SUB_FIELD_3:SUB_FIELD_3.1SUB_FIELD_3.1.1:SUB_FIELD_3.1.2
            // PARAM_NAME_4
            // PARAM_NAME_5:SUB_FIELD_5
            // ...

            // go through the list of sortedData recursively - each pass will advance the index

            for (Int32 i = 0; i < sortedData.Length; ++i)
            {
                String parameterName = sortedData[i].Key;
                Object parameterValue = sortedData[i].Value;
                String rootParameterName = ParameterName.GetRootName(parameterName);

                if (parameterValue == null)
                {
                    // we can ignore null values, although we can expect, from the solver Int32, Double and Boolean, so null values will probably not come
                    continue;
                }

                if (!store.TryGetParameter(rootParameterName, out Parameter rootParameter))
                {
                    throw new Exception("Cannot find root parameter: " + rootParameterName);
                }

                SetTraits(store, rootParameter, parameterName, parameterValue, definitionProvider);
                //index += SetTraits(store, store.Parameters[ParameterName.GetRootName(parameterName)], parameterName, parameterValue, cur);
            }

            return store;
        }


        private static void SetTraits(ParameterStore store, Parameter parameter, String fullParameterName, Object parameterValue, IDefinitionProvider definitionProvider)
        {
            if (fullParameterName == parameter.Name)
            {
                // the parameter is a leaf => we can handle its value directly
                if (parameter is PrimitiveValueParameter primitiveValueParameter)
                {
                    SetValue(primitiveValueParameter, parameterValue);
                }
                else
                {
                    throw new Exception("Unexpected leaf parameter");
                }
            }
            else
            {
                // it is not a leaf => walk to the leaf using the provided parameterName & create all parameters in-between

                if (parameter is ObjectParameter objectParameter)
                {
                    SetTraitsForObject(store, objectParameter, fullParameterName, parameterValue, definitionProvider);
                }
                else if (parameter is InterfaceParameter interfaceParameter)
                {
                    SetTraitsForInterface(store, interfaceParameter, fullParameterName, parameterValue, definitionProvider);
                }
                else if (parameter is ArrayParameter arrayParameter)
                {
                    SetTraitsForArray(store, arrayParameter, fullParameterName, parameterValue, definitionProvider);
                }
                else
                {
                    throw new Exception("Unexpected parameter type:" + parameter);
                }
            }
        }


        private static void SetValue(PrimitiveValueParameter primitiveValueParameter, Object value)
        {
            switch (primitiveValueParameter)
            {
                case BooleanParameter booleanParameter: booleanParameter.Value = (Boolean)value; break;
                case CharParameter charParameter: charParameter.Value = (Char)value; break;
                case ByteParameter byteParameter: byteParameter.Value = (Byte)value; break;
                case SByteParameter sbyteParameter: sbyteParameter.Value = (SByte)value; break;
                case Int16Parameter int16Parameter: int16Parameter.Value = (Int16)value; break;
                case Int32Parameter int32Parameter: int32Parameter.Value = (Int32)value; break;
                case Int64Parameter int64Parameter: int64Parameter.Value = (Int64)value; break;
                case UInt16Parameter uint16Parameter: uint16Parameter.Value = (UInt16)value; break;
                case UInt32Parameter uint32Parameter: uint32Parameter.Value = (UInt32)value; break;
                case UInt64Parameter uint64Parameter: uint64Parameter.Value = (UInt64)value; break;
                case SingleParameter singleParameter: singleParameter.Value = (Single)value; break;
                case DoubleParameter doubleParameter: doubleParameter.Value = (Double)value; break;
                default:
                    throw new ArgumentException("Unexpected parameter type: " + primitiveValueParameter.GetType().FullName);
            }
        }

        private static void SetLength(ParameterStore store, ArrayParameter arrayParameter, Object value) //, ExplicitActiveState cur)
        {
            arrayParameter.Length = (Int32?)value;

            //if (!arrayParameter.TryGetTrait<LengthTrait>(out LengthTrait lengthTrait))
            //{
            //    // TODO: setup a special method??
            //    String lengthParamterName = ParameterName.ConstructField(arrayParameter.Name, ArrayParameter.IsNullParameterName);

            //    // not yet specified => the parameter was not added to the store as well
            //    Int32Parameter lengthParamter = new Int32Parameter();
            //    lengthParamter.Name = lengthParamterName;
            //    store.AddParameter(lengthParamter);

            //    lengthTrait.LengthParameter = lengthParamter;
            //}

            //SetValue(lengthTrait.LengthParameter, value);
        }

        private static void SetIsNull(ParameterStore store, ReferenceTypeParameter referenceTypeParameter, Object value) //, ExplicitActiveState cur)
        {
            referenceTypeParameter.IsNull = (Boolean?)value;

            //if (!nullableParameter.TryGetTrait<IsNullTrait>(out IsNullTrait isNullTrait))
            //{
            //    // TODO: setup a special method??
            //    String isNullParamterName = ParameterName.ConstructField(nullableParameter.Name, ReferenceTypeParameter.IsNullParameterName);

            //    // not yet specified => the parameter was not added to the store as well
            //    BooleanParameter isNullParamter = new BooleanParameter();
            //    isNullParamter.Name = isNullParamterName;
            //    store.AddParameter(isNullParamter);

            //    isNullTrait.IsNullParameter = isNullParamter;
            //}

            //SetValue(isNullTrait.IsNullParameter, value);
        }


        private static void SetTraitsForObject(ParameterStore store, ObjectParameter objectParameter, String fullParameterName, Object parameterValue, IDefinitionProvider definitionProvider)
        {
            // try to find the next parameter in the store (e.g. parameter.Name:FIELD_NAME
            String accessor = ParameterName.GetAccessor(objectParameter.Name, fullParameterName);

            // IsNull
            if (accessor == ReferenceTypeParameter.IsNullParameterName)
            {
                // we need to continue with the IsNull parameter
                SetIsNull(store, objectParameter, (Boolean)parameterValue);
            }
            // is a Field
            else
            {
                //Parameter nextParameter = objectParameter.GetField(accessor);
                //if (nextParameter == null)
                if (!objectParameter.TryGetField(accessor, out Parameter nextParameter)) // nextParameter == null)
                {
                    String nextParamterName = ParameterName.ConstructField(objectParameter.Name, accessor);
                    TypeDef parameterType = definitionProvider.GetTypeDefinition(objectParameter.TypeName);  //GetType(objectParameter, cur);

                    // next parameter is not yet initialized => create it
                    nextParameter = ParameterFactory.CreateParameter(parameterType.FindField(accessor).FieldType, nextParamterName);

                    objectParameter.SetField(accessor, nextParameter);

                    store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }

        private static void SetTraitsForInterface(ParameterStore store, InterfaceParameter interfaceParameter, String fullParameterName, Object parameterValue, IDefinitionProvider definitionProvider)
        {
            String accessor = ParameterName.GetAccessor(interfaceParameter.Name, fullParameterName);

            // IsNull
            if (accessor == ReferenceTypeParameter.IsNullParameterName)
            {
                SetIsNull(store, interfaceParameter, parameterValue);
            }
            // method result
            else
            {
                if (!ParameterName.TryParseMethodName(accessor, out String methodName, out Int32 callIndex))
                {
                    throw new Exception("Invalid parameter name for method result.");
                }


                //Parameter nextParameter = interfaceParameter.GetMethod(methodName, callIndex);
                //if (nextParameter == null)
                if (!interfaceParameter.TryGetMethodResult(methodName, callIndex, out Parameter nextParameter))
                {
                    TypeDef parameterType = definitionProvider.GetTypeDefinition(interfaceParameter.TypeName);

                    String nextParameterName = ParameterName.ConstructMethod(interfaceParameter.Name, methodName, callIndex);
                    nextParameter = ParameterFactory.CreateParameter(parameterType.FindMethod(methodName).ReturnType, nextParameterName);

                    interfaceParameter.SetMethodResult(methodName, callIndex, nextParameter);

                    store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }

        private static void SetTraitsForArray(ParameterStore store, ArrayParameter arrayParameter, String fullParameterName, Object parameterValue, IDefinitionProvider definitionProvider)
        {
            String accessor = ParameterName.GetAccessor(arrayParameter.Name, fullParameterName);

            // IsNull
            if (accessor == ReferenceTypeParameter.IsNullParameterName)
            {
                SetIsNull(store, arrayParameter, parameterValue);
            }
            // Length
            else if (accessor == ArrayParameter.LengthParameterName)
            {
                SetLength(store, arrayParameter, parameterValue);
            }
            // item
            else
            {
                if (!Int32.TryParse(accessor, out Int32 index))
                {
                    throw new Exception("Unexpected accessor - only index or length expected: " + accessor);
                }

                //Parameter nextParameter = arrayParameter.GetItemAt(index);
                //if (nextParameter == null)
                if (!arrayParameter.TryGetItemAt(index, out Parameter nextParameter))
                {
                    TypeDef elementType = definitionProvider.GetTypeDefinition(arrayParameter.ElementTypeName);

                    String nextParameterName = ParameterName.ConstructIndex(arrayParameter.Name, index);

                    nextParameter = ParameterFactory.CreateParameter(elementType.ToTypeSig(), nextParameterName);

                    arrayParameter.SetItemAt(index, nextParameter);

                    store.AddParameter(nextParameter);
                }

                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }
    }
}
