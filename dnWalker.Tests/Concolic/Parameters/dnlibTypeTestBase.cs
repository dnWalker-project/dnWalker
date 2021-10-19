using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Concolic.Parameters
{
    public abstract class dnlibTypeTestBase
    {
        private static readonly ModuleContext _context = ModuleDef.CreateModuleContext();
        private static readonly List<ModuleDef> _modules = new List<ModuleDef>();


        static dnlibTypeTestBase()
        {
            ModuleDef mainModule = ModuleDefMD.Load(typeof(dnlibTypeTestBase).Module, _context);

            _modules.Add(mainModule);

            _modules.AddRange(mainModule
                .GetAssemblyRefs()
                .Select(ar => _context.AssemblyResolver.Resolve(ar.Name, mainModule))
                .Where(a => a != null)
                .SelectMany(a => a.Modules));
        }

        public static TypeSig GetType(String typeName)
        {
            if (typeName.EndsWith("[]"))
            {
                return GetArrayType(typeName.Substring(0, typeName.Length - 2));
            }

            TypeSig type = _modules
                .SelectMany(m => m.Types)
                .FirstOrDefault(t => t.ReflectionFullName == typeName)
                .ToTypeSig();

            if (type == null) throw new Exception("Could not resolve type: " + typeName);

            return type;
        }

        public static TypeSig GetArrayType(String elementTypeName)
        {
            TypeSig elementType = GetType(elementTypeName);

            //ArraySig array = new ArraySig(elementType.ToTypeSig());
            SZArraySig array = new SZArraySig(elementType);

            if (array == null)
            {
                throw new Exception("ArraySig is a null!");
            }

            return array;
        }

        public static TypeSig GetType(Type type)
        {
            if (type.IsArray)
            {
                return GetArrayType(type.GetElementType());
            }

            return GetType(type.FullName);
        }

        public static TypeSig GetArrayType(Type elementType)
        {
            return GetArrayType(elementType.FullName);
        }

        public static void SomeMethod()
        {

        }
    }
}
