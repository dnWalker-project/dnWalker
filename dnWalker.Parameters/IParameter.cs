using System;
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

        /// <summary>
        /// Shallow copy will create a new instance of the parameter with same children (using alias parameters)
        /// </summary>
        /// <returns></returns>
        IParameter ShallowCopy(int id = -1);

    }

    public static partial class ParameterExtensions
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
        public static string GetAccessString(this IParameter parameter)
        {
            if (parameter.Accessor == null) return string.Empty;

            string accessString = parameter.Accessor.GetAccessString();

            if (parameter.TryGetParent(out IParameter? parent))
            {
                accessString = parent.GetAccessString() + accessString;
            }

            return accessString;
        }
    }
}
