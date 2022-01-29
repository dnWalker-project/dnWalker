using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{

    public class ArrayParameter : ReferenceTypeParameter, IArrayParameter
    {
        private ItemOwnerImplementation _items;

        internal ArrayParameter(IParameterSet set, TypeSignature elementType) : base(set, elementType.MakeArray())
        {
            ElementType = elementType;
            _items = new ItemOwnerImplementation(Reference, Set);
        }
        internal ArrayParameter(IParameterSet set, TypeSignature elementType, ParameterRef reference) : base(set, elementType.MakeArray(), reference)
        {
            ElementType = elementType;
            _items = new ItemOwnerImplementation(Reference, Set);
        }

        public TypeSignature ElementType
        {
            get;
        }



        public override ArrayParameter CloneData(IParameterSet newContext)
        {
            ArrayParameter arrayParameter = new ArrayParameter(newContext, ElementType, Reference)
            {
                IsNull = IsNull,
                Length = Length,
            };
            //foreach (var a in Accessors.Select(ac => ac.Clone()))
            //{
            //    arrayParameter.Accessors.Add(a);
            //}

            _items.CopyTo(arrayParameter._items);

            return arrayParameter;
        }

        #region IItemOwner Members
        public int? Length
        {
            get
            {
                return ((IItemOwner)_items).Length;
            }

            set
            {
                ((IItemOwner)_items).Length = value;
            }
        }

        public ParameterRef[] GetItems()
        {
            return ((IItemOwner)_items).GetItems();
        }

        public bool TryGetItem(int index, out ParameterRef itemRef)
        {
            return ((IItemOwner)_items).TryGetItem(index, out itemRef);
        }

        public void SetItem(int index, ParameterRef itemRef)
        {
            ((IItemOwner)_items).SetItem(index, itemRef);
        }

        public void ClearItem(int index)
        {
            ((IItemOwner)_items).ClearItem(index);
        }
        #endregion IItemOwner Members


        public override string ToString()
        {
            return $"ArrayParameter<{ElementType}>, Reference = {Reference}, IsNull = {IsNull}, Length = {Length}";
        }
    }

    public static partial class ParameterContextExtensions
    {
        public static IArrayParameter CreateArrayParameter(this IParameterSet set, TypeSignature elementType, bool? isNull = null, int? length = null)
        {
            return CreateArrayParameter(set, elementType, set.GetParameterRef(), isNull, length);
        }

        public static IArrayParameter CreateArrayParameter(this IParameterSet set, TypeSignature elementType, ParameterRef reference, bool? isNull = null, int? length = null)
        {
            ArrayParameter parameter = new ArrayParameter(set, elementType, reference)
            {
                IsNull = isNull,
                Length = length
            };

            set.Parameters.Add(parameter.Reference, parameter);

            return parameter;
        }
    }
}
