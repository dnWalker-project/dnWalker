using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ArrayParameter : ReferenceTypeParameter, IArrayParameter
    {
        private int? _length;
        private IParameter?[] _items = Array.Empty<IParameter?>();
        private readonly string _elementTypeName;


        public ArrayParameter(string elementTypeName) : base(elementTypeName + "[]")
        {
            _elementTypeName = elementTypeName;
        }

        public ArrayParameter(string elementTypeName, int id) : base(elementTypeName + "[]", id)
        {
            _elementTypeName = elementTypeName;
        }


        public int? Length
        {
            get { return _length; }
            set 
            {
                _length = value;
                if (value.HasValue) Resize(value.Value);
            }
        }

        public string ElementTypeName
        {
            get { return _elementTypeName; }
        }

        /// <summary>
        /// Updates the inner storage so it can contain enough items.
        /// </summary>
        /// <param name="minLength"></param>
        /// <returns></returns>
        private void Resize(int minLength)
        {
            if (minLength < _items.Length) return;

            IParameter?[] newItems = new IParameter[minLength];
            _items.CopyTo(newItems, 0);

            _items = newItems;
        }

        public IParameter?[] GetItems()
        {
            // if _length is null => we do not care about the length, ergo we just use the highest index + 1
            // if _length is not null and is lower than the length of the array, cut higher indeces off...
            int length = _length ?? _items.Length;

            IParameter?[] items = new IParameter[length];
            Array.Copy(_items, items, length);

            return items;
        }

        public bool TryGetItem(int index, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (index >= 0 && index < _length)
            {
                parameter = _items[index];
                return parameter != null;
            }

            parameter = null;
            return false;
        }

        public void SetItem(int index, IParameter? parameter)
        {
            ClearItem(index);
            if (parameter != null)
            {
                if (index >= _items.Length)
                {
                    Resize(index + 1);
                }

                _items[index] = parameter;
                parameter.Accessor = new ItemParameterAccessor(index, this);

            }
        }

        public void ClearItem(int index)
        {
            if (index >= _items.Length)
            {
                return;
            }

            ref IParameter? p = ref _items[index];

            if (p != null)
            {
                p.Accessor = null;
                p = null;
            }
        }

        public override IEnumerable<IParameter> GetChildren()
        {
            int length = _length ?? _items.Length;

            return _items
                .Select((p, i) => (p, i))
                .Where(tpl => tpl.i < length && tpl.p != null)
                .Select(tpl => tpl.p!);
        }

        public override IParameter ShallowCopy(ParameterStore store, int id)
        {
            ArrayParameter arrayParameter = new ArrayParameter(ElementTypeName, id);
            arrayParameter.Length = Length;
            arrayParameter.IsNull = IsNull;

            for (int i = 0; i < _items.Length; ++i)
            {
                IParameter? itemParameter = _items[i];
                if (itemParameter is IReferenceTypeParameter refTypeItem)
                {
                    arrayParameter.SetItem(i, refTypeItem.CreateAlias(store));
                }
                else if (itemParameter is IPrimitiveValueParameter valueItem)
                {
                    // can only create alias for a reference type parameter
                    arrayParameter.SetItem(i, valueItem.ShallowCopy(store));
                }
            }

            return arrayParameter;
        }
    }
}
