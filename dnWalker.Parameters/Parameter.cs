using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class Parameter : IParameter, IEquatable<IParameter?>
    {
        private readonly int _id;
        private readonly string _typeName;
        private ParameterAccessor? _accessor;

        protected Parameter(string typeName)
        {
            _typeName = typeName;
            _id = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        protected Parameter(string typeName, int id)
        {
            _typeName = typeName;
            _id = id < 0 ? System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this) : id;
        }

        public string TypeName
        {
            get { return _typeName; }
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
                _accessor = value;
            }
        }
        //public bool IsRoot
        //{
        //    get { return ((IParameter)this).IsRoot; }
        //}

        //public bool TryGetParent([NotNullWhen(true)] out IParameter? parent)
        //{
        //    return ((IParameter)this).TryGetParent(out parent);
        //}

        //public IEnumerable<IParameter> GetSelfAndDescendants()
        //{
        //    return((IParameter)this).GetSelfAndDescendants();
        //}

        public abstract IEnumerable<IParameter> GetChildren();


        public override bool Equals(object? obj)
        {
            return Equals(obj as IParameter);
        }

        public bool Equals(IParameter? other)
        {
            return other != null &&
                   _id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }

        public abstract IParameter ShallowCopy(ParameterStore store, int id);
    }
}
