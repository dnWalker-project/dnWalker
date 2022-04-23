using dnlib.DotNet;

using dnWalker.TypeSystem;
using dnWalker.Symbolic.Heap;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnWalker.Symbolic.Expressions;

namespace dnWalker.Symbolic
{
    public static class ExplicitActiveStateExtensions
    {
        private const string ModelAttribute = "model";

        public static IModel GetModel(this ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(ModelAttribute, out IModel model))
            {
                model = new Model();
                cur.SetModel(model);
            }
            return model;
        }

        public static void SetModel(this ExplicitActiveState cur, IModel model)
        {
            cur.PathStore.CurrentPath.SetPathAttribute(ModelAttribute, model);
        }

        /// <summary>
        /// Setup the <paramref name="model"/> as in/put model for the active state.
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="model"></param>
        public static void Initialize(this ExplicitActiveState cur, IModel model)
        {
            cur.SetModel(model);

            Dictionary<Location, uint> locationMapping = new Dictionary<Location, uint>();
            cur.SetLocationMapping(locationMapping);

            DataElementList args = cur.CurrentMethod.Arguments;

            ParameterList paramList = cur.CurrentMethod.Definition.Parameters;
            for (int i = 0; i < args.Length; ++i)
            {
                MethodArgVar methodArgVar = new MethodArgVar(paramList[i].Name, paramList[i].Type);

                // if the valuation does not exist, create default
                IValue value = model.GetValueOrDefault(methodArgVar);

                if (value is Location location &&
                    location != Location.Null)
                {
                    // the value is a location => initialize it on the heap
                    if (!model.HeapInfo.TryGetNode(location, out IHeapNode heapNode)) System.Diagnostics.Debug.Fail("NonNull location has no node.");

                    ObjectReference or = heapNode switch
                    {
                        IObjectHeapNode objectNode => cur.DynamicArea.AllocateObject(objectNode.Type.ToTypeDefOrRef()),
                        IArrayHeapNode arrayNode => cur.DynamicArea.AllocateArray(arrayNode.Type.ToTypeDefOrRef(), arrayNode.Length),
                        // IStringHeapNode stringNode => cur.DynamicArea.AllocateString(stringNode.Type.ToTypeDefOrRef(), stringNode.Value),
                        _ => throw new NotSupportedException("Unexpected heap node")
                    };

                    Allocation allocation = cur.DynamicArea.Allocations[or];
                    allocation.SetHeapNode(heapNode, cur);
                    
                    // pin it - we assume these objects were created outside and as such have some outside references
                    cur.DynamicArea.SetPinnedAllocation(or, true);

                    locationMapping[location] = or.Location;
                }

                IDataElement valueDE = CreateDataElement(value);
                valueDE.SetExpression(new VariableExpression(methodArgVar), cur);
                args[i] = valueDE;
            }



            // eager loading - we want lazy
            //// 1. allocate objects on heap
            //{
            //    IHeapInfo heapInfo = model.HeapInfo;
            //    DynamicArea da = cur.DynamicArea;

            //    foreach (IHeapNode heapNode in heapInfo.Nodes)
            //    {
            //        ObjectReference oRef = ObjectReference.Null;
            //        if (heapNode is IObjectHeapNode objectNode)
            //        {
            //            oRef = da.AllocateObject(objectNode.Type.ToTypeDefOrRef());
            //        }
            //        else if (heapNode is IArrayHeapNode arrayNode)
            //        {
            //            oRef = da.AllocateArray(arrayNode.Type.ToTypeDefOrRef(), arrayNode.Length);

            //        }

            //        if (oRef.IsNull()) continue;
            //        da.SetPinnedAllocation(oRef, true);

            //        Allocation allocation = da.Allocations[oRef];
            //        allocation.SetHeapNode(heapNode, cur);

            //        locationMapping[heapNode.Location] = oRef.Location;
            //    }
            //}

            //// 2. set the variables
            //// 2.1. start with the method args - we need to ensure all of them are set
            //{
            //    ParameterList paramList = cur.CurrentMethod.Definition.Parameters;
            //    for (int i = 0; i < args.Length; ++i)
            //    {
            //        MethodArgVar methodArgVar = new MethodArgVar(paramList[i].Name, paramList[i].Type);

            //        // if the valuation does not exist, create default
            //        IValue value = model.GetValueOrDefault(methodArgVar);

            //        IDataElement valueDE =  CreateDataElement(value);
            //        args[i] = valueDE;
            //    }
            //}
            //// 2.2. now we evaluate all other variables (i.e. StaticField, InstanceField, Element, MethodResult)
            //// WE IGNORE THESE VARIABLES -> we will initialize lazily
            //{
            //    DynamicArea da = cur.DynamicArea;
            //    StaticArea sa = cur.StaticArea;

            //    foreach (Valuation valuation in model.GetValuations())
            //    {
            //        IVariable variable = valuation.Variable;
            //        IValue value = valuation.Value;

            //        switch (variable)
            //        {
            //            case MethodArgVar _: break;  // already set up
            //            case StaticFieldVar staticField:
            //                {
            //                    // update the AllocatedClass
            //                    IDataElement de = CreateDataElement(value);
            //                    ITypeDefOrRef classTD = staticField.Field.DeclaringType;
            //                    // TODO: ensure static area can work with ITypeDefOrRef
            //                    // otherwise generic fields will not be supported!!!
            //                    AllocatedClass allocatedClass = sa.GetClass(classTD.ResolveTypeDefThrow());
            //                    allocatedClass.Fields[DefinitionProviderExtensions.GetFieldOffset(staticField.Field)] = de;
            //                    break;
            //                }
            //            case InstanceFieldVar instanceFieldVar:
            //                {
            //                    // update the AllocatedObject
            //                    IDataElement de = CreateDataElement(value);

            //                    break;
            //                }

            //            default:
            //                throw new NotSupportedException("Unexpected variable type.");
            //        }
            //    }
            //}


            IDataElement CreateDataElement(IValue value)
            {
                return value switch
                {
                    Location loc => loc == Location.Null ? new ObjectReference(0) : new ObjectReference(locationMapping[loc]), // we need to create a new ObjectReference for each occurrence
                    PrimitiveValue<sbyte> primitive => new Int4(primitive.Value),
                    PrimitiveValue<byte> primitive => new Int4(primitive.Value),
                    PrimitiveValue<short> primitive => new Int4(primitive.Value),
                    PrimitiveValue<ushort> primitive => new Int4(primitive.Value),
                    PrimitiveValue<int> primitive => new Int4(primitive.Value),
                    PrimitiveValue<uint> primitive => new UnsignedInt4(primitive.Value),
                    PrimitiveValue<bool> primitive => new Int4(primitive.Value ? 1 : 0),
                    
                    PrimitiveValue<long> primitive => new Int8(primitive.Value),
                    PrimitiveValue<ulong> primitive => new UnsignedInt8(primitive.Value),

                    PrimitiveValue<float> primitive => new Float4(primitive.Value),
                    PrimitiveValue<double> primitive => new Float8(primitive.Value),

                    _ => throw new NotSupportedException("Unexpected value type.")
                };
            }
        }
    }
}
