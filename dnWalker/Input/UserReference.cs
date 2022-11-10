using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class UserReference : UserData
    {
        public UserReference()
        {
        }

        public UserReference(string reference)
        {
            Reference = reference;
        }

        public override IValue Build(IModel model, TypeSig expectedType, IDictionary<string, IValue> references)
        {
            if (string.IsNullOrWhiteSpace(Reference))
            {
                throw new InvalidOperationException("The reference is not set.");
            }


            if (references.TryGetValue(Reference, out IValue value))
            {
                return value;
            }

            throw new Exception("Reference not found.");
        }

        public string Reference
        {
            get;
            set;
        }
    }
}
