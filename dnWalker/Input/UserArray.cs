using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class UserArray : UserData
    {
        public int Length
        {
            get;
            set;
        }

        public IDictionary<int, UserData> Elements
        {
            get;
        } = new Dictionary<int, UserData>();

        public TypeSig ElementType
        {
            get;
            set;
        }

        public override IValue Build(IModel model, TypeSig expectedType, IDictionary<string, IValue> references)
        {
            TypeSig elementType = ElementType ?? expectedType.Next ?? throw new InvalidOperationException();

            // assert elementType is assignable to expectedType.Next!!!

            int length = Elements.Keys.Count > 0 ? Elements.Keys.Max() + 1 : 0;

            IArrayHeapNode arrayNode = model.HeapInfo.InitializeArray(elementType, length);
            SetReference(arrayNode.Location, references);

            foreach ((int index, UserData ud) in Elements)
            {
                arrayNode.SetElement(index, ud.Build(model, elementType, references));
            }

            return arrayNode.Location;
        }
    }
}
