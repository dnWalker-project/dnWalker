using dnlib.DotNet;

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
        public static ParameterStore SetTraits(this ParameterStore store, ExplicitActiveState cur, IDictionary<String, Object> data)
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

                SetTraits(store, store.Parameters[ParameterName.GetRootName(parameterName)], parameterName, parameterValue, cur);
                //index += SetTraits(store, store.Parameters[ParameterName.GetRootName(parameterName)], parameterName, parameterValue, cur);
            }

            return store;
        }


        private static void SetTraits(ParameterStore store, Parameter parameter, String fullParameterName, Object parameterValue, ExplicitActiveState cur)
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
                    SetTraitsForObject(store, objectParameter, fullParameterName, parameterValue, cur);
                }
                else if (parameter is InterfaceParameter interfaceParameter)
                {
                    SetTraitsForInterface(store, interfaceParameter, fullParameterName, parameterValue, cur);
                }
                else if (parameter is ArrayParameter arrayParameter)
                {
                    SetTraitsForArray(store, arrayParameter, fullParameterName, parameterValue, cur);
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
            if (!arrayParameter.TryGetTrait<LengthTrait>(out LengthTrait lengthTrait))
            {
                // TODO: setup a special method??
                String lengthParamterName = ParameterName.ConstructField(arrayParameter.Name, ArrayParameter.IsNullParameterName);

                // not yet specified => the parameter was not added to the store as well
                Int32Parameter lengthParamter = new Int32Parameter();
                lengthParamter.Name = lengthParamterName;
                store.AddParameter(lengthParamter);

                lengthTrait.Value = lengthParamter;
            }

            SetValue(lengthTrait.Value, value);
        }

        private static void SetIsNull(ParameterStore store, NullableParameter nullableParameter, Object value) //, ExplicitActiveState cur)
        {
            if (!nullableParameter.TryGetTrait<IsNullTrait>(out IsNullTrait isNullTrait))
            {
                // TODO: setup a special method??
                String isNullParamterName = ParameterName.ConstructField(nullableParameter.Name, NullableParameter.IsNullParameterName);

                // not yet specified => the parameter was not added to the store as well
                BooleanParameter isNullParamter = new BooleanParameter();
                isNullParamter.Name = isNullParamterName;
                store.AddParameter(isNullParamter);

                isNullTrait.Value = isNullParamter;
            }

            SetValue(isNullTrait.Value, value);
        }


        private static void SetTraitsForObject(ParameterStore store, ObjectParameter objectParameter, String fullParameterName, Object parameterValue, ExplicitActiveState cur)
        {
            // try to find the next parameter in the store (e.g. parameter.Name:FIELD_NAME
            String accessor = ParameterName.GetAccessor(objectParameter.Name, fullParameterName);

            // IsNull
            if (accessor == NullableParameter.IsNullParameterName)
            {
                // we need to continue with the IsNull parameter
                SetIsNull(store, objectParameter, (Boolean)parameterValue);
            }
            // is a Field
            else
            {
                Parameter nextParameter = objectParameter.GetField(accessor);
                if (nextParameter == null)
                {
                    String nextParamterName = ParameterName.ConstructField(objectParameter.Name, accessor);
                    TypeDef parameterType = GetType(objectParameter, cur);

                    // next parameter is not yet initialized => create it
                    nextParameter = ParameterFactory.CreateParameter(nextParamterName, parameterType.FindField(accessor).FieldType.ToTypeDefOrRef());

                    objectParameter.SetField(accessor, nextParameter);

                    store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, cur);
            }
        }

        private static void SetTraitsForInterface(ParameterStore store, InterfaceParameter interfaceParameter, String fullParameterName, Object parameterValue, ExplicitActiveState cur)
        {
            String accessor = ParameterName.GetAccessor(interfaceParameter.Name, fullParameterName);

            // IsNull
            if (accessor == NullableParameter.IsNullParameterName)
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


                Parameter nextParameter = interfaceParameter.GetMethod(methodName, callIndex);
                if (nextParameter == null)
                {
                    TypeDef parameterType = GetType(interfaceParameter, cur);

                    String nextParameterName = ParameterName.ConstructMethod(interfaceParameter.Name, methodName, callIndex);
                    nextParameter = ParameterFactory.CreateParameter(nextParameterName, parameterType.FindMethod(methodName).ReturnType.ToTypeDefOrRef());

                    interfaceParameter.SetMethod(methodName, callIndex, nextParameter);

                    store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, cur);
            }
        }

        private static void SetTraitsForArray(ParameterStore store, ArrayParameter arrayParameter, String fullParameterName, Object parameterValue, ExplicitActiveState cur)
        {
            String accessor = ParameterName.GetAccessor(arrayParameter.Name, fullParameterName);

            // IsNull
            if (accessor == NullableParameter.IsNullParameterName)
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

                Parameter nextParameter = arrayParameter.GetItemAt(index);
                if (nextParameter == null)
                {
                    String nextParameterName = ParameterName.ConstructIndex(arrayParameter.Name, index);
                    nextParameter = ParameterFactory.CreateParameter(nextParameterName, arrayParameter.ElementType);

                    arrayParameter.SetItemAt(index, nextParameter);

                    store.AddParameter(nextParameter);
                }

                SetTraits(store, nextParameter, fullParameterName, parameterValue, cur);
            }
        }
    }
}
