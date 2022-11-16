using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;
using dnWalker.TestWriter.Generators.Schemas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    /// <summary>
    /// Just some kind of helper to load plugins and extensions. Should use some kind of safer technique!!!
    /// </summary>
    internal class ExtensionsHelper
    {
        static ExtensionsHelper()
        {
            // dirty, may be problematic!!!
            string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            foreach (string dll in Directory.GetFiles(execPath, "*.dll"))
            {
                Assembly.LoadFile(dll);
            }
        }

        public IEnumerable<T> GetExtensions<T>(Func<Type, bool>? filter = null)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                foreach(Type type in assembly.GetTypes()) 
                {
                    if (type.IsAssignableTo(typeof(T)) && !type.IsAbstract)
                    {
                        if (filter != null && filter(type))
                            continue;

                        yield return (T)Activator.CreateInstance(type)!;
                    }
                }
            }
        }

        public ITestTemplate CreateTestTemplate()
        {
            IArrangePrimitives[] arrangeWriters = GetExtensions<IArrangePrimitives>().ToArray();
            IActPrimitives[] actWriters = GetExtensions<IActPrimitives>().ToArray();
            IAssertPrimitives[] assertWriters = GetExtensions<IAssertPrimitives>().ToArray();

            return new TestTemplate(arrangeWriters, actWriters, assertWriters);
        }

        public ITestSchemaProvider CreateTestSchemaProvider()
        {
            ITestSchemaProvider[] testSchemaProviders= GetExtensions<ITestSchemaProvider>(t => t != typeof(MergedTestSchemaProvider)).ToArray();
            return new MergedTestSchemaProvider(testSchemaProviders);
        }

        public ITestFramework CreateTestFramework()
        {
            return GetExtensions<ITestFramework>().First();
        }
    }
}
