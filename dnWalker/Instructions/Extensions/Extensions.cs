using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static partial class Extensions
    {
        public static ExtendableInstructionFactory AddStandardExtensions(this ExtendableInstructionFactory factory)
        {
            factory.AddParameterHandlers();
            factory.AddSymbolicExecution();

            return factory;
        }

        private static IEnumerable<Type> GetExtensions(string ns)
        {
            return typeof(Extensions)
                .Assembly
                .GetTypes()
                .Where(t => 
                    t.Namespace == ns && 
                    !t.IsAbstract && 
                    t.IsAssignableTo(typeof(IInstructionExecutor)));
        }

        private static ExtendableInstructionFactory RegisterExtensionsFrom(this ExtendableInstructionFactory factory, string ns)
        {
            foreach(Type extensionType in GetExtensions(ns))
            {
                factory.RegisterExtension((IInstructionExecutor)Activator.CreateInstance(extensionType));
            }

            return factory;
        }
    }
}
