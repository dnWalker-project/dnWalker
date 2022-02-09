using dnlib.DotNet;

using dnWalker.DataElements;
using dnWalker.TypeSystem;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        public static IParameter GetOrCreateParameter(this IDataElement dataElement, ExplicitActiveState cur, TypeSignature expectedType)
        {
            if (dataElement.TryGetParameter(cur, out IParameter parameter))
            {
                // parameter is already associated with this IDataElement => return it XXXX
                // return its alias / copy if primitive value
                return parameter;
            }

            if (dataElement is ObjectReference objectReference)
            {
                if (objectReference.IsNull())
                {
                    parameter = CreateNullParameter(expectedType, cur);
                }
                else
                {
                    DynamicAllocation allocation = cur.DynamicArea.Allocations[objectReference];

                    if (allocation is AllocatedArray allocatedArray)
                    {
                        parameter = CreateArrayParameter(allocatedArray, cur);
                    }
                    else if (allocation is AllocatedObject allocatedObject)
                    {
                        parameter = CreateObjectParameter(allocatedObject, cur);
                    }
                    else if (allocation is AllocatedDelegate allocatedDelegate)
                    {
                        // delegate parameters are not implemented right now => we cannot substitute 
                        throw new NotImplementedException("TODO: implement delegate parameter???");
                        //return null;
                    }
                    else
                    {
                        throw new NotSupportedException($"Cannot create parameter from allocation of type {allocation.GetType()}");
                        //return null;
                    }
                }
            }
            else if (dataElement is INumericElement numeric)
            {
                parameter = CreatePrimitiveValueParameter(numeric, expectedType.FullName, cur);
            }
            else
            {
                return null;
            }


            if (parameter != null)
            {
                dataElement.SetParameter(parameter, cur);
            }

            return parameter;
        }

        private static IParameter CreateNullParameter(TypeSignature expectedType, ExplicitActiveState cur)
        {
            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                throw new InvalidOperationException("Cannot create a new parameter without the parameter store!");
            }

            IParameterSet executionContext = store.ExecutionSet;


            //if (expectedType.IsArray)
            //{
            //    var arraySig = expectedType.ToArraySig();
                
            //    return executionContext.CreateArrayParameter(new TypeSignature(arraySig.Next.ToTypeDefOrRef()), isNull: true);

            //}
            //else if (expectedType.IsSZArray)
            //{
            //    var arraySig = expectedType.ToSZArraySig();

            //    return executionContext.CreateArrayParameter(new TypeSignature(arraySig.Next.ToTypeDefOrRef()), isNull: true);
            //}
            if (expectedType.IsArray || expectedType.IsSZArray)
            {
                // different parameter type for ARRAY and SZARRAY ? Or extend the IArrayParameter to work with multi dimensional arrays?
                return executionContext.CreateArrayParameter(expectedType.ElementType, isNull: true);
            }
            else if (expectedType.IsInterface || !expectedType.IsValueType)
            {
                return executionContext.CreateObjectParameter(expectedType, isNull: true);
            }
            else
            {
                throw new NotSupportedException($"Unsupported TypeSig: {expectedType}");
            }

        }

        private static IParameter CreateArrayParameter(AllocatedArray allocatedArray, ExplicitActiveState cur)
        {
            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                throw new InvalidOperationException("Cannot create a new parameter without the parameter store!");
            }

            IParameterSet context = store.ExecutionSet;

            TypeSignature elementSig = new TypeSignature(allocatedArray.Type);

            int length = allocatedArray.Fields.Length;

            IArrayParameter parameter = context.CreateArrayParameter(new TypeSignature(elementSig.ToTypeDefOrRef()), length, false);
            for (int i = 0; i < length; ++i)
            {
                parameter.SetItem(i, GetOrCreateParameter(allocatedArray.Fields[i], cur, elementSig));
            }

            return parameter;
        }

        private static IParameter CreateObjectParameter(AllocatedObject allocatedObject, ExplicitActiveState cur)
        {
            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                throw new InvalidOperationException("Cannot create a new parameter without the parameter store!");
            }

            IParameterSet context = store.ExecutionSet;

            IObjectParameter objectParameter = context.CreateObjectParameter(new TypeSignature(allocatedObject.Type), isNull: false);

            IReadOnlyList<FieldDef> fields = GetFieldMapping(allocatedObject.Type);

            for (int i = 0; i < fields.Count(); ++i)
            {
                IParameter fieldParameter = GetOrCreateParameter(allocatedObject.Fields[i], cur, new TypeSignature(fields[i].FieldType.ToTypeDefOrRef()));

                objectParameter.SetField(fields[i].Name, fieldParameter);
            }

            return objectParameter;
        }

        private static IReadOnlyList<FieldDef> GetFieldMapping(ITypeDefOrRef type)
        {
            List<FieldDef> fields = new List<FieldDef>();

            foreach (var typeDefOrRef in type.InheritanceEnumerator())
            {
                fields.AddRange(typeDefOrRef.ResolveTypeDef().Fields);
            }

            return fields;
        }

        private static IParameter CreatePrimitiveValueParameter(INumericElement dataElement, string expectedTypeName, ExplicitActiveState cur)
        {
            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                throw new InvalidOperationException("Cannot create a new parameter without the parameter store!");
            }

            IParameterSet context = store.ExecutionSet;
            IPrimitiveValueParameter parameter;

            switch (expectedTypeName)
            {
                case "System.Boolean":
                    parameter = context.CreateBooleanParameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.Byte":
                    parameter = context.CreateByteParameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.SByte":
                    parameter = context.CreateSByteParameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.Int16":
                    parameter = context.CreateInt16Parameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.Int32":
                    parameter = context.CreateInt32Parameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.Int64":
                    parameter = context.CreateInt64Parameter();
                    parameter.Value = dataElement.ToInt8(false).Value;
                    break;

                case "System.UInt16":
                    parameter = context.CreateUInt16Parameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.UInt32":
                    parameter = context.CreateUInt32Parameter();
                    parameter.Value = dataElement.ToUnsignedInt4(false).Value;
                    break;

                case "System.UInt64":
                    parameter = context.CreateUInt64Parameter();
                    parameter.Value = dataElement.ToUnsignedInt8(false).Value;
                    break;

                case "System.Char":
                    parameter = context.CreateCharParameter();
                    parameter.Value = dataElement.ToInt4(false).Value;
                    break;

                case "System.Single":
                    parameter = context.CreateSingleParameter();
                    parameter.Value = dataElement.ToFloat4(false).Value;
                    break;

                case "System.Double":
                    parameter = context.CreateDoubleParameter();
                    parameter.Value = dataElement.ToFloat8(false).Value;
                    break;

                default: throw new Exception($"Unexpected primitive value parameter type {expectedTypeName}.");
            }

            return parameter;
        }
    }
}
