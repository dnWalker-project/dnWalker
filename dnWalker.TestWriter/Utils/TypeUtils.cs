using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Utils
{
    public static class TypeUtils
    {
        public static string GetNameOrAlias(this TypeSig? type)
        {
            if (type == null) return "void";

            switch (type.GetElementType())
            {
                case ElementType.Void:
                    return "void";

                case ElementType.Boolean:
                    return "bool";

                case ElementType.Char:
                    return "char";

                case ElementType.I1:
                    return "sbyte";

                case ElementType.U1:
                    return "byte";

                case ElementType.I2:
                    return "short";

                case ElementType.U2:
                    return "ushort";

                case ElementType.I4:
                    return "int";

                case ElementType.U4:
                    return "uint";

                case ElementType.I8:
                    return "long";

                case ElementType.U8:
                    return "ulong";

                case ElementType.R4:
                    return "float";

                case ElementType.R8:
                    return "double";

                case ElementType.String:
                    return "string";

                case ElementType.I:
                    return "IntPtr";

                case ElementType.U:
                    return "UIntPtr";

                case ElementType.SZArray:
                case ElementType.Array:
                    return type.Next.GetNameOrAlias() + "[]";

                case ElementType.Class:
                case ElementType.Object:
                case ElementType.ValueType:
                    return GetName(type.ToClassOrValueTypeSig().TypeDefOrRef.ResolveTypeDefThrow());

                case ElementType.GenericInst:
                    {
                        GenericInstSig genInst = type.ToGenericInstSig();
                        // generic typename is finished by `XYZ, where XYZ is the gen arg count
                        return genInst.GenericType.GetNameOrAlias().Split('`')[0] + "<" + string.Join(", ", genInst.GenericArguments.Select(ga => ga.GetNameOrAlias())) + ">";
                    }

                case ElementType.Ptr:
                    return type.Next.GetNameOrAlias() + "*";

                case ElementType.ByRef:
                    return "ref " + type.Next.GetNameOrAlias();

                case ElementType.Var:
                case ElementType.MVar:
                    return type.TypeName;

            }
            throw new NotSupportedException($"Unsupported type: '{type}'");
        }

        public static string GetName(this ITypeDefOrRef typeDef)
        {
            StringBuilder sb = new StringBuilder();

            ITypeDefOrRef td = typeDef.DeclaringType;

            if (td != null)
            {
                sb.Append(td.GetName());
                sb.Append('.');
            }

            sb.Append(typeDef.Name);

            return sb.ToString();
        }

        public static string GetNamespace(this ITypeDefOrRef typeDef)
        {
            while (typeDef.DeclaringType != null) 
            {
                typeDef = typeDef.DeclaringType;
            }

            return typeDef.Namespace;
        }

    }
}
