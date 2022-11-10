using dnlib.DotNet;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IDefinitionProvider
    {
        IDomain Context { get; }

        TypeDef GetTypeDefinition(string fullTypeName);
        MethodDef GetMethodDefinition(string fullMethodName);

        int SizeOf(TypeSig type);

        IBaseTypes BaseTypes { get; }
    }
}
