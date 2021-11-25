using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            _isNullParameter = new BooleanParameter(IsNullName, true, this);
        }

        protected ReferenceTypeParameter(string typeName, string localName, Parameter parent) : base(typeName, localName, parent)
        {
            _isNullParameter = new BooleanParameter(IsNullName, true, this);
        }

        private readonly BooleanParameter _isNullParameter;

        public BooleanParameter IsNullParameter
        {
            get { return _isNullParameter; }
        }

        public bool IsNull
        {
            get { return _isNullParameter.Value; }
            set { _isNullParameter.Value = value; }
        }

        public override bool TryGetChild(ParameterName parameterName, [NotNullWhen(true)] out Parameter? parameter)
        {
            if (parameterName.TryGetField(out string? field) && field == IsNullName)
            {
                parameter = IsNullParameter;
                return true;
            }

            parameter = null;
            return false;
        }
    }
}
