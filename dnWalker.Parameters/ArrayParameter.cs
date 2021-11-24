using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class ArrayParameter : ReferenceTypeParameter
    {
        public static readonly string LengthName = "#__Length__";

        private string _elementTypeName;
        private static readonly Parameter?[] EmptyArray = new Parameter?[0];

        public ArrayParameter(string elementTypeName, string localName) : base(elementTypeName + "[]", localName)
        {
            _elementTypeName = elementTypeName;
            _lengthParameter = new Int32Parameter(LengthName, this);
        }

        public ArrayParameter(string elementTypeName, string localName, Parameter? owner) : base(elementTypeName + "[]", localName, owner)
        {
            _elementTypeName= elementTypeName;
            _lengthParameter = new Int32Parameter(LengthName, this);
        }

        public string ElementTypeName
        {
            get { return _elementTypeName; }
        }

        private readonly Int32Parameter _lengthParameter;

        public int Length
        {
            get { return _lengthParameter.Value ?? 0; }
            set 
            {
                int length = Length;
                // we are downsizing => do nothing;
                if (length > value)
                {
                    length = value;
                    return;
                }

                Resize(value);
            }
        }

        public Int32Parameter LengthParameter
        {
            get { return _lengthParameter; }
        }

        private void Resize(int newLength)
        {
            // round up to next power of 2 or something?
            Parameter[] newItems = new Parameter[newLength];

            int length = Length;

            Array.Copy(_items, newItems, length);

            _items = newItems;

            _lengthParameter.Value = newLength;
        }

        private Parameter?[] _items = EmptyArray;

        public void SetItem(int index, Parameter? item)
        {
            if (item == null)
            {
                ClearItem(index);
            }

            if (index > Length)
            {
                Resize(index + 1);
            }
        }

        private void ClearItem(int index)
        {
            if (index < Length)
            {
                Parameter? item = _items[index];
                if (item != null)
                {
                    _items[index] = null;
                    item.Owner = null;
                }
            }
        }

        public bool TryGetItem(int index, [NotNullWhen(true)]out Parameter? item)
        {
            if (index >= Length)
            {
                item = null;
                return false;
            }

            item = _items[index];
            return item != null;
        }


        public override IEnumerable<Parameter> GetOwnedParameters()
        {
            return ((IEnumerable<Parameter>)_items.Where(item => item != null)).Append(IsNullParameter).Append(LengthParameter);
        }
    }
}
