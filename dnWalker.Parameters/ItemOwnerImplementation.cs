using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    internal class ItemOwnerImplementation : IItemOwner
    {
        private readonly ParameterRef _ownerRef;
        private readonly IParameterSet _context;

        private ParameterRef[] _items = ParameterRef.EmptyArray;

        public ItemOwnerImplementation(ParameterRef ownerRef, IParameterSet context)
        {
            _ownerRef = ownerRef;
            _context = context;
        }

        public int? Length
        {
            get;
            set;
        }

        private bool HasDefinedLength(out int length)
        {
            if (Length is null or < 0)
            {
                length = 0;
                return false;
            }
            length = Length.Value;
            return true;
        }

        private int GetActualLength()
        {
            if (!HasDefinedLength(out int length))
            {
                int lastNonEmpty = Array.FindLastIndex(_items, r => r != ParameterRef.Empty);
                return lastNonEmpty + 1;
            }
            return length;
        }

        public ParameterRef[] GetItems()
        {
            int length = GetActualLength();
            if (length == 0) return ParameterRef.EmptyArray;

            ParameterRef[] refs = new ParameterRef[length];
            _items.CopyTo(refs, 0);

            return refs;

            //return RuntimeHelpers.GetSubArray(_items, new Range(0, GetActualLength() - 1));
        }

        public bool TryGetItem(int index, out ParameterRef parameterRef)
        {
            if (index < 0 ||
                (HasDefinedLength(out int length) && index >= length) ||
                index >= _items.Length)
            {
                parameterRef = ParameterRef.Empty;
                return false;
            }
            parameterRef = _items[index];
            return parameterRef != ParameterRef.Empty;
        }

        private void Resize(int minimalLength)
        {
            if (_items.Length >= minimalLength) return;

            ParameterRef[] newItems = new ParameterRef[minimalLength];
            _items.CopyTo(newItems, 0);
            _items = newItems;
        }

        public void SetItem(int index, ParameterRef parameterRef)
        {
            if (index < 0) return;

            ClearItem(index);

            Resize(index + 1);
            _items[index] = parameterRef;

            if (parameterRef.TryResolve(_context, out IParameter? itemParameter))
            {
                itemParameter.Accessors.Add(new ItemParameterAccessor(index, _ownerRef));
            }
            else
            {
                // throw new Exception("Trying to set item with an unknown parameter!");
            }
        }

        public void ClearItem(int index)
        {
            if (index < 0 || index >= _items.Length) return;

            if (_items[index].TryResolve(_context, out IParameter? itemParameter))
            {
                itemParameter.Accessors.RemoveAt(itemParameter.Accessors.IndexOf(pa => pa is ItemParameterAccessor ia && ia.ParentRef == _ownerRef && ia.Index == index));
                //itemParameter.Accessor = null;
            }

            _items[index] = ParameterRef.Empty;
        }

        public void CopyTo(ItemOwnerImplementation other)
        {
            other.Length = Length;
            other._items = (ParameterRef[]) _items.Clone();
        }
    }
}
