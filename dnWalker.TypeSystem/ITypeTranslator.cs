using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface ITypeTranslator
    {
        TypeSignature FromString(string text);
        string GetString(TypeSignature signature);
    }
}
