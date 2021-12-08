using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Tests.Xml
{
    public static class SerializationTestDataProvider
    {
        private const string ObjectParameter_IsNull = "<Object Type=\"MyNamespace.MyClass\" Id=\"5\" IsNull=\"true\" />";
        private const string ObjectParameter_NotNull_NoFields = "<Object Type=\"MyNamespace.MyClass\" Id=\"5\" IsNull=\"false\" />";
        private const string ObjectParameter_NotNull_PrimitiveFields_ComplexField = "<Object Type=\"MyNamespace.MyClass\" Id=\"5\" IsNull=\"false\">\r\n  <Field Name=\"My_Field\">\r\n    <PrimitiveValue Type=\"System.Int32\" Id=\"6\" Value=\"-5\" />\r\n  </Field>\r\n  <Field Name=\"My_ComplexField\">\r\n    <Object Type=\"MyNamespace.AnotherClass\" Id=\"7\" IsNull=\"false\" />\r\n  </Field>\r\n</Object>";
        private const string ObjectParameter_NotNull_PrimitiveFields = "<Object Type=\"MyNamespace.MyClass\" Id=\"5\" IsNull=\"false\">\r\n  <Field Name=\"My_Field\">\r\n    <PrimitiveValue Type=\"System.Int32\" Id=\"6\" Value=\"-5\" />\r\n  </Field>\r\n</Object>";
        private const string PrimitiveValueFormat = "<PrimitiveValue Type=\"{0}\" Id=\"{1}\" Value=\"{2}\" />";

        public class PrimitiveValueParameterProvider : IEnumerable<object[]>
        {
            public static IEnumerable<object[]> GeneratePrimitiveValueParameters()
            {
                static string GenerateExpectedXml(string fulltypename, int id, object value)
                {
                    return string.Format(PrimitiveValueFormat, fulltypename, id, value.ToString());
                }

                IParameter parameter;

                // Boolean Parameters => TRUE/FALSE
                parameter = new BooleanParameter(true);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "true") };

                parameter = new BooleanParameter(false);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "false") };

                // Char parameters => normal symbol / control symbol / space - TODO: generate from some big subset of chars?
                parameter = new CharParameter('a');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "U+0061") };

                parameter = new CharParameter('\n');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "U+000A") };

                parameter = new CharParameter(' ');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "U+0020") };

                // Byte Parameters => 1/MinValue/MaxValue
                parameter = new ByteParameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new ByteParameter(byte.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, byte.MinValue) };

                parameter = new ByteParameter(byte.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, byte.MaxValue) };

                // SByte Parameters => +1/-1/MinValue/MaxValue
                parameter = new SByteParameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new SByteParameter(-1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1) };

                parameter = new SByteParameter(sbyte.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, sbyte.MinValue) };

                parameter = new SByteParameter(sbyte.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, sbyte.MaxValue) };

                // Int16 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int16Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new Int16Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new Int16Parameter(-1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1) };

                parameter = new Int16Parameter(short.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, short.MinValue) };

                parameter = new Int16Parameter(short.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, short.MaxValue) };

                // Int32 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int32Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new Int32Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new Int32Parameter(-1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1) };

                parameter = new Int32Parameter(int.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, int.MinValue) };

                parameter = new Int32Parameter(int.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, int.MaxValue) };

                // Int64 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int64Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new Int64Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new Int64Parameter(-1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1) };

                parameter = new Int64Parameter(long.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, long.MinValue) };

                parameter = new Int64Parameter(long.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, long.MaxValue) };

                // UInt16 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt16Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new UInt16Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new UInt16Parameter(ushort.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, ushort.MinValue) };

                parameter = new UInt16Parameter(ushort.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, ushort.MaxValue) };

                // UInt32 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt32Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new UInt32Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new UInt32Parameter(uint.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, uint.MinValue) };

                parameter = new UInt32Parameter(uint.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, uint.MaxValue) };

                // UInt64 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt64Parameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new UInt64Parameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new UInt64Parameter(ulong.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, ulong.MinValue) };

                parameter = new UInt64Parameter(ulong.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, ulong.MaxValue) };


                // Single Parameters => +1/-1/MinValue/MaxValue
                parameter = new SingleParameter(-1.0f);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1.0f) };

                parameter = new SingleParameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new SingleParameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new SingleParameter(float.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, float.MinValue) };

                parameter = new SingleParameter(float.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, float.MaxValue) };

                parameter = new SingleParameter(float.NaN);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "NAN") };

                parameter = new SingleParameter(float.PositiveInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "INF") };

                parameter = new SingleParameter(float.NegativeInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "-INF") };

                // Double Parameters => +1/-1/MinValue/MaxValue
                parameter = new DoubleParameter(-1.0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, -1.0) };

                parameter = new DoubleParameter(1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 1) };

                parameter = new DoubleParameter(0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, 0) };

                parameter = new DoubleParameter(double.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, double.MinValue) };

                parameter = new DoubleParameter(double.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, double.MaxValue) };

                parameter = new DoubleParameter(double.NaN);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "NAN") };

                parameter = new DoubleParameter(double.PositiveInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "INF") };

                parameter = new DoubleParameter(double.NegativeInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.Id, "-INF") };
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return GeneratePrimitiveValueParameters().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GeneratePrimitiveValueParameters().GetEnumerator();
            }
        }
        public class ObjectParameterProvider : IEnumerable<object[]>
        {
            public static IEnumerable<object[]> GenerateObjectParameters()
            {
                const string Type = "MyNamespace.MyClass";

                IObjectParameter parameter;

                // IsNull == true && no fields
                {
                    parameter = new ObjectParameter(Type, 5) { IsNull = true };
                    yield return new object[] { parameter, ObjectParameter_IsNull };
                }


                // IsNull == false && no fields
                {
                    parameter = new ObjectParameter(Type, 5) { IsNull = false };
                    yield return new object[] { parameter, ObjectParameter_NotNull_NoFields };
                }

                // IsNull == false && some fields
                {
                    parameter = new ObjectParameter(Type, 5) { IsNull = false };
                    parameter.SetField("My_Field", new Int32Parameter(-5, 6));
                    yield return new object[] { parameter, ObjectParameter_NotNull_PrimitiveFields };
                }

                // IsNull == false && primitive field && complex field
                {
                    parameter = new ObjectParameter(Type, 5) { IsNull = false };
                    parameter.SetField("My_Field", new Int32Parameter(-5, 6));

                    ObjectParameter complexFieldValue = new ObjectParameter("MyNamespace.AnotherClass", 7) { IsNull = false };

                    parameter.SetField("My_ComplexField", complexFieldValue);
                    yield return new object[] { parameter, ObjectParameter_NotNull_PrimitiveFields_ComplexField };
                }
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return GenerateObjectParameters().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GenerateObjectParameters().GetEnumerator();
            }
        }



    }
}
