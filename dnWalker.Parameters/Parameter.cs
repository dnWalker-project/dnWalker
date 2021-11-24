using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class Parameter
    {
        private readonly string _typeName;
        private readonly string _localName;
        private Parameter? _owner;

        public string LocalName
        {
            get { return _localName; }
        }

        public string GetFullName()
        {
            return _owner == null ? _localName : $"{_owner.GetFullName()}{ParameterNameUtils.Delimiter}{_localName}";
        }

        public Parameter? Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public abstract IEnumerable<Parameter> GetOwnedParameters();

        protected Parameter(string typeName, string localName) : this(typeName, localName, null)
        { }

        protected Parameter(string typeName, string localName, Parameter? owner)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            if (string.IsNullOrEmpty(localName))
            {
                throw new ArgumentNullException(nameof(localName));
            }

            _typeName = typeName;
            _localName = localName;
            _owner = owner;
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public override string ToString()
        {
            return $"Parameter: {LocalName}, Type: {TypeName}";
        }
    }
}
