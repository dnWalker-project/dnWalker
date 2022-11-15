using System;
namespace dnWalker.Interface
{
    public interface IAppModel
    {
        bool LoadAssembly(string assemblyFile);
        bool LoadModels(string modelsFile);
        bool Explore(string method, string output);
    }
}

