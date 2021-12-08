﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameter
    {
        string TypeName { get; }
        int Id { get; }

        /// <summary>
        /// Gets, sets how is this <see cref="IParameter"/> accessable.
        /// </summary>
        ParameterAccessor? Accessor { get; set; }



        IEnumerable<IParameter> GetChildren();
    }

    public static class ParameterExtensions
    {
        public static bool IsRoot(this IParameter parameter)
        {
            return parameter.Accessor is RootParameterAccessor;
        }

        public static IEnumerable<IParameter> GetSelfAndDescendants(this IParameter parameter)
        {
            return parameter.GetChildren().SelectMany(p => p.GetSelfAndDescendants()).Append(parameter);
        }

        public static bool TryGetParent(this IParameter parameter, [NotNullWhen(true)] out IParameter? parent)
        {
            if (parameter.Accessor != null)
            {
                parent = parameter.Accessor.Parent;
                return parent != null;
            }

            parent = null;
            return false;
        }
    }
}
