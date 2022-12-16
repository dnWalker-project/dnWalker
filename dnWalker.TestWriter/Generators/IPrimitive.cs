using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public interface IPrimitives
    {
        IEnumerable<string> Namespaces { get; }
    }
}
