using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection
{
    public abstract class ReflectionParameter : IParameter
    {
        private readonly Type _type;
        private readonly int _id;
        private ParameterAccessor? _accessor;

        protected ReflectionParameter(Type type)
        { 
            _type = type;
            _id = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        protected ReflectionParameter(Type type, int id)
        {
            _type = type;
            _id = id;
        }

        string IParameter.TypeName
        {
            get { return _type.FullName!; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public int Id
        {
            get { return _id; }
        }

        public ParameterAccessor? Accessor
        {
            get { return _accessor; }
            set
            {
                if (_accessor != null)
                {
                    _accessor.ClearChild();
                }
                _accessor = value;
                if (_accessor != null)
                {
                    _accessor.OnChildSet(this);
                }
            }
        }
    }
}
