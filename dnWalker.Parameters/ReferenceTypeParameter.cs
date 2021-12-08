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
    public abstract class ReferenceTypeParameter : Parameter, IReferenceTypeParameter
    {
        protected ReferenceTypeParameter(string typeName) : base(typeName)
        {
        }

        protected ReferenceTypeParameter(string typeName, int id) : base(typeName, id)
        {
        }

        public bool IsNull
        {
            get;
            set;
        } = true;

        private readonly HashSet<int> _refs = new HashSet<int>();

        public bool ReferenceEquals(IReferenceTypeParameter? other)
        {
            return other != null && (_refs.Contains(other.Id));
        }

        public void SetReferenceEquals(IReferenceTypeParameter other, bool value = true)
        {
            if (other == null)  throw new ArgumentNullException(nameof(other));

            if (!value)
            {
                ClearReferenceEquals(other);
                return;
            }

            if (_refs.Add(other.Id))
            {
                // we added the id
                other.SetReferenceEquals(this, value);
            }
        }

        public void ClearReferenceEquals(IReferenceTypeParameter other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (_refs.Remove(other.Id))
            {
                other.ClearReferenceEquals(this);
            }
        }
    }
}
