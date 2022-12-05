using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

namespace dnWalker.TypeSystem
{
    public static class DefinitionProviderExtensions
    {
        public static bool IsSubtype(this IDefinitionProvider definitionProvider, ITypeDefOrRef subType, ITypeDefOrRef superType)
        {
            SigComparer sigComparer = new SigComparer();

            TypeSig superSig = superType.ToTypeSig();

            foreach (ITypeDefOrRef tr in subType.InheritanceEnumerator())
            {
                if (sigComparer.Equals(tr.ToTypeSig(), superSig))
                {
                    return true;
                }
            }

            return false;
        }


        [Obsolete("The results of this method should be somehow cached in the definition provider. AND distinct static vs instance fields.")]
        public static int GetFieldOffset(IField field)
        {
            FieldDef fd = field.ResolveFieldDefThrow();
            if (!fd.FieldOffset.HasValue)
            {
                IList<FieldDef> fields = fd.DeclaringType.Fields;
                for (var i = 0; i < fields.Count; i++)
                {
                    if (fields[i] == fd)
                    {
                        fd.FieldOffset = (uint)i;
                        break;
                    }
                }
            }


            int typeOffset = 0;
            bool matched = false;
            int retval = 0;

            foreach (TypeDef typeDef in fd.DeclaringType.InheritanceEnumerator())
            {
                /*
				 * We start searching for the right field from the declaring type,
				 * it is possible that the declaring type does not define field, therefore
				 * it might be possible that we have to search further for field in
				 * the inheritance tree, (hence matched), and this continues until
				 * a field is found which has the same offset and the same name 
				 */
                if (TypeEqualityComparer.Instance.Equals(typeDef, fd.DeclaringType) || matched)
                {
                    if (fd.FieldOffset < typeDef.Fields.Count
                        && typeDef.Fields[(int)fd.FieldOffset].Name.Equals(fd.Name))
                    {
                        retval = (int)fd.FieldOffset;
                        break;
                    }

                    matched = true;
                }

                if (TypeEqualityComparer.Instance.Equals(typeDef.BaseType, typeDef.Module.CorLibTypes.Object)) 
                    // if base type is System.Object, stop
                {
                    typeOffset += Math.Max(0, typeDef.Fields.Count - 1);
                }
            }

            return typeOffset + retval;
        }

        public static TypeSig GetTypeSig(this IDefinitionProvider definitionProvider, TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Object => definitionProvider.BaseTypes.Object,
                TypeCode.Boolean => definitionProvider.BaseTypes.Boolean,
                TypeCode.Char => definitionProvider.BaseTypes.Char,
                TypeCode.SByte => definitionProvider.BaseTypes.SByte,
                TypeCode.Byte => definitionProvider.BaseTypes.Byte,
                TypeCode.Int16 => definitionProvider.BaseTypes.Int16,
                TypeCode.UInt16 => definitionProvider.BaseTypes.UInt16,
                TypeCode.Int32 => definitionProvider.BaseTypes.Int32,
                TypeCode.UInt32 => definitionProvider.BaseTypes.UInt32,
                TypeCode.Int64 => definitionProvider.BaseTypes.Int64,
                TypeCode.UInt64 => definitionProvider.BaseTypes.UInt64,
                TypeCode.Single => definitionProvider.BaseTypes.Single,
                TypeCode.Double => definitionProvider.BaseTypes.Double,
                TypeCode.String => definitionProvider.BaseTypes.String,
                _ => throw new NotSupportedException()
            };
        }

        public static TypeDef GetTypeDefinition(this IDefinitionProvider definitionProvider, string? ns, string name)
        {
            if (ns != null)
            {
                return definitionProvider.GetTypeDefinition(ns + "." + name);
            }
            else
            {
                int slashIdx = name.IndexOf('/');
                if (slashIdx > 0)
                {
                    name = name.Substring(slashIdx + 1);
                }

                return definitionProvider.Context.Modules
                    .SelectMany(md => md.GetTypes())
                    .Where(td => td.Name == name)
                    .SingleOrDefault() ?? throw new TypeNotFoundException(name);
            }
        }

        public static MethodDef GetMethodDefinitionPartial(this IDefinitionProvider definitionProvider, string methodSpecification)
        {
            string[] parts = methodSpecification.Split("::");

            Debug.Assert(parts.Length == 2);

            string typeNameOrFullName = parts[0];

            TypeDef td = FindType(definitionProvider, typeNameOrFullName);

            parts = parts[1].Split(new char[] { '(', ')' });
            string methodName = parts[0];

            MethodDef[] methods = td.FindMethods(methodName).ToArray();

            if (methods.Length == 1)
            {
                return methods[0];
            }

            if (parts.Length == 1) throw new MemberNotFoundException(td.Name, methodName);

            TypeSig[] argTypes = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(fullNameOrName => FindType(definitionProvider, fullNameOrName).ToTypeSig())
                .ToArray();

            return methods.FirstOrDefault(m => m.Parameters.Where(p => p.IsNormalMethodParameter).Select(p => p.Type).SequenceEqual(argTypes, TypeEqualityComparer.Instance)) ?? throw new MemberNotFoundException(td.Name, methodName);

            static TypeDef FindType(IDefinitionProvider definitionProvider, string fullNameOrName)
            {
                try
                {
                    TypeDef td = definitionProvider.GetTypeDefinition(fullNameOrName);
                    return td;
                }
                catch (TypeNotFoundException)
                {
                    // will throw new TypeNotFoundException should the no type be found
                    // or Linq exception if more than one type matches...
                    TypeDef td = definitionProvider.GetTypeDefinition(null, fullNameOrName);
                    return td;
                }
            }
        }

    }
}
