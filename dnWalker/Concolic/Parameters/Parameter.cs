
using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class Parameter
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

                if (_name != value)
                {
                    _name = value;
                    OnNameChanged(value);
                }
            }
        }

        public bool HasName()
        {
            return !String.IsNullOrWhiteSpace(_name);
        }

        public string TypeName { get; }


        protected virtual void OnNameChanged(string newName)
        {

        }

        protected Parameter(string typeName)
        {
            TypeName = typeName;
        }

        protected Parameter(string typeName, string name)
        {
            if (String.IsNullOrWhiteSpace(typeName)) throw new ArgumentNullException(nameof(typeName));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            TypeName = typeName;
            Name = name;
        }


        // TODO: make these methods as extension methods - so switch to shared parameter lib is easier
        public abstract IEnumerable<ParameterExpression> GetParameterExpressions();
        public abstract bool HasSingleExpression { get; }
        public abstract ParameterExpression GetSingleParameterExpression();

        public override string ToString()
        {
            return $"Parameter: {Name}, Type: {TypeName}";
        }

        public abstract bool TryGetChildParameter(string name, out Parameter childParameter);
    }
}
