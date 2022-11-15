using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal interface IAppRunner
    {
        int Run(IAppModel appModel);
    }
}
