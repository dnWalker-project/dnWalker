using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.ChoiceGenerators
{
    public interface IChoiceGenerator
    {
        object GetNextChoice();

        bool HasMoreChoices { get; }

        Stack<int> GetErrorTrace();
    }
}
