using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
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

            AllocatedArray parentArray = (AllocatedArray)cur.DynamicArea.Allocations[(int)_locationMapping[parentLocation]];
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

            AllocatedObject parentObject = (AllocatedObject)cur.DynamicArea.Allocations[(int)_locationMapping[parentLocation]];
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

        private readonly Dictionary<Location, uint> _locationMapping = new Dictionary<Location, uint>();

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
                else if (_locationMapping.TryGetValue(location, out uint heapAddress))
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
                    _locationMapping[location] = or.Location - 1;
                    result = or;
                }
            }
            else
            {
                result = value.ToDataElement();
            }

            return result;
        }

    }
}
