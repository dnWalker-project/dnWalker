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
        private const string ObjectParameter_IsNull = "<Object Type=\"MyNamespace.MyClass\" Name=\"My_Namespace_My_Class\" IsNull=\"true\" />";
        private const string ObjectParameter_NotNull_NoFields = "<Object Type=\"MyNamespace.MyClass\" Name=\"My_Namespace_My_Class\" IsNull=\"false\" />";
        private const string ObjectParameter_NotNull_PrimitiveFields_ComplexField = "<Object Type=\"MyNamespace.MyClass\" Name=\"My_Namespace_My_Class\" IsNull=\"false\">\r\n  <Field Name=\"My_Field\">\r\n    <PrimitiveValue Type=\"System.Int32\" Name=\"My_Field\">-5</PrimitiveValue>\r\n  </Field>\r\n  <Field Name=\"My_ComplexField\">\r\n    <Object Type=\"MyNamespace.AnotherClass\" Name=\"My_ComplexField\" IsNull=\"false\" />\r\n  </Field>\r\n</Object>";
        private const string ObjectParameter_NotNull_PrimitiveFields = "<Object Type=\"MyNamespace.MyClass\" Name=\"My_Namespace_My_Class\" IsNull=\"false\">\r\n  <Field Name=\"My_Field\">\r\n    <PrimitiveValue Type=\"System.Int32\" Name=\"My_Field\">-5</PrimitiveValue>\r\n  </Field>\r\n</Object>";
        private const string PrimitiveValueFormat = "<PrimitiveValue Type=\"{0}\" Name=\"{1}\">{2}</PrimitiveValue>";

        public class PrimitiveValueParameterProvider : IEnumerable<object[]>
        {
            public static IEnumerable<object[]> GeneratePrimitiveValueParameters()
            {
                string GenerateExpectedXml(string fulltypename, string paramName, object value)
                {
                    return string.Format(PrimitiveValueFormat, fulltypename, paramName, value.ToString());
                }

                Parameter parameter;

                // Boolean Parameters => TRUE/FALSE
                parameter = new BooleanParameter("My:Boolean:Parameter", true);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "true") };

                parameter = new BooleanParameter("My:Boolean:Parameter", false);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "false") };

                // Char parameters => normal symbol / control symbol / space - TODO: generate from some big subset of chars?
                parameter = new CharParameter("My:Char:Parameter", 'a');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "U+0061") };

                parameter = new CharParameter("My:Char:Parameter", '\n');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "U+000A") };

                parameter = new CharParameter("My:Char:Parameter", ' ');
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "U+0020") };

                // Byte Parameters => 1/MinValue/MaxValue
                parameter = new ByteParameter("My:Byte:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new ByteParameter("My:Byte:Parameter", byte.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, byte.MinValue) };

                parameter = new ByteParameter("My:Byte:Parameter", byte.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, byte.MaxValue) };

                // SByte Parameters => +1/-1/MinValue/MaxValue
                parameter = new SByteParameter("My:SByte:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new SByteParameter("My:SByte:Parameter", -1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1) };

                parameter = new SByteParameter("My:SByte:Parameter", sbyte.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, sbyte.MinValue) };

                parameter = new SByteParameter("My:SByte:Parameter", sbyte.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, sbyte.MaxValue) };

                // Int16 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int16Parameter("My:Int16:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new Int16Parameter("My:Int16:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new Int16Parameter("My:Int16:Parameter", -1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1) };

                parameter = new Int16Parameter("My:Int16:Parameter", short.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, short.MinValue) };

                parameter = new Int16Parameter("My:Int16:Parameter", short.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, short.MaxValue) };

                // Int32 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int32Parameter("My:Int32:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new Int32Parameter("My:Int32:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new Int32Parameter("My:Int32:Parameter", -1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1) };

                parameter = new Int32Parameter("My:Int32:Parameter", int.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, int.MinValue) };

                parameter = new Int32Parameter("My:Int32:Parameter", int.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, int.MaxValue) };

                // Int64 Parameters => +1/-1/MinValue/MaxValue
                parameter = new Int64Parameter("My:Int64:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new Int64Parameter("My:Int64:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new Int64Parameter("My:Int64:Parameter", -1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1) };

                parameter = new Int64Parameter("My:Int64:Parameter", long.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, long.MinValue) };

                parameter = new Int64Parameter("My:Int64:Parameter", long.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, long.MaxValue) };

                // UInt16 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt16Parameter("My:UInt16:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new UInt16Parameter("My:UInt16:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new UInt16Parameter("My:UInt16:Parameter", ushort.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, ushort.MinValue) };

                parameter = new UInt16Parameter("My:UInt16:Parameter", ushort.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, ushort.MaxValue) };

                // UInt32 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt32Parameter("My:UInt32:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new UInt32Parameter("My:UInt32:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new UInt32Parameter("My:UInt32:Parameter", uint.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, uint.MinValue) };

                parameter = new UInt32Parameter("My:UInt32:Parameter", uint.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, uint.MaxValue) };

                // UInt64 Parameters => +1/-1/MinValue/MaxValue
                parameter = new UInt64Parameter("My:UInt64:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new UInt64Parameter("My:UInt64:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new UInt64Parameter("My:UInt64:Parameter", ulong.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, ulong.MinValue) };

                parameter = new UInt64Parameter("My:UInt64:Parameter", ulong.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, ulong.MaxValue) };


                // Single Parameters => +1/-1/MinValue/MaxValue
                parameter = new SingleParameter("My:Single:Parameter", -1.0f);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1.0f) };

                parameter = new SingleParameter("My:Single:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new SingleParameter("My:Single:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new SingleParameter("My:Single:Parameter", float.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, float.MinValue) };

                parameter = new SingleParameter("My:Single:Parameter", float.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, float.MaxValue) };

                parameter = new SingleParameter("My:Single:Parameter", float.NaN);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "NAN") };

                parameter = new SingleParameter("My:Single:Parameter", float.PositiveInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "INF") };

                parameter = new SingleParameter("My:Single:Parameter", float.NegativeInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "-INF") };

                // Double Parameters => +1/-1/MinValue/MaxValue
                parameter = new DoubleParameter("My:Double:Parameter", -1.0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, -1.0) };

                parameter = new DoubleParameter("My:Double:Parameter", 1);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 1) };

                parameter = new DoubleParameter("My:Double:Parameter", 0);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, 0) };

                parameter = new DoubleParameter("My:Double:Parameter", double.MinValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, double.MinValue) };

                parameter = new DoubleParameter("My:Double:Parameter", double.MaxValue);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, double.MaxValue) };

                parameter = new DoubleParameter("My:Double:Parameter", double.NaN);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "NAN") };

                parameter = new DoubleParameter("My:Double:Parameter", double.PositiveInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "INF") };

                parameter = new DoubleParameter("My:Double:Parameter", double.NegativeInfinity);
                yield return new object[] { parameter, GenerateExpectedXml(parameter.TypeName, parameter.LocalName, "-INF") };
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
                const string Name = "My_Namespace_My_Class";

                ObjectParameter parameter;

                // IsNull == true && no fields
                {
                    parameter = new ObjectParameter(Type, Name) { IsNull = true };
                    yield return new object[] { parameter, ObjectParameter_IsNull };
                }

                // IsNull == true && some fields
                {
                    parameter = new ObjectParameter(Type, Name) { IsNull = true };
                    parameter.SetField("My_Field", new Int32Parameter("My_Field", -5 ));
                    yield return new object[] { parameter, ObjectParameter_IsNull };
                }

                // IsNull == false && no fields
                {
                    parameter = new ObjectParameter(Type, Name) { IsNull = false };
                    yield return new object[] { parameter, ObjectParameter_NotNull_NoFields };
                }

                // IsNull == false && some fields
                {
                    parameter = new ObjectParameter(Type, Name) { IsNull = false };
                    parameter.SetField("My_Field", new Int32Parameter("My_Field", -5 ));
                    yield return new object[] { parameter, ObjectParameter_NotNull_PrimitiveFields };
                }

                // IsNull == false && primitive field && complex field
                {
                    parameter = new ObjectParameter(Type, Name) { IsNull = false };
                    parameter.SetField("My_Field", new Int32Parameter("My_Field", -5));

                    ObjectParameter complexFieldValue = new ObjectParameter("MyNamespace.AnotherClass", "My_ComplexField") { IsNull = false };

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
