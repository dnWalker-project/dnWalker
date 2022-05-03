using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public static class TypeUtils
    {
        public static bool InitLayout(this TypeDef typeDef)
        {
            IList<FieldDef> fields = typeDef.Fields;

            if (fields.Count == 0 || fields[0].FieldOffset.HasValue) return false; // initialized

            InitLayoutRecursive(typeDef);

            return true;
        }

        private static uint InitLayoutRecursive(TypeDef typeDef)
        {
            if (typeDef.FullName == "System.Object") return 0;

            uint? fOffset = typeDef.Fields[0].FieldOffset;
            if (fOffset.HasValue)
            {
                // already initialized => skip it & reuse
                return (uint)(fOffset.Value + typeDef.Fields.Count);
            }

            uint offset = InitLayoutRecursive(typeDef.GetBaseTypeThrow().ResolveTypeDefThrow());

            IList<FieldDef> fields = typeDef.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].FieldOffset = offset++;
            }
            return offset;
        }

        public static TypeCode GetTypeCode(this TypeSig type)
        {
            if (type.IsObject()) return TypeCode.Object;
            else if (type.IsBoolean()) return TypeCode.Boolean;
            else if (type.IsByte()) return TypeCode.Byte;
            else if (type.IsSByte()) return TypeCode.SByte;
            else if (type.IsUInt16()) return TypeCode.UInt16;
            else if (type.IsInt16()) return TypeCode.Int16;
            else if (type.IsUInt32()) return TypeCode.UInt32;
            else if (type.IsInt32()) return TypeCode.Int32;
            else if (type.IsUInt64()) return TypeCode.UInt64;
            else if (type.IsInt64()) return TypeCode.Int64;
            else if (type.IsSingle()) return TypeCode.Single;
            else if (type.IsDouble()) return TypeCode.Double;
            else if (type.IsString()) return TypeCode.String;
            else if (type.IsChar()) return TypeCode.Char;

            return TypeCode.Empty;
        }
    }
}
