using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

using ReflectionOpCodes = System.Reflection.Emit.OpCodes;
using ReflectionTypeAttributes = System.Reflection.TypeAttributes;
using ReflectionMethodAttributes = System.Reflection.MethodAttributes;

using dnlibOpCodes = dnlib.DotNet.Emit.OpCodes;
using dnlibMethodImplAttributes = dnlib.DotNet.MethodImplAttributes;
using dnlibMethodAttributes = dnlib.DotNet.MethodAttributes;
using System.IO;

namespace dnWalker.Tests.Dynamic
{
    public class DynamicTypeGenerationTests
    {
        private Type GenerateTypeWith_Return5_Method_Using_Reflection()
        {
            const String AssemblyName = "Reflection_MyAssembly";
            const String ModuleName = "Reflection_MyModule";
            const String NamespaceName = "Reflection_MyNamespace";
            const String TypeName = "Reflection_MyType";
            const String FullTypeName = NamespaceName + "." + TypeName;
            const String MethodName = "Return5";

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.Run);
            
            
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(FullTypeName, ReflectionTypeAttributes.Public | ReflectionTypeAttributes.Class, typeof(Object));

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(MethodName, ReflectionMethodAttributes.Public, typeof(Int32), Type.EmptyTypes);

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(ReflectionOpCodes.Nop);
            ilGenerator.Emit(ReflectionOpCodes.Ldc_I4, 5);
            ilGenerator.Emit(ReflectionOpCodes.Ret);

            return typeBuilder.CreateType();
        }

        private Type GenerateTypeWith_Return5_Method_Using_dnlib()
        {
            ModuleDefMD baseModule = ModuleDefMD.Load(typeof(DynamicTypeGenerationTests).Module);

            const String AssemblyName = "dnlib_MyAssembly";
            const String ModuleName = "dnlib_MyModule";
            const String NamespaceName = "dnlib_MyNamespace";
            const String TypeName = "dnlib_MyType";
            const String FullTypeName = NamespaceName + "." + TypeName;
            const String MethodName = "Return5";


            AssemblyDefUser assembly = new AssemblyDefUser(AssemblyName);
            ModuleDefUser module = new ModuleDefUser(ModuleName);
            assembly.Modules.Add(module);

            TypeDefUser type = new TypeDefUser(NamespaceName, TypeName);
            module.AddAsNonNestedType(type);

            dnlibMethodImplAttributes method_Return5_implFlags = dnlibMethodImplAttributes.IL | dnlibMethodImplAttributes.Managed;
            dnlibMethodAttributes method_Return5_flags = dnlibMethodAttributes.Public | dnlibMethodAttributes.HideBySig | dnlibMethodAttributes.ReuseSlot;

            MethodDefUser method_Return5 = new MethodDefUser
                (
                    name: MethodName,
                    MethodSig.CreateInstance(baseModule.CorLibTypes.Int32),
                    method_Return5_implFlags, method_Return5_flags
                );

            type.Methods.Add(method_Return5);

            CilBody body = new CilBody();
            method_Return5.Body = body;

            body.Instructions.Add(dnlibOpCodes.Nop.ToInstruction());
            body.Instructions.Add(dnlibOpCodes.Ldc_I4.ToInstruction(5));
            body.Instructions.Add(dnlibOpCodes.Ret.ToInstruction());


            using (MemoryStream stream = new MemoryStream())
            {
                module.Write(stream);

                stream.Position = 0;

                Assembly reflectionAssembly = AppDomain.CurrentDomain.Load(stream.ToArray());

                try
                {
                    Type[] reflectionTypes = reflectionAssembly.GetTypes();

                    Type activatorType = reflectionAssembly.GetType(type.ReflectionFullName);

                    return activatorType;
                }
                catch(Exception e)
                {
                    throw;
                }
            }

        }

        private TypeDef AsTypeDef(this Type type, ModuleContext context)
        {
            ModuleDefMD module = ModuleDefMD.Load(type.Module, context);
            TypeDef dnlibType = module.Assembly.Find(type.FullName, true);
            return dnlibType;
        }

        [Fact]
        public void Test_Reflection_GeneratedType()
        {
            Type generatedType = GenerateTypeWith_Return5_Method_Using_Reflection();
            Object o = Activator.CreateInstance(generatedType);

            MethodInfo generatedMethod = generatedType.GetMethod("Return5");
            Int32 retVal = (Int32) generatedMethod.Invoke(o, Array.Empty<Object>());

            retVal.Should().Be(5);
        }

        [Fact]
        public void Test_dnlib_GeneratedType()
        {
            Type generatedType = GenerateTypeWith_Return5_Method_Using_dnlib();
            Object o = Activator.CreateInstance(generatedType);

            MethodInfo generatedMethod = generatedType.GetMethod("Return5");
            Int32 retVal = (Int32)generatedMethod.Invoke(o, Array.Empty<Object>());


            retVal.Should().Be(5);
        }
    }
}
