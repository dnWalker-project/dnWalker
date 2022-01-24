using dnlib.DotNet;

using dnWalker.TypeSystem;

using MMC;
using MMC.Data;
using MMC.State;
using System;
using System.Collections.Generic;

namespace dnWalker.DataElements
{
    public class ReturnValue : IDataElement, IComparable
    {
        private readonly Allocation _allocatedObject;
        private readonly ExplicitActiveState _explicitActiveState;

        internal ReturnValue(Allocation allocatedObject, ExplicitActiveState explicitActiveState)
        {
            _allocatedObject = allocatedObject;
            _explicitActiveState = explicitActiveState;
            HashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        public int HashCode { get; }

        string IDataElement.WrapperName => throw new NotImplementedException();

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }

            if (_allocatedObject.Type.FullName != obj.GetType().FullName)
            {
                return -1;
            }

            var visitor = new EqVisitor
            {
                BaseObject = obj
            };
            _allocatedObject.Accept(visitor, _explicitActiveState);
            return (int)visitor.Result;
        }

        bool IDataElement.Equals(IDataElement other)
        {
            throw new NotImplementedException();
        }

        bool IDataElement.ToBool()
        {
            throw new NotImplementedException();
        }

        string IDataElement.ToString()
        {
            throw new NotImplementedException();
        }

        private class EqVisitor : IStorageVisitor
        {
            public object Result { get; private set; }
            
            public object BaseObject { get; internal set; }

            public EqVisitor()
            {
                Result = -1;
            }

            public void VisitActiveState(ExplicitActiveState act)
            {
                throw new NotImplementedException();
            }

            public void VisitAllocatedArray(AllocatedArray arr, ExplicitActiveState cur)
            {
                throw new NotImplementedException();
            }

            public void VisitAllocatedClass(AllocatedClass cls)
            {
                throw new NotImplementedException();
            }

            public void VisitAllocatedDelegate(AllocatedDelegate del, ExplicitActiveState cur)
            {
                throw new NotImplementedException();
            }

            public void VisitAllocatedObject(AllocatedObject ao, ExplicitActiveState cur)
            {
                var fields = new List<FieldDef>();
                foreach (var typeDefOrRef in ao.Type.InheritanceEnumerator())
                {
                    fields.AddRange(typeDefOrRef.ResolveTypeDef().Fields);
                }

                foreach (var field in fields)
                {
                    var f = BaseObject.GetType().GetField(field.Name, System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Instance
                            | System.Reflection.BindingFlags.Public);
                    
                    if (f == null)
                    {
                        throw new NullReferenceException($"Field {field.FullName} not found.");
                    }

                    var objValue = f.GetValue(BaseObject);
                    var fieldValue = ao.Fields[(int)field.FieldOffset.Value];
                    if (!DataElement.CreateDataElement(objValue, cur.DefinitionProvider).Equals(fieldValue))
                    {
                        Result = -1;
                        return;
                    }
                }

                Result = 0;
            }

            public void VisitDynamicArea(DynamicArea dyn, ExplicitActiveState cur)
            {
                throw new NotImplementedException();
            }

            public void VisitMethodState(MethodState meth)
            {
                throw new NotImplementedException();
            }

            public void VisitStaticArea(IStaticArea stat)
            {
                throw new NotImplementedException();
            }

            public void VisitThreadPool(ThreadPool tp)
            {
                throw new NotImplementedException();
            }

            public void VisitThreadState(ThreadState trd)
            {
                throw new NotImplementedException();
            }
        }
    }
}
