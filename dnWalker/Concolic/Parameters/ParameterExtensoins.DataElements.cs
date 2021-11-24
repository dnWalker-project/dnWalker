using dnlib.DotNet;

using dnWalker.Parameters;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Parameter = dnWalker.Parameters.Parameter;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {
        private static readonly ConditionalWeakTable<Parameter, IDataElement> _dataElements = new ConditionalWeakTable<Parameter, IDataElement>();

        private static IDataElement AsDataElement<TValue>(PrimitiveValueParameter<TValue> parameter) where TValue : struct
        {
            IDataElement dataElement;

            switch (Type.GetTypeCode(typeof(TValue)))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                    dataElement = new Int4(Convert.ToInt32(parameter.GetValue()));
                    break;

                case TypeCode.UInt32:
                    dataElement = new UnsignedInt4(Convert.ToUInt32(parameter.GetValue()));
                    break;

                case TypeCode.Int64:
                    dataElement = new Int8(Convert.ToInt64(parameter.GetValue()));
                    break;

                case TypeCode.UInt64:
                    dataElement = new UnsignedInt8(Convert.ToUInt64(parameter.GetValue()));
                    break;

                case TypeCode.Single:
                    dataElement = new Float4(Convert.ToSingle(parameter.GetValue()));
                    break;

                case TypeCode.Double:
                    dataElement = new Float8(Convert.ToDouble(parameter.GetValue()));
                    break;

                default:
                    throw new NotSupportedException();
            }

            return dataElement;
        }

        private static IDataElement AsDataElement(ObjectParameter objectParameter, ExplicitActiveState cur, bool byCil = false)
        {
            if (objectParameter.IsNull)
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
                ObjectReference objectReference = cur.DynamicArea.AllocateObject(cur.DynamicArea.DeterminePlacement(byCil), type);
                return objectReference;
            }
            
        }

        private static IDataElement AsDataElement(InterfaceParameter interfaceParameter, ExplicitActiveState cur, bool byCil = false)
        {
            if (interfaceParameter.IsNull)
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
                ObjectReference interfaceReference = cur.DynamicArea.AllocateObject(cur.DynamicArea.DeterminePlacement(byCil), type);
                return interfaceReference;
            }
        }

        private static IDataElement AsDataElement(ArrayParameter arrayParameter, ExplicitActiveState cur, bool byCil = false)
        {
            if (arrayParameter.IsNull)
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
                ObjectReference arrayReference = cur.DynamicArea.AllocateArray(cur.DynamicArea.DeterminePlacement(byCil), elementType, arrayParameter.Length);
                return arrayReference;
            }
        }

        public static IDataElement AsDataElement(this Parameter parameter, ExplicitActiveState cur, bool byCil = false)
        {
            if (_dataElements.TryGetValue(parameter, out IDataElement dataElement))
            {
                return dataElement;
            }

            dataElement = parameter switch
            {
                BooleanParameter p => AsDataElement(p),
                CharParameter p => AsDataElement(p),
                ByteParameter p => AsDataElement(p),
                SByteParameter p => AsDataElement(p),
                Int16Parameter p => AsDataElement(p),
                Int32Parameter p => AsDataElement(p),
                Int64Parameter p => AsDataElement(p),
                UInt16Parameter p => AsDataElement(p),
                UInt32Parameter p => AsDataElement(p),
                UInt64Parameter p => AsDataElement(p),
                SingleParameter p => AsDataElement(p),
                DoubleParameter p => AsDataElement(p),

                ObjectParameter p => AsDataElement(p, cur, byCil),
                InterfaceParameter p => AsDataElement(p, cur, byCil),
                ArrayParameter p => AsDataElement(p, cur, byCil),
                _ => throw new NotSupportedException()
            };

            dataElement.SetParameter(parameter, cur);

            _dataElements.AddOrUpdate(parameter, dataElement);
            return dataElement;
        }

        public static bool HasDataElement(this Parameter parameter)
        {
            return _dataElements.TryGetValue(parameter, out IDataElement _);
        }

        public static IEnumerable<Parameter> GetUsedDescendantParameters(this Parameter parameter)
        {
            if (HasDataElement(parameter))
            {
                return parameter.GetOwnedParameters().SelectMany(p => GetUsedDescendantParameters(p)).Append(parameter);
            }
            return Enumerable.Empty<Parameter>();
        }

        public static IEnumerable<Parameter> GetUsedParameters(this ParameterStore store)
        {
            return store.RootParameters.SelectMany(p => GetUsedDescendantParameters(p));
        }
    }
}
