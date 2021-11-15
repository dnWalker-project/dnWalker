using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnWalker.Parameters;

namespace dnWalker.TestGenerator.Parameters
{
    public static partial class ParameterExtensions
    {
        public static void WriteInitialization(this Parameter parameter, CodeWriter codeWriter)
        {
            switch (parameter)
            {
                case BooleanParameter p: WriteInitialization(p, codeWriter); break;
                case CharParameter p: WriteInitialization(p, codeWriter); break;
                case ByteParameter p: WriteInitialization(p, codeWriter); break;
                case SByteParameter p: WriteInitialization(p, codeWriter); break;
                case Int16Parameter p: WriteInitialization(p, codeWriter); break;
                case Int32Parameter p: WriteInitialization(p, codeWriter); break;
                case Int64Parameter p: WriteInitialization(p, codeWriter); break;
                case UInt16Parameter p: WriteInitialization(p, codeWriter); break;
                case UInt32Parameter p: WriteInitialization(p, codeWriter); break;
                case UInt64Parameter p: WriteInitialization(p, codeWriter); break;
                case SingleParameter p: WriteInitialization(p, codeWriter); break;
                case DoubleParameter p: WriteInitialization(p, codeWriter); break;
                case ObjectParameter p: WriteInitialization(p, codeWriter); break;
                case InterfaceParameter p: WriteInitialization(p, codeWriter); break;
                case ArrayParameter p: WriteInitialization(p, codeWriter); break;
                default:
                    throw new NotSupportedException();
            }
        }

        public static void WriteInitialization(this ObjectParameter objectParameter, CodeWriter codeWriter)
        {
            // declare the variable & create the object using its constructor
            // TODO: find the best constructor based on the fields in use...
            codeWriter.WriteVariableDeclaration(objectParameter.TypeName, objectParameter.Name);

            using (IDisposable privateObjectScope = codeWriter.BeginCodeBlock())
            {
                // create the PrivateObject instance
                codeWriter.WriteVariableDeclaration("var", "privateObject", $"new PrivateObject({objectParameter.Name})");

                foreach (KeyValuePair<string, Parameter> field in objectParameter.GetKnownFields())
                {
                    //codeWriter.WriteVariableDeclaration(field.Value.FullTypeName, field.Key, $"default({field.Value.FullTypeName})");

                    using (IDisposable initializeBlockField = codeWriter.BeginCodeBlock())
                    {
                        field.Value.WriteInitialization(codeWriter);

                    }
                }

            }
        }

        private static void WritePrimitiveValueInitialization<T>(PrimitiveValueParameter<T> p, CodeWriter codeWriter, string? typeName = null) where T : struct
        {
            codeWriter.WriteVariableDeclaration(typeName ?? p.TypeName, p.Name, p.Value.HasValue ? p.Value.Value.ToString()! : default(T).ToString()!);
        }

        public static void WriteInitialization(this BooleanParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "bool");
        }
        public static void WriteInitialization(this CharParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "char");
        }
        public static void WriteInitialization(this ByteParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "byte");
        }
        public static void WriteInitialization(this SByteParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "sbyte");
        }
        public static void WriteInitialization(this Int16Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "short");
        }
        public static void WriteInitialization(this Int32Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "int");
        }
        public static void WriteInitialization(this Int64Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "long");
        }
        public static void WriteInitialization(this UInt16Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "ushort");
        }
        public static void WriteInitialization(this UInt32Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "uint");
        }
        public static void WriteInitialization(this UInt64Parameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "ulong");
        }
        public static void WriteInitialization(this SingleParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "single");
        }
        public static void WriteInitialization(this DoubleParameter p, CodeWriter codeWriter)
        {
            WritePrimitiveValueInitialization(p, codeWriter, "double");
        }
    }
}
