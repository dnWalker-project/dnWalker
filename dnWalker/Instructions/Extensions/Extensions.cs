using dnWalker.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static partial class Extensions
    {
        private const string BaseNamespace = "dnWalker.Instructions.Extensions";

        private static readonly string[] _defualtExtensions = new string[]
        {
            // ordered from first to execute to last to execute
            // all of them are within the base assembly => we can ommit the assembly part
            BaseNamespace + "." + "Symbolic.BinaryBranch",
            BaseNamespace + "." + "Symbolic.BinaryOperation",
            BaseNamespace + "." + "Symbolic.CALLVIRT",
            BaseNamespace + "." + "Symbolic.CONV",
            BaseNamespace + "." + "Symbolic.CONV_OVF",
            BaseNamespace + "." + "Symbolic.DIV",
            BaseNamespace + "." + "Symbolic.LDARG",
            BaseNamespace + "." + "Symbolic.LDARGA",
            BaseNamespace + "." + "Symbolic.LDELEM+ExceptionsHandler",
            BaseNamespace + "." + "Symbolic.LDELEM+StateInitializer",
            BaseNamespace + "." + "Symbolic.LDFLD+NullReferenceHandler",
            BaseNamespace + "." + "Symbolic.LDFLD+StateInitializer",
            BaseNamespace + "." + "Symbolic.LDLEN+NullReferenceHandler",
            BaseNamespace + "." + "Symbolic.LDLEN+StateInitializer",
            BaseNamespace + "." + "Symbolic.REM",
            BaseNamespace + "." + "Symbolic.RET",
            BaseNamespace + "." + "Symbolic.STELEM+ExceptionsHandler",
            BaseNamespace + "." + "Symbolic.STELEM+ModelUpdater",
            BaseNamespace + "." + "Symbolic.STFLD+ModelUpdater",
            BaseNamespace + "." + "Symbolic.STFLD+NullReferenceHandler",
            BaseNamespace + "." + "Symbolic.UnaryBranch",
            BaseNamespace + "." + "Symbolic.UnaryOperation",
            
            //BaseNamespace + "." + "Symbolic.NativePeers.MethodCallNativePeers",
            BaseNamespace + "." + "NativePeers.MethodCallNativePeers",

        };

        public static ExtendableInstructionFactory AddExtensionsFrom(this ExtendableInstructionFactory factory, IConfiguration configuration)
        {
            foreach (IInstructionExecutor exec in CreateInstructionExtensions(configuration))
            { 
                factory.RegisterExtension(exec);
            }

            return factory;
        }

        public static IEnumerable<IInstructionExecutor> CreateInstructionExtensions(this IConfiguration configuration)
        {
            string[] extensionsIdentifiers = configuration.GetValue<string[]>("InstructionExtensions") ?? _defualtExtensions;


            return extensionsIdentifiers
                .Select(static id =>
                {
                    (string assemblyName, string typeName) = ExtensibilityPointHelper.FromTypeIdentifier(id);
                    return ExtensibilityPointHelper.Create<IInstructionExecutor>(assemblyName, typeName);
                });
        }
    }
}
