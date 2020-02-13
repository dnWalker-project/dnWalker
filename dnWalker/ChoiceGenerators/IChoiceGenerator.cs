using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.ChoiceGenerators
{
    public interface IChoiceGenerator
    {
        int Next();
        Stack<int> GetErrorTrace();
    }
}
