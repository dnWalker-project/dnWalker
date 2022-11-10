using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal class AppModel
    {
        public IDomain Domain
        {
            get;
        }

        // 
        //public IConfigurationBuilder Configuration
        //{
        //    get;
        //}

        public void LoadAssembly(string assemblyFile)
        {

        }

        public void LoadConfiguration(string configurationFile)
        {

        }

        public void Explore(string typeName, string methodName)
        {

        }
    }
}
