using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Utils;
using dnWalker.Symbolic.Variables;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public partial class SymbolicContext
    {
        private bool EnsureInitialized(MethodArgumentVariable methodArgument, ExplicitActiveState cur)
        {
            DataElementList args = cur.CurrentMethod.Arguments;
            int index = methodArgument.Parameter.Index;

            IDataElement de = args[index];
            if (de.TryGetExpression(cur, out Expression argExpr))
            {
                // already initialized => do nothing & return false - did nothing
                return false;
            }

            // NOT initialized => we must initialize it
            de = LazyInitialize(methodArgument, cur);
            de.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(methodArgument));

            args[index] = de;

            return true;
        }


        private bool EnsureInitialized(StaticFieldVariable staticField, ExplicitActiveState cur)
        {
            TypeDef theClass = staticField.Field.DeclaringType.ResolveTypeDefThrow();
            theClass.InitLayout();
            AllocatedClass allocatedClass = cur.StaticArea.GetClass(theClass);

            // done, but it is not very good... TODO::: will probably not work, as the class layout is NOT YET DONE!!!!
            int index = (int)staticField.Field.ResolveFieldDefThrow().FieldOffset.Value;

            IDataElement de = allocatedClass.Fields[index];

            if (de.TryGetExpression(cur, out Expression argExpr))
            {
                // already initialized => do nothing & return false - did nothing
                return false;
            }

            // NOT initialized => we must initialize it
            de = LazyInitialize(staticField, cur);
            de.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(staticField));

            allocatedClass.Fields[index] = de;

            return true;
        }

        private bool EnsureInitialized(ArrayElementVariable arrayElement, ExplicitActiveState cur)
        {
            int index = (int)arrayElement.Index;

            if (!_inputModel.TryGetValue(arrayElement.Parent, out IValue parent)) throw new InvalidOperationException("This should not happen.");

            Location parentLocation = (Location)parent;

            if (parentLocation == Location.Null) return false;

            AllocatedArray parentArray = (AllocatedArray)cur.DynamicArea.Allocations[(int)_symToConcrMapping[parentLocation]];

            if (index < 0 || index >= parentArray.Fields.Length) return false;
            
            IDataElement de = parentArray.Fields[index];

            if (de.TryGetExpression(cur, out Expression elemExpr))
            {
                // already initialized => do nothing & return false - did nothing
                return false;
            }

            // NOT initialized => we must initialize it
            de = LazyInitialize(arrayElement, cur);
            de.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(arrayElement));

            parentArray.Fields[index] = de;

            return true;
        }

        private bool EnsureInitialized(InstanceFieldVariable instanceField, ExplicitActiveState cur)
        {
            // done, but it is not very good... TODO::: will probably not work, as the class layout is NOT YET DONE!!!!
            instanceField.Field.DeclaringType.ResolveTypeDefThrow().InitLayout();
            int index = (int)instanceField.Field.ResolveFieldDefThrow().FieldOffset;

            if (!_inputModel.TryGetValue(instanceField.Parent, out IValue parent)) throw new InvalidOperationException("This should not happen.");

            Location parentLocation = (Location)parent;

            AllocatedObject parentObject = (AllocatedObject)cur.DynamicArea.Allocations[(int)_symToConcrMapping[parentLocation]];
            IDataElement de = parentObject.Fields[index];

            if (de.TryGetExpression(cur, out Expression elemExpr))
            {
                // already initialized => do nothing & return false - did nothing
                return false;
            }

            // NOT initialized => we must initialize it
            de = LazyInitialize(instanceField, cur);
            de.SetExpression(cur, cur.GetExpressionFactory().MakeVariable(instanceField));

            parentObject.Fields[index] = de;

            return true;
        }

        private readonly Dictionary<Location, uint> _symToConcrMapping = new Dictionary<Location, uint>();
        private readonly Dictionary<uint, Location> _concrToSymMapping = new Dictionary<uint, Location>();

        public IDataElement LazyInitialize(IVariable variable, ExplicitActiveState cur)
        {
            IDataElement result;
            if (!_inputModel.TryGetValue(variable, out IValue value))
            {
                // the model knows nothing about that value => use default
                value = ValueFactory.GetDefault(variable.Type);
                _inputModel.TrySetValue(variable, value);
            }

            // the input has some information about the value => we must create the data element for it
            if (value is Location location)
            {
                if (location == Location.Null)
                {
                    // the location is null => return null object reference
                    result = new ObjectReference(0);
                }
                else if (_symToConcrMapping.TryGetValue(location, out uint heapAddress))
                {
                    // the location is not null && its node has already been allocated => return object reference which points at that allocation
                    // heap address starts at 0 and IS NOT NULL => the transformation +1
                    result  = new ObjectReference(heapAddress + 1);
                }
                else
                {
                    // the location is not null && its node has not been allocated => allocated it && return object reference which points to that allocation
                    DynamicArea da = cur.DynamicArea;
                    IHeapNode node = _inputModel.HeapInfo.GetNode(location);
                    ObjectReference or = node switch
                    {
                        IArrayHeapNode arrNode => da.AllocateArray(node.Type.ToTypeDefOrRef(), arrNode.Length),
                        IObjectHeapNode _ => da.AllocateObject(node.Type.ToTypeDefOrRef()),
                        _ => throw new InvalidOperationException("Unexpected heap node type.")
                    };

                    // heap address starts at 0 and IS NOT NULL => the transformation +1
                    _symToConcrMapping[location] = or.Location - 1;
                    _concrToSymMapping[or.Location - 1] = location;
                    result = or;
                }
            }
            else
            {
                result = value.ToDataElement();
            }

            _outputModel.TrySetValue(variable, value);

            return result;
        }


        public Location ProcessExistingReference(ObjectReference objRef, ExplicitActiveState cur)
        {
            if (objRef.IsNull()) return Location.Null;

            if (_concrToSymMapping.TryGetValue(objRef.Location - 1, out Location location)) return location;

            DynamicAllocation allocation = cur.DynamicArea.Allocations[objRef];
            location = allocation switch
            {
                AllocatedArray arrayAllocation => ProcessExistingArray(arrayAllocation, cur),
                AllocatedObject objectAllocation => ProcessExistingObject(objectAllocation, cur),
                _ => throw new NotSupportedException()
            };

            _symToConcrMapping[location] = objRef.Location - 1;
            _concrToSymMapping[objRef.Location - 1] = location;

            return location;
        }

        private Location ProcessExistingObject(AllocatedObject objectAllocation, ExplicitActiveState cur)
        {
            // we assume that this location has no mapping yet
            IObjectHeapNode heapNode = _outputModel.HeapInfo.InitializeObject(objectAllocation.Type.ToTypeSig());

            TypeDef type = objectAllocation.Type.ResolveTypeDefThrow();
            type.InitLayout();
            for (int i = 0; i < objectAllocation.Fields.Length; ++i)
            {
                IField field = objectAllocation.FieldDefs[i];
                IValue fieldValue;
                if (field.FieldSig.Type.IsPrimitive || field.FieldSig.Type.IsString())
                {
                    fieldValue = objectAllocation.Fields[i].AsModelValue(field.FieldSig.Type);
                }
                else
                {
                    ObjectReference refValue = (ObjectReference)objectAllocation.Fields[i];
                    fieldValue = ProcessExistingReference(refValue, cur);
                }

                heapNode.SetField(field, fieldValue);
            }

            return heapNode.Location;
        }

        private Location ProcessExistingArray(AllocatedArray arrayAllocation, ExplicitActiveState cur)
        {
            // we assume that this location has no mapping yet
            IArrayHeapNode heapNode = _outputModel.HeapInfo.InitializeArray(arrayAllocation.Type.ToTypeSig(), arrayAllocation.Fields.Length);

            TypeSig elementType = arrayAllocation.Type.ToTypeSig();
            bool primitiveOrString = elementType.IsPrimitive || elementType.IsString();

            for (int i = 0; i < arrayAllocation.Fields.Length; ++i)
            {
                IValue elementValue;
                if (primitiveOrString)
                {
                    elementValue = arrayAllocation.Fields[i].AsModelValue(elementType);
                }
                else
                {
                    ObjectReference refValue = (ObjectReference)arrayAllocation.Fields[i];
                    elementValue = ProcessExistingReference(refValue, cur);
                }

                heapNode.SetElement(i, elementValue);
            }

            return heapNode.Location;
        }
    }
}
