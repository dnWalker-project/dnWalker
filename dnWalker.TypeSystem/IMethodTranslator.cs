using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IMethodTranslator
    {
        MethodSignature FromString(string text);
        string GetString(MethodSignature signature);
    }
}
