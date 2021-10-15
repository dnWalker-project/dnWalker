using dnlib.DotNet;

using dnWalker.DataElements;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ParameterStore
    {
        private readonly ExplicitActiveState _cur;

        private readonly Dictionary<Int32, StoredItemInfo> _items = new Dictionary<Int32, StoredItemInfo>();

        

        public ParameterStore(ExplicitActiveState cur)
        {
            _cur = cur;
        }

        public IDataElement Get(Int32 id, TypeDef expectedType)
        {
            if (_items.TryGetValue(id, out StoredItemInfo itemInfo))
            {
                return itemInfo.ConstructDataElement(_cur);
            }
            else
            {
                // default values...
                if (expectedType.IsInterface || expectedType.IsClass)
                {
                    return ObjectReference.Null;
                }
                else // if (expectedType.IsValueType)
                {
                    throw new NotImplementedException("TODO implement default value for all ValueTypes");
                }
            }
        }

        //public StoredObjectInfoBuilder IsObject(Int32 id, TypeDef typeDef)
        //{
        //    StoredObjectInfo objectInfo
        //}

        public void SetUpToken(Int32 id, Action<StoredTokenInfo> setup)
        {
            StoredTokenInfo tokenInfo = null;
            if (!_items.TryGetValue(id, out StoredItemInfo itemInfo))
            {
                tokenInfo = new StoredTokenInfo(id);
                _items[id] = tokenInfo;
            }
            else if (itemInfo is StoredObjectInfo objectInfo)
            {
                throw new InvalidOperationException("Cannot setup OBJECT using TOKEN setup.");
            }
            else if (itemInfo is StoredValueInfo valueInfo)
            {
                throw new InvalidOperationException("Cannot setup VALUE using TOKEN setup.");
            }
            else
            {
                tokenInfo = (StoredTokenInfo)itemInfo;
            }
            setup(tokenInfo);
        }

        public void SetUpObject(Int32 id, Action<StoredObjectInfo> setup)
        {
            StoredObjectInfo objectInfo = null;
            if (!_items.TryGetValue(id, out StoredItemInfo itemInfo))
            {
                objectInfo = new StoredObjectInfo(id);
                _items[id] = objectInfo;
            }
            else if (itemInfo is StoredTokenInfo tokenInfo)
            {
                throw new InvalidOperationException("Cannot setup TOKEN using OBJECT setup.");
            }
            else if (itemInfo is StoredValueInfo valueInfo)
            {
                throw new InvalidOperationException("Cannot setup VALUE using OBJECT setup.");
            }
            else
            {
                objectInfo = (StoredObjectInfo)itemInfo;
            }

            setup(objectInfo);
        }
    }

    public abstract class StoredItemInfo
    {
        private readonly Int32 _id;

        protected StoredItemInfo(Int32 id)
        {
            Id = id;
        }

        public Int32 Id { get; }

        public abstract IDataElement ConstructDataElement(ExplicitActiveState cur);

    }

    /// <summary>
    /// Represents stored basic types, e.g. numbers, constants etc.
    /// </summary>
    public class StoredTokenInfo : StoredItemInfo
    {
        public StoredTokenInfo(Int32 id) : base(id)
        {
        }

        //public TypeCode TypeCode
        //{
        //    get;set;
        //}
        public Object Value
        {
            get;set;
        }

        public override IDataElement ConstructDataElement(ExplicitActiveState cur)
        {
            return cur.DefinitionProvider.CreateDataElement(Value);
        }
    }


    /// <summary>
    /// Represents stored objects of value type.
    /// </summary>
    public class StoredValueInfo : StoredItemInfo
    {
        public StoredValueInfo(Int32 id) : base(id)
        {
        }

        public TypeDef Type { get; set; }
        public override IDataElement ConstructDataElement(ExplicitActiveState cur)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents stored objects of reference type.
    /// </summary>
    public class StoredObjectInfo : StoredItemInfo
    {
        public StoredObjectInfo(Int32 id) : base(id)
        {
        }

        public TypeDef Type { get; set; }
        public Boolean IsNull { get; set; }
        public Boolean IsInterface
        {
            get { return Type.IsInterface; } 
        }

        public override IDataElement ConstructDataElement(ExplicitActiveState cur)
        {
            if (IsNull) return ObjectReference.Null;

            if (IsInterface) return new InterfaceProxy(Type);

            return cur.DynamicArea.AllocateObject(Type);
        }
    }

    //public class StoredItemBuilder
    //{
    //    private readonly Int32 _id;

    //    public StoredObjectBuilder IsObject()
    //    {
    //        return new StoredObjectBuilder(_id);
    //    }

    //    public void IsString(String value)
    //    {

    //    }

    //    public void IsNumber(Int32 value)
    //    {

    //    }

    //    public void IsNumber(Single value)
    //    {

    //    }

    //    public void IsNumber(Double value)
    //    {

    //    }
    //}

    //public class StoredObjectBuilder
    //{
    //    private readonly Int32 _id;

    //    private TypeDef _type;

    //    internal StoredObjectBuilder(Int32 id)
    //    {
    //        _id = id;
    //    }

    //    public TypeDef Type { get; }

    //    public StoredObjectBuilder OfType(TypeDef type)
    //    {
    //        _type = type;
    //        return this;
    //    }
    //}


}
