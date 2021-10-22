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
        /// <summary>
        /// /
        /// </summary>
        /// <param name="store"></param>
        /// <param name="definitionProvider"></param>
        /// <param name="data">Output from the solver</param>
        /// <returns></returns>
        public static ParameterStore SetTraits(this ParameterStore store, IDefinitionProvider definitionProvider, IDictionary<string, object> data)
        {
            var sortedData = new SortedDictionary<string, object>(data).ToArray();

            // sortedData should be in order:
            // PARAM_NAME_1:SUB_FIELD_1:SUB_FIELD_1.2 = 5
            // PARAM_NAME_1:SUB_FIELD_2 = true
            // PARAM_NAME_3:SUB_FIELD_3:SUB_FIELD_3.1SUB_FIELD_3.1.1:SUB_FIELD_3.1.1 = false
            // PARAM_NAME_3:SUB_FIELD_3:SUB_FIELD_3.1SUB_FIELD_3.1.1:SUB_FIELD_3.1.2 = 5
            // PARAM_NAME_4
            // PARAM_NAME_5:SUB_FIELD_5
            // ...

            // go through the list of sortedData recursively - each pass will advance the index

            for (var i = 0; i < sortedData.Length; ++i)
            {
                var parameterName = sortedData[i].Key;
                var parameterValue = sortedData[i].Value;
                var rootParameterName = ParameterName.GetRootName(parameterName);

                if (parameterValue == null)
                {
                    // we can ignore null values, although we can expect, from the solver Int32, Double and Boolean, so null values will probably not come
                    continue;
                }

                if (!store.TryGetParameter(rootParameterName, out var rootParameter))
                {
                    throw new Exception("Cannot find root parameter: " + rootParameterName);
                }

                SetTraits(store, rootParameter, parameterName, parameterValue, definitionProvider);
                //index += SetTraits(store, store.Parameters[ParameterName.GetRootName(parameterName)], parameterName, parameterValue, cur);
            }

            return store;
        }


        private static void SetTraits(ParameterStore store, Parameter parameter, string fullParameterName, object parameterValue, IDefinitionProvider definitionProvider)
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


        private static void SetValue(PrimitiveValueParameter primitiveValueParameter, object value)
        {
            switch (primitiveValueParameter)
            {
                case BooleanParameter booleanParameter: booleanParameter.Value = (bool)value; break;
                case CharParameter charParameter: charParameter.Value = (char)value; break;
                case ByteParameter byteParameter: byteParameter.Value = (byte)value; break;
                case SByteParameter sbyteParameter: sbyteParameter.Value = (sbyte)value; break;
                case Int16Parameter int16Parameter: int16Parameter.Value = (short)value; break;
                case Int32Parameter int32Parameter: int32Parameter.Value = (int)value; break;
                case Int64Parameter int64Parameter: int64Parameter.Value = (long)value; break;
                case UInt16Parameter uint16Parameter: uint16Parameter.Value = (ushort)value; break;
                case UInt32Parameter uint32Parameter: uint32Parameter.Value = (uint)value; break;
                case UInt64Parameter uint64Parameter: uint64Parameter.Value = (ulong)value; break;
                case SingleParameter singleParameter: singleParameter.Value = (float)value; break;
                case DoubleParameter doubleParameter: doubleParameter.Value = (double)value; break;
                default:
                    throw new ArgumentException("Unexpected parameter type: " + primitiveValueParameter.GetType().FullName);
            }
        }

        private static void SetLength(ParameterStore store, ArrayParameter arrayParameter, object value) //, ExplicitActiveState cur)
        {
            arrayParameter.Length = (int?)value;

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

        private static void SetIsNull(ParameterStore store, ReferenceTypeParameter referenceTypeParameter, object value) //, ExplicitActiveState cur)
        {
            referenceTypeParameter.IsNull = (bool?)value;

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


        private static void SetTraitsForObject(ParameterStore store, ObjectParameter objectParameter, string fullParameterName, object parameterValue, IDefinitionProvider definitionProvider)
        {
            // try to find the next parameter in the store (e.g. parameter.Name:FIELD_NAME
            var accessor = ParameterName.GetAccessor(objectParameter.Name, fullParameterName);

            // IsNull
            if (accessor == ReferenceTypeParameter.IsNullParameterName)
            {
                // we need to continue with the IsNull parameter
                SetIsNull(store, objectParameter, (bool)parameterValue);
            }
            // is a Field
            else
            {
                //Parameter nextParameter = objectParameter.GetField(accessor);
                //if (nextParameter == null)
                if (!objectParameter.TryGetField(accessor, out var nextParameter)) // nextParameter == null)
                {
                    var nextParamterName = ParameterName.ConstructField(objectParameter.Name, accessor);
                    var parameterType = definitionProvider.GetTypeDefinition(objectParameter.TypeName);  //GetType(objectParameter, cur);

                    // next parameter is not yet initialized => create it
                    nextParameter = ParameterFactory.CreateParameter(parameterType.FindField(accessor).FieldType, nextParamterName);

                    objectParameter.SetField(accessor, nextParameter);

                    //store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }

        private static void SetTraitsForInterface(ParameterStore store, InterfaceParameter interfaceParameter, string fullParameterName, object parameterValue, IDefinitionProvider definitionProvider)
        {
            var accessor = ParameterName.GetAccessor(interfaceParameter.Name, fullParameterName);

            // IsNull
            if (accessor == ReferenceTypeParameter.IsNullParameterName)
            {
                SetIsNull(store, interfaceParameter, parameterValue);
            }
            // method result
            else
            {
                if (!ParameterName.TryParseMethodName(accessor, out var methodName, out var callIndex))
                {
                    throw new Exception("Invalid parameter name for method result.");
                }


                //Parameter nextParameter = interfaceParameter.GetMethod(methodName, callIndex);
                //if (nextParameter == null)
                if (!interfaceParameter.TryGetMethodResult(methodName, callIndex, out var nextParameter))
                {
                    var parameterType = definitionProvider.GetTypeDefinition(interfaceParameter.TypeName);

                    var nextParameterName = ParameterName.ConstructMethod(interfaceParameter.Name, methodName, callIndex);
                    nextParameter = ParameterFactory.CreateParameter(parameterType.FindMethod(methodName).ReturnType, nextParameterName);

                    interfaceParameter.SetMethodResult(methodName, callIndex, nextParameter);

                    //store.AddParameter(nextParameter);
                }

                // try to set the value for the next parameter
                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }

        private static void SetTraitsForArray(ParameterStore store, ArrayParameter arrayParameter, string fullParameterName, object parameterValue, IDefinitionProvider definitionProvider)
        {
            var accessor = ParameterName.GetAccessor(arrayParameter.Name, fullParameterName);

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
                if (!Int32.TryParse(accessor, out var index))
                {
                    throw new Exception("Unexpected accessor - only index or length expected: " + accessor);
                }

                //Parameter nextParameter = arrayParameter.GetItemAt(index);
                //if (nextParameter == null)
                if (!arrayParameter.TryGetItemAt(index, out var nextParameter))
                {
                    var elementType = definitionProvider.GetTypeDefinition(arrayParameter.ElementTypeName);

                    var nextParameterName = ParameterName.ConstructIndex(arrayParameter.Name, index);

                    nextParameter = ParameterFactory.CreateParameter(elementType.ToTypeSig(), nextParameterName);

                    arrayParameter.SetItemAt(index, nextParameter);

                    //store.AddParameter(nextParameter);
                }

                SetTraits(store, nextParameter, fullParameterName, parameterValue, definitionProvider);
            }
        }
    }
}
