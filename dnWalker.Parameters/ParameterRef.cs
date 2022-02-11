using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public struct ParameterRef : IEquatable<ParameterRef>
    {
        private readonly int _id;

        public static readonly ParameterRef Empty = new ParameterRef(0);
        public static readonly ParameterRef[] EmptyArray = new ParameterRef[0];

        public ParameterRef(int id)
        {
            _id = id;
        }

        public IParameter? Resolve(IReadOnlyParameterSet context)
        {
            if (context.Parameters.TryGetValue(this, out IParameter? value))
            {
                return value;
            }
            return null;
        }

        public bool TryResolve(IReadOnlyParameterSet context, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (_id <= 0)
            {
                parameter = null;
                return false;
            }

            return context.Parameters.TryGetValue(this, out parameter);
        }

        public TParameter? Resolve<TParameter>(IReadOnlyParameterSet context)
            where TParameter : class, IParameter
        {
            if (context.Parameters.TryGetValue(this, out IParameter? value))
            {
                return value as TParameter;
            }
            return null;
        }

        public bool TryResolve<TParameter>(IReadOnlyParameterSet context, [NotNullWhen(true)]out TParameter? parameter)
            where TParameter : class, IParameter
        {
            context.Parameters.TryGetValue(this, out IParameter? p);
            parameter = p as TParameter;
            return parameter != null;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParameterRef reference && Equals(reference);
        }

        public bool Equals(ParameterRef other)
        {
            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }

        public static bool operator ==(ParameterRef left, ParameterRef right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ParameterRef left, ParameterRef right)
        {
            return !(left == right);
        }

        public static implicit operator int(ParameterRef parameterReference)
        {
            return parameterReference._id;
        }

        public static implicit operator ParameterRef(int id)
        {
            return new ParameterRef(id);
        }

        public override string ToString()
        {
            return $"0x{_id:X8}";
        }
    }
}
