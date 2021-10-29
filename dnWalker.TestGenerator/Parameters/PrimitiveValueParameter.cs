using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Parameters
{
    public abstract class PrimitiveValueParameter<TValue> : Parameter 
        // where TValue : struct
    {
        protected PrimitiveValueParameter(string fullTypeName, string name, TValue value) : base(fullTypeName, name)
        {
            Value = value;
        }

        public TValue Value { get; }
    }

    public class BooleanParameter : PrimitiveValueParameter<bool>
    {
        public BooleanParameter(string name, bool value) : base("System.Boolean", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("bool", Name, Value.ToString());
        }
    }

    public class CharParameter : PrimitiveValueParameter<char>
    {
        public CharParameter(string name, char value) : base("System.Char", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("char", Name, Value.ToString());
        }
    }

    public class ByteParameter : PrimitiveValueParameter<byte>
    {
        public ByteParameter(string name, byte value) : base("System.Byte", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("byte", Name, Value.ToString());
        }
    }

    public class SByteParameter : PrimitiveValueParameter<sbyte>
    {
        public SByteParameter(string name, sbyte value) : base("System.SBbyte", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("sbyte", Name, Value.ToString());
        }
    }

    public class Int16Parameter : PrimitiveValueParameter<short>
    {
        public Int16Parameter(string name, short value) : base("System.Int16", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("short", Name, Value.ToString());
        }
    }

    public class Int32Parameter : PrimitiveValueParameter<int>
    {
        public Int32Parameter(string name, int value) : base("System.Int32", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("int", Name, Value.ToString());
        }
    }

    public class Int64Parameter : PrimitiveValueParameter<long>
    {
        public Int64Parameter(string name, long value) : base("System.Int64", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("long", Name, Value.ToString());
        }
    }

    public class UInt16Parameter : PrimitiveValueParameter<ushort>
    {
        public UInt16Parameter(string name, ushort value) : base("System.UInt16", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("ushort", Name, Value.ToString());
        }
    }

    public class UInt32Parameter : PrimitiveValueParameter<uint>
    {
        public UInt32Parameter(string name, uint value) : base("System.UInt32", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("uint", Name, Value.ToString());
        }
    }

    public class SBbyteParameter : PrimitiveValueParameter<ulong>
    {
        public SBbyteParameter(string name, ulong value) : base("System.SBbyte", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("ulong", Name, Value.ToString());
        }
    }

    public class SingleParameter : PrimitiveValueParameter<float>
    {
        public SingleParameter(string name, float value) : base("System.Single", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("float", Name, Value.ToString());
        }
    }

    public class DoubleParameter : PrimitiveValueParameter<double>
    {
        public DoubleParameter(string name, double value) : base("System.Double", name, value)
        {
        }

        public override void Initialize(CodeWriter codeWriter)
        {
            codeWriter.WriteVariableDeclaration("double", Name, Value.ToString());
        }
    }
}
