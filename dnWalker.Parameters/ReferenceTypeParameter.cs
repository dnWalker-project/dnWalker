using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter
    {
        public static readonly string IsNullName = "#__IsNUll__";

        protected ReferenceTypeParameter(string typeName, string localName) : base(typeName, localName)
        {
            _isNullParameter = new BooleanParameter(IsNullName, this);
        }

        protected ReferenceTypeParameter(string typeName, string localName, Parameter? owner) : base(typeName, localName, owner)
        {
            _isNullParameter = new BooleanParameter(IsNullName, this);
        }

        private readonly BooleanParameter _isNullParameter;

        public override IEnumerable<Parameter> GetOwnedParameters()
        {
            yield return _isNullParameter;
        }

        public BooleanParameter IsNullParameter
        {
            get { return _isNullParameter; }
        }

        public bool IsNull
        {
            get { return _isNullParameter.Value ?? true; }
            set { _isNullParameter.Value = value; }
        }
    }
}
