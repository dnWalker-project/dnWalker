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
        private Parameter? _parent;

        private ParameterName? _fullName;

        public string LocalName
        {
            get { return _localName; }
        }

        public ParameterName FullName
        {
            get
            {
                //return _parent == null ? _localName : $"{_parent.GetFullName()}{ParameterNameUtils.Delimiter}{_localName}";
                if (!_fullName.HasValue)
                {
                    if (_parent == null)
                    {
                        // create a new root parameter name
                        _fullName = _localName;
                    }
                    else
                    {
                        _fullName = _parent.FullName.WithAccessor(_localName);
                    }
                }

                return _fullName.Value;
            }
        }

        public Parameter? Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public abstract IEnumerable<Parameter> GetChildren();

        protected Parameter(string typeName, string localName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"'{nameof(typeName)}' cannot be null or whitespace.", nameof(typeName));
            }

            if (string.IsNullOrWhiteSpace(localName))
            {
                throw new ArgumentException($"'{nameof(localName)}' cannot be null or whitespace.", nameof(localName));
            }

            _typeName = typeName;
            _localName = localName;
            _fullName = null;
        }

        protected Parameter(string typeName, string localName, Parameter owner)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            _typeName = typeName;
            _localName = localName;
            _parent = owner ?? throw new ArgumentNullException(nameof(owner));
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
