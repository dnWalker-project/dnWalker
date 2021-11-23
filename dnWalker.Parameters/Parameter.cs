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

        //public IList<ParameterTrait> Traits { get; }

        /// <summary>
        /// Invoked when the parameter name changes. When overridden, updates names of child parameters.
        /// </summary>
        /// <param name="newName"></param>
        protected virtual void OnNameChanged(string newName)
        { }

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
            //Traits = new List<ParameterTrait>();
        }

        public abstract IEnumerable<ParameterExpression> GetParameterExpressions();
        public abstract bool HasSingleExpression { get; }
        public abstract ParameterExpression GetSingleParameterExpression();

        public override string ToString()
        {
            return $"Parameter: {Name}, Type: {TypeName}";
        }

        public abstract bool TryGetChildParameter(string name, out Parameter childParameter);

        public abstract IEnumerable<Parameter> GetChildrenParameters();
    }
}
