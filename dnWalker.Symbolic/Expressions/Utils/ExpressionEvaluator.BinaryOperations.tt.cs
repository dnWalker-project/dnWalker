using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public partial class ExpressionEvaluator
    {
        private static class BinaryOperations
        {

            public static IValue Add(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value + r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value + (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value + r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue Subtract(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value - r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value - (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value - r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue Multiply(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value * r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value * (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value * r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue Divide(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value / r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value / (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value / r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue Modulo(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value % r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value % (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value % r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue Equal(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value == r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value == (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value == r.Value);
                
                    case (Location l, Location r):
                        return ValueFactory.GetValue(l == r);
                }
                throw new NotSupportedException();
            }

            public static IValue NotEqual(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value != r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value != (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value != r.Value);
                
                    case (Location l, Location r):
                        return ValueFactory.GetValue(l != r);
                }
                throw new NotSupportedException();
            }

            public static IValue GreaterThan(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue GreaterThanOrEqual(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value >= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value >= (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value >= r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue LessThan(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value < r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value < (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value < r.Value);
                
                }
                throw new NotSupportedException();
            }

            public static IValue LessThanOrEqual(IValue left, IValue right)
            {
                switch (left, right)
                {
                    case (PrimitiveValue<char> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (char)r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<char> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (byte)r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<byte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (ushort)r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ushort> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (uint)r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<uint> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue((char)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue((byte)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue((ushort)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue((uint)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue((sbyte)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue((short)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue((int)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue((long)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue((float)l.Value <= r.Value);
                
                    case (PrimitiveValue<ulong> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue((double)l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (sbyte)r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<sbyte> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (short)r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<short> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (int)r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<int> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (long)r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<long> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (float)r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<float> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<char>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<byte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ushort>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<uint>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<ulong>  r):
                        return ValueFactory.GetValue(l.Value <= (double)r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<sbyte>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<short>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<int>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<long>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<float>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                    case (PrimitiveValue<double> l, PrimitiveValue<double>  r):
                        return ValueFactory.GetValue(l.Value <= r.Value);
                
                }
                throw new NotSupportedException();
            }

        }
    }
}