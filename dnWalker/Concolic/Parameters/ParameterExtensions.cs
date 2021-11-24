using dnlib.DotNet;

using dnWalker.Parameters;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Parameter = dnWalker.Parameters.Parameter;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {

        public static ParameterStore InitializeDefaultMethodParameters(this ParameterStore store, MethodDef method)
        {
            TypeSig[] parameterTypes = method.Parameters.Select(p => p.Type).ToArray();
            String[] parameterNames = method.Parameters.Select(p => p.Name).ToArray();

            return InitializeRootParameters(store, parameterTypes, parameterNames);
        }

        public static ParameterStore InitializeRootParameters(this ParameterStore store, TypeSig[] parameterTypes, String[] parameterNames)
        {
            for (Int32 i = 0; i < parameterTypes.Length; ++i)
            {
                Parameter parameter = ParameterFactory.CreateParameter(parameterTypes[i], parameterNames[i]);
                store.AddParameter(parameter);
            }

            return store;
        }

        public static DataElementList GetMethodParematers(this ParameterStore store, ExplicitActiveState cur, MethodDef method)
        {
            return GetMethodParematers(store, cur, method.Parameters.Select(p => p.Name).ToArray());
        }

        public static DataElementList GetMethodParematers(this ParameterStore store, ExplicitActiveState cur, String[] parameterNames)
        {
            DataElementList arguments = cur.StorageFactory.CreateList(parameterNames.Length);

            for (Int32 i = 0; i < parameterNames.Length; ++i)
            {
                if (store.TryGetParameter(parameterNames[i], out Parameter parameter))
                {
                    arguments[i] = parameter.CreateDataElement(cur);
                }
                else
                {
                    throw new Exception("Cannot initialize method parameters, missing parameter: " + parameterNames[i]);
                }

            }

            return arguments;
        }


        public static IDataElement CreateDataElement(this Parameter parameter, ExplicitActiveState cur)
        {
            switch (parameter)
            {
                case BooleanParameter p: return CreateDataElement(p, cur);
                case CharParameter p: return CreateDataElement(p, cur);
                case ByteParameter p: return CreateDataElement(p, cur);
                case SByteParameter p: return CreateDataElement(p, cur);
                case Int16Parameter p: return CreateDataElement(p, cur);
                case Int32Parameter p: return CreateDataElement(p, cur);
                case Int64Parameter p: return CreateDataElement(p, cur);
                case UInt16Parameter p: return CreateDataElement(p, cur);
                case UInt32Parameter p: return CreateDataElement(p, cur);
                case UInt64Parameter p: return CreateDataElement(p, cur);
                case SingleParameter p: return CreateDataElement(p, cur);
                case DoubleParameter p: return CreateDataElement(p, cur);
                case ObjectParameter p: return CreateDataElement(p, cur);
                case InterfaceParameter p: return CreateDataElement(p, cur);
                case ArrayParameter p: return CreateDataElement(p, cur);
                default:
                    throw new NotSupportedException();
            }

        }



        // from Instruction.cs
        private static Int32 GetFieldOffset(TypeDef type, String fieldName)
        {
            FieldDef fld = type.FindField(fieldName);
            if (!fld.HasLayoutInfo)
            {
                fld = DefinitionProvider.GetFieldDefinition(fld);
            }


            Int32 typeOffset = 0;
            Boolean matched = false;
            Int32 retval = 0;

            foreach (TypeDef typeDef in DefinitionProvider.InheritanceEnumerator(type))
            {
                /*
                 * We start searching for the right field from the declaringtype,
                 * it is possible that the declaring type does not define fld, therefore
                 * it might be possible that we have to search further for fld in
                 * the inheritance tree, (hence matched), and this continues until
                 * a field is found which has the same offset and the same name 
                 */
                if (typeDef.FullName.Equals(fld.DeclaringType.FullName) || matched)
                {
                    if (fld.FieldOffset < typeDef.Fields.Count
                        && typeDef.Fields[(Int32)fld.FieldOffset].Name.Equals(fld.Name))
                    {
                        retval = (Int32)fld.FieldOffset;
                        break;
                    }

                    matched = true;
                }

                if (typeDef.BaseType != null && typeDef.BaseType.FullName != TypeNames.ObjectTypeName) // if base type is System.Object, stop
                {
                    typeOffset += Math.Max(0, typeDef.Fields.Count - 1);
                }
            }

            retval = typeOffset + retval;

            Debug.Assert(retval >= 0, $"Offset for type {type.FullName} is negative.");

            return retval;
        }

        public static IDataElement CreateDataElement(this ObjectParameter p, ExplicitActiveState cur)
        {


            DynamicArea dynamicArea = cur.DynamicArea;



            if (!p.IsNull.HasValue || p.IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(p, cur);
                return nullReference;
            }

            TypeDef typeDef = cur.DefinitionProvider.GetTypeDefinition(p.TypeName);

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference objectReference = dynamicArea.AllocateObject(location, typeDef);
            AllocatedObject allocatedObject = (AllocatedObject)dynamicArea.Allocations[objectReference];
            allocatedObject.ClearFields(cur);


            foreach ((TypeSig fieldType, String fieldName) in DefinitionProvider.InheritanceEnumerator(typeDef)
                                                                                .SelectMany(td => td.ResolveTypeDef().Fields)
                                                                                .Select(f => (f.FieldType, f.Name)))
            {
                if (!p.TryGetField(fieldName, out Parameter fieldParameter))
                {
                    fieldParameter = ParameterFactory.CreateParameter(fieldType);
                    p.SetField(fieldName, fieldParameter);
                }

                Int32 fieldOffset = GetFieldOffset(typeDef, fieldName);

                IDataElement fieldDataElement = fieldParameter.CreateDataElement(cur);
                allocatedObject.Fields[fieldOffset] = fieldDataElement;
            }

            //foreach (FieldValueTrait fieldValue in Traits.OfType<FieldValueTrait>())
            //{
            //    String fieldName = fieldValue.FieldName;
            //    Parameter parameter = fieldValue.FieldValueParameter;

            //    Int32 fieldOffset = GetFieldOffset(type, fieldName);

            //    IDataElement fieldDataElement = parameter.CreateDataElement(cur);
            //    allocatedObject.Fields[fieldOffset] = fieldDataElement;
            //}

            objectReference.SetParameter(p, cur);
            return objectReference;
        }
        public static IDataElement CreateDataElement(this InterfaceParameter p, ExplicitActiveState cur)
        {
            DynamicArea dynamicArea = cur.DynamicArea;

            if (!p.IsNull.HasValue || p.IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(p, cur);
                return nullReference;
            }

            TypeDef typeDef = cur.DefinitionProvider.GetTypeDefinition(p.TypeName);

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference interfaceReference = dynamicArea.AllocateObject(location, typeDef);
            AllocatedObject allocatedInterface = (AllocatedObject)dynamicArea.Allocations[interfaceReference];
            allocatedInterface.ClearFields(cur);

            // TODO: somehow create structure for resolving the method and using the callindex

            //MethodResolver resolver = new MethodResolver();
            //foreach(MethodResultTrait methodResult in Traits.OfType<MethodResultTrait>())
            //{
            //    resolver[methodResult.MethodName] = new MethodResultProvider(s => methodResult.Value.AsDataElement(s));
            //}

            interfaceReference.SetParameter(p, cur);
            return interfaceReference;
        }
        public static IDataElement CreateDataElement(this ArrayParameter p, ExplicitActiveState cur)
        {

            DynamicArea dynamicArea = cur.DynamicArea;

            if (!p.IsNull.HasValue || p.IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(p, cur);
                return nullReference;
            }

            Int32 length = p.Length ?? 0;

            Int32 location = dynamicArea.DeterminePlacement(false);

            ITypeDefOrRef elementType = cur.DefinitionProvider.GetTypeDefinition(p.ElementTypeName);

            ObjectReference objectReference = dynamicArea.AllocateArray(location, elementType, length);

            AllocatedArray allocatedArray = (AllocatedArray)dynamicArea.Allocations[objectReference];
            allocatedArray.ClearFields(cur);

            if (length > 0)
            {
                for (Int32 i = 0; i < length; ++i)
                {
                    if (!p.TryGetItemAt(i, out Parameter itemParameter))
                    {
                        itemParameter = ParameterFactory.CreateParameter(elementType.ToTypeSig());
                        p.SetItemAt(i, itemParameter);
                    }
                    IDataElement itemDataElement = itemParameter.CreateDataElement(cur);
                    allocatedArray.Fields[i] = itemDataElement;
                }

                //foreach (FieldValueTrait field in Traits.OfType<FieldValueTrait>().Where(t => t.FieldName != LengthParameterName))
                //{
                //    String fieldName = field.FieldName;
                //    Parameter parameter = field.FieldValueParameter;

                //    if (Int32.TryParse(fieldName, out Int32 index) && index < length)
                //    {
                //        IDataElement itemDataElement = parameter.CreateDataElement(cur);
                //        allocatedArray.Fields[index] = itemDataElement;
                //    }
                //}
            }

            objectReference.SetParameter(p, cur);
            return objectReference;
        }

        public static IDataElement CreateDataElement(this BooleanParameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4((p.Value.HasValue && p.Value.Value) ? 1 : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this CharParameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this ByteParameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this SByteParameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this Int16Parameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this Int32Parameter p, ExplicitActiveState cur)
        {
            Int4 element = new Int4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this Int64Parameter p, ExplicitActiveState cur)
        {
            Int8 element = new Int8(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this UInt16Parameter p, ExplicitActiveState cur)
        {
            UnsignedInt4 element = new UnsignedInt4(p.Value.HasValue ? p.Value.Value : 0U);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this UInt32Parameter p, ExplicitActiveState cur)
        {
            UnsignedInt4 element = new UnsignedInt4(p.Value.HasValue ? p.Value.Value : 0U);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this UInt64Parameter p, ExplicitActiveState cur)
        {
            UnsignedInt8 element = new UnsignedInt8(p.Value.HasValue ? p.Value.Value : 0U);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this SingleParameter p, ExplicitActiveState cur)
        {
            Float4 element = new Float4(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
        public static IDataElement CreateDataElement(this DoubleParameter p, ExplicitActiveState cur)
        {
            Float8 element = new Float8(p.Value.HasValue ? p.Value.Value : 0);
            element.SetParameter(p, cur);
            return element;
        }
    }
}
