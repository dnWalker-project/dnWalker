using MMC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public enum DataElementKind
    {
        ByRef,
        UInt64,
        Int64,
        Double,
        Type,
        Int32,
        UInt32,
        NativeInt,
        Single,
        String
    }

    public class ValueComparer
    {
        public static int CompareUnsigned(INumericElement v1, INumericElement v2)
        {
            if (v1.Equals(v2))
            {
                return 0;
            }

            return v1.ToUnsignedInt8(false).CompareTo(v2.ToUnsignedInt8(false));

            /*var res = debuggerRuntime.CompareUnsigned(v1, v2);
            if (!(res is null))
                return res.Value;

            var v1z = IsIntegerZeroOrNull(v1);
            var v2z = IsIntegerZeroOrNull(v2);
            if (v2z == ZeroResult.Zero)
            {
                if (v1z == ZeroResult.Zero)
                    return 0;
                if (v1z == ZeroResult.NonZero)
                    return 1;
            }
            if (v1z == ZeroResult.Zero)
            {
                if (v2z == ZeroResult.NonZero)
                    return -1;
            }*/
            /*switch (v1.Kind)
            {
            }*/
            /*

                case DataElementKind.Int32:
                    switch (v2.Kind)
                    {
                        case DataElementKind.Int32:
                            return ((Int8)v1).Value.CompareTo(((Int8)v2).Value);

                        case DataElementKind.NativeInt:
                            if (v2 is ConstantNativeIntILValue)
                            {
                                if (debuggerRuntime.PointerSize == 4)
                                    return ((ConstantInt32ILValue)v1).UnsignedValue.CompareTo((((ConstantNativeIntILValue)v2).UnsignedValue32));
                                return ((ulong)((ConstantInt32ILValue)v1).Value).CompareTo(((ConstantNativeIntILValue)v2).UnsignedValue64);
                            }
                            goto case DataElementKind.ByRef;

                        case DataElementKind.ByRef:
                        case DataElementKind.Int64:
                        case DataElementKind.Double:
                        case DataElementKind.Type:
                        default:
                            throw new InvalidMethodBodyInterpreterException();
                    }

                case DataElementKind.Int64:
                    switch (v2.Kind)
                    {
                        case DataElementKind.Int64:
                            return ((ulong)((ConstantInt64ILValue)v1).Value).CompareTo((ulong)((ConstantInt64ILValue)v2).Value);

                        case DataElementKind.Int32:
                        case DataElementKind.Double:
                        case DataElementKind.NativeInt:
                        case DataElementKind.ByRef:
                        case DataElementKind.Type:
                        default:
                            throw new InvalidMethodBodyInterpreterException();
                    }

                case DataElementKind.Double:
                    switch (v2.Kind)
                    {
                        case DataElementKind.Double:
                            return ((ConstantFloatILValue)v1).Value.CompareTo(((ConstantFloatILValue)v2).Value);

                        case DataElementKind.Int32:
                        case DataElementKind.Int64:
                        case DataElementKind.NativeInt:
                        case DataElementKind.ByRef:
                        case DataElementKind.Type:
                        default:
                            throw new InvalidMethodBodyInterpreterException();
                    }

                case DataElementKind.NativeInt:
                    switch (v2.Kind)
                    {
                        case DataElementKind.Int32:
                            if (v1 is ConstantNativeIntILValue)
                            {
                                if (debuggerRuntime.PointerSize == 4)
                                    return ((ConstantNativeIntILValue)v1).UnsignedValue32.CompareTo(((ConstantInt32ILValue)v2).UnsignedValue);
                                return ((ConstantNativeIntILValue)v1).UnsignedValue64.CompareTo(((ConstantInt32ILValue)v2).UnsignedValue);
                            }
                            goto case DataElementKind.ByRef;

                        case DataElementKind.NativeInt:
                            if (v1 == v2)
                                return 0;
                            if (v1 is ConstantNativeIntILValue && v2 is ConstantNativeIntILValue)
                            {
                                if (debuggerRuntime.PointerSize == 4)
                                    return ((ConstantNativeIntILValue)v1).UnsignedValue32.CompareTo(((ConstantNativeIntILValue)v2).UnsignedValue32);
                                return ((ConstantNativeIntILValue)v1).UnsignedValue64.CompareTo(((ConstantNativeIntILValue)v2).UnsignedValue64);
                            }
                            goto case DataElementKind.ByRef;

                        case DataElementKind.ByRef:
                        case DataElementKind.Int64:
                        case DataElementKind.Double:
                        case DataElementKind.Type:
                        default:
                            throw new InvalidMethodBodyInterpreterException();
                    }

                case DataElementKind.ByRef:
                case DataElementKind.Type:
                default:
                    throw new InvalidMethodBodyInterpreterException();
            }*/
            throw new NotImplementedException();
            }
    }
}
