using System;
using System.Collections.Generic;
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
    }
}
