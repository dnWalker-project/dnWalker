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
        private int _length;
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


        public int Length
        {
            get { return _length; }
            set 
            {
                _length = Resize(value);
            }
        }

        public string ElementTypeName
        {
            get { return _elementTypeName; }
        }


        private int Resize(int minLength)
        {
            if (minLength < _items.Length) return minLength;

            IParameter?[] newItems = new IParameter[minLength];
            _items.CopyTo(newItems, 0);

            _items = newItems;
            return minLength;
        }

        public IParameter?[] GetItems()
        {
            IParameter?[] items = new IParameter[_length];
            Array.Copy(_items, items, _length);

            return items;
        }

        public bool TryGetItem(int index, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (index > 0 && index < _length)
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
                if (index >= _length)
                {
                    _length = Resize(index + 1);
                }

                _items[index] = parameter;
                parameter.Accessor = new ItemParameterAccessor(index, this);
            }
        }

        public void ClearItem(int index)
        {
            if (index >= _length)
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
            return _items.Where(p => p != null).Select(p => p!);
        }
    }
}
