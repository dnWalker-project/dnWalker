using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public abstract class UserData
    {
        public string Id
        {
            get;
            set;
        }

        public abstract IValue Build(IModel model, TypeSig expectedType, IDictionary<string, IValue> references);

        protected void SetReference(IValue value, IDictionary<string, IValue> references)
        {
            string id = Id;
            if (!string.IsNullOrWhiteSpace(id))
            {
                references[id] = value;
            }
        }
    }
}
