﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    /// <summary>
    /// Represents a set of parameters. Provides resolution from <see cref="ParameterRef"/> to <see cref="IParameter"/>.
    /// </summary>
    public interface IParameterContext
    {
        IDictionary<ParameterRef, IParameter> Parameters { get; }

        IDictionary<string, ParameterRef> Roots { get; }

        /// <summary>
        /// Creates a new parameter reference based on the parameter context strategy.
        /// </summary>
        /// <returns></returns>
        ParameterRef GetParameterRef();
    }

    public static partial class ParameterContextExtensions
    {
        public static bool Remove(this IParameterContext context, ParameterRef reference)
        {
            if (reference.TryResolve(context, out IParameter? parameter))
            {
                context.Parameters.Remove(reference);
                if (parameter.Accessor is RootParameterAccessor r)
                {
                    context.Roots.Remove(r.Expression);
                }
                return true;
            }

            return false;
        }
    }
}
