using dnWalker.Concolic;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Z3.LinqBinding;

namespace dnWalker.Z3
{
    public class Solver : ISolver
    {
        private class Z3TypeTranslator<TInt, TReal, TBool> : ExpressionVisitor
            where TInt : struct, IConvertible
            where TReal : struct, IConvertible
            where TBool : struct, IConvertible
        {
            private static readonly Type IntType = typeof(TInt);
            private static readonly Type RealType = typeof(TReal);
            private static readonly Type BoolType = typeof(TBool);

            private static readonly TInt CharMin = (TInt)Convert.ChangeType(Char.MinValue, IntType);
            private static readonly TInt CharMax = (TInt)Convert.ChangeType(Char.MaxValue, IntType);

            private static readonly TInt SByteMin = (TInt)Convert.ChangeType(SByte.MinValue, IntType);
            private static readonly TInt SByteMax = (TInt)Convert.ChangeType(SByte.MaxValue, IntType);

            private static readonly TInt ByteMin = (TInt)Convert.ChangeType(Byte.MinValue, IntType);
            private static readonly TInt ByteMax = (TInt)Convert.ChangeType(Byte.MaxValue, IntType);

            private static readonly TInt Int16Min = (TInt)Convert.ChangeType(Int16.MinValue, IntType);
            private static readonly TInt Int16Max = (TInt)Convert.ChangeType(Int16.MaxValue, IntType);

            private static readonly TInt UInt16Min = (TInt)Convert.ChangeType(UInt16.MinValue, IntType);
            private static readonly TInt UInt16Max = (TInt)Convert.ChangeType(UInt16.MaxValue, IntType);

            private static readonly TInt Int32Min = (TInt)Convert.ChangeType(Int32.MinValue, IntType);
            private static readonly TInt Int32Max = (TInt)Convert.ChangeType(Int32.MaxValue, IntType);

            private static readonly TInt UInt32Min = (TInt)Convert.ChangeType(0, IntType);
            private static readonly TInt UInt32Max = (TInt)Convert.ChangeType(Int32.MaxValue, IntType);

            private static readonly TInt Int64Min = (TInt)Convert.ChangeType(Int32.MinValue, IntType);
            private static readonly TInt Int64Max = (TInt)Convert.ChangeType(Int32.MaxValue, IntType);

            private static readonly TInt UInt64Min = (TInt)Convert.ChangeType(0, IntType);
            private static readonly TInt UInt64Max = (TInt)Convert.ChangeType(Int32.MaxValue, IntType);

            private static readonly TReal SingleMin = (TReal)Convert.ChangeType(Single.MinValue, RealType);
            private static readonly TReal SingleMax = (TReal)Convert.ChangeType(Single.MaxValue, RealType);

            private static readonly TReal DoubleMax = (TReal)Convert.ChangeType(Double.MaxValue, RealType);
            private static readonly TReal DoubleMin = (TReal)Convert.ChangeType(Double.MinValue, RealType);

            private static readonly TReal DecimalMax = (TReal)Convert.ChangeType(Decimal.MaxValue, RealType);
            private static readonly TReal DecimalMin = (TReal)Convert.ChangeType(Decimal.MinValue, RealType);

            public IReadOnlyCollection<Expression> ValueConstraints 
            {
                get
                {
                    return _valueConstraints;
                }
            }

            public IReadOnlyDictionary<string, ParameterExpression> Cache
            {
                get
                {
                    return _cache;
                }
            }

            private readonly Dictionary<string, ParameterExpression> _cache = new Dictionary<string, ParameterExpression>();
            private readonly List<Expression> _valueConstraints = new List<Expression>();

            private ParameterExpression CreateIntegerParameter(string name, TInt min, TInt max)
            {
                ParameterExpression parameter = Expression.Parameter(IntType, name);
                _cache[name] = parameter;
                
                Expression greaterThan = Expression.GreaterThanOrEqual(parameter, Expression.Constant(min, IntType));
                Expression lessThan = Expression.LessThanOrEqual(parameter, Expression.Constant(max, IntType));

                _valueConstraints.Add(Expression.And(lessThan, greaterThan));

                return parameter;
            }

            private ParameterExpression CreateRealParameter(string name, TReal min, TReal max)
            {
                ParameterExpression parameter = Expression.Parameter(IntType, name);
                _cache[name] = parameter;

                Expression greaterThan = Expression.GreaterThanOrEqual(parameter, Expression.Constant(min, RealType));
                Expression lessThan = Expression.LessThanOrEqual(parameter, Expression.Constant(max, RealType));

                _valueConstraints.Add(greaterThan);
                _valueConstraints.Add(lessThan);

                return parameter;
            }

            private ParameterExpression CreateBoolParameter(string name)
            {
                ParameterExpression parameter = Expression.Parameter(BoolType, name);
                _cache[name] = parameter;

                return parameter;
            }


            protected override Expression VisitConstant(ConstantExpression constant)
            {
                Type type = constant.Type;
                Type z3Type = GetZ3TypeFor(type);
                if (z3Type == type) return constant;
                return Expression.Constant(Convert.ChangeType(constant.Value, z3Type), z3Type);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                string name = node.Name;
                Type type = node.Type;
                
                if (_cache.TryGetValue(name, out var convertedParameter)) return convertedParameter;

                if (type == IntType || type == RealType || type == BoolType)
                {
                    _cache[name] = node;
                    return node;
                }


                // if parameter is integer type => setup int32 parameter with constraints of min/max values (add the expressions into the ValueConstraints list
                switch (Type.GetTypeCode(node.Type))
                {
                    // bool types
                    case TypeCode.Boolean:
                        return CreateBoolParameter(name);

                    // in types
                    //case TypeCode.Char:
                    //    // char as a single value (integer arithmetics) VS a single element string (theory of sequences)
                    //    // the char sort exists within the Z3 => use it...
                    //    _cache[node.Name] = node;
                    //    return node;
                    case TypeCode.Char:
                        return CreateIntegerParameter(name, CharMin, CharMax);

                    case TypeCode.SByte:
                        return CreateIntegerParameter(name, SByteMin, SByteMax);

                    case TypeCode.Byte:
                        return CreateIntegerParameter(name, ByteMin, ByteMax);

                    case TypeCode.Int16:
                        return CreateIntegerParameter(name, Int16Min, Int16Max);

                    case TypeCode.UInt16:
                        return CreateIntegerParameter(name, UInt16Min, UInt16Max);

                    case TypeCode.Int32:
                        return CreateIntegerParameter(name, Int32Min, Int32Max);

                    case TypeCode.UInt32:
                        return CreateIntegerParameter(name, UInt32Min, UInt32Max);

                    case TypeCode.Int64:
                        return CreateIntegerParameter(name, Int64Min, Int64Max);

                    case TypeCode.UInt64:
                        return CreateIntegerParameter(name, UInt64Min, UInt64Max);


                    case TypeCode.Single:
                        return CreateRealParameter(name, SingleMin, SingleMax);

                    case TypeCode.Double:
                        return CreateRealParameter(name, DoubleMin, DoubleMax);

                    case TypeCode.Decimal:
                        return CreateRealParameter(name, DecimalMin, DecimalMax);

                    case TypeCode.String:
                        // no need to transform this parameter
                        _cache[node.Name] = node;
                        return node;

                    default:
                        throw new NotSupportedException();
                }
            }

            private static Type GetZ3TypeFor(Type type)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return BoolType;

                    case TypeCode.Char:
                    //    return typeof(char);

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return IntType;

                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return RealType;

                    case TypeCode.String:
                        return typeof(string);

                    default:
                        throw new NotSupportedException();
                }
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                Expression operand = Visit(node.Operand);

                if (node.NodeType == ExpressionType.Convert)
                {
                    if (node.Type == operand.Type)
                    {
                        // our visit changed the type of the operand to the cast target
                        // => we can omit the cast operation and just return the operand
                        return operand;
                    }

                    Type z3Target = GetZ3TypeFor(node.Type);

                    return Expression.Convert(operand, z3Target);
                }
                else
                {
                    return node.Update(operand);
                }
            }
        }

        public Dictionary<string, object> Solve(Expression expression, IList<ParameterExpression> parameters)
        {
            // first, we must make sure the expression contains only Z3 supported types, e.g.
            // int32, double and bool
            Z3TypeTranslator<int, double, bool> visitor = new Z3TypeTranslator<int, double, bool>();
            expression = visitor.Visit(expression);
            if (visitor.ValueConstraints.Count > 0)
            {
                expression = visitor
                    .ValueConstraints
                    .Append(expression)
                    .Aggregate((e1, e2) => Expression.And(e1, e2))
                    .Simplify();
            }

            // all parameters should be transformed to Z3 supported types, e.g. int, double and bool => parameters list is no longer usable, we use the cache of the visitor to recreate it...
            parameters = visitor.Cache.Values.ToList();

            using (var ctx = new Z3Context())
            {
                ctx.Log = Console.Out; // see internal logging

                var builder = new ClassBuilder("Parameters");
                var myclass = builder.CreateObject(
                    parameters.Select(p => p.Name).ToArray(),
                    parameters.Select(p => p.Type).ToArray());

                var TP = myclass.GetType();

                var context = Activator.CreateInstance(TP);

                foreach (var param in parameters)
                {
                    var getDefaultValue =
                        Expression.Lambda<Func<object>>(
                            // it needs to be casted to `object` to create `Func<object>`
                            Expression.Convert(
                                Expression.Default(param.Type),
                                typeof(object)
                            )
                        ).Compile();
                }

                //var method = ctx.GetType().GetMethods().Single(m => m.Name == "NewTheorem" && m.GetParameters().Length == 0);
                //var theorem = ( method.MakeGenericMethod(TP)

                var theorem = new dnWalker.Z3.Theorem(TP, ctx, new List<LambdaExpression> { Expression.Lambda(expression, parameters) });
                // TODO
                /*var theorem = ctx
                    .NewTheorem(context)
                    .Where(Expression.Lambda(expression, parameters));*/
                
                var result = theorem.Solve();
                if (result == null)
                {
                    return null;
                }

                return result.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(result, null));
            }
        }
    }

    internal class ExpandoTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICollection<KeyValuePair<string, object>> eoColl;

        public ExpandoTypeDescriptionProvider(ICollection<KeyValuePair<string, object>> eoColl)
        {
            this.eoColl = eoColl;
        }

        public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
        {
            return new ExpandoObjectTypeDescriptor(instance);
        }
    }

    public class ExpandoObjectTypeDescriptor : ICustomTypeDescriptor
    {
        private readonly IDictionary<string, object> m_Instance;

        public ExpandoObjectTypeDescriptor(dynamic instance)
        {
            m_Instance = instance as IDictionary<string, object>;
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return m_Instance;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(
                m_Instance.Keys
                          .Select(x => new ExpandoObjectPropertyDescriptor(m_Instance, x))
                          .ToArray<PropertyDescriptor>());
        }

        class ExpandoObjectPropertyDescriptor : PropertyDescriptor
        {
            private readonly IDictionary<string, object> m_Instance;
            private readonly string m_Name;

            public ExpandoObjectPropertyDescriptor(IDictionary<string, object> instance, string name)
                : base(name, null)
            {
                m_Instance = instance;
                m_Name = name;
            }

            public override Type PropertyType
            {
                get { return m_Instance[m_Name].GetType(); }
            }

            public override void SetValue(object component, object value)
            {
                m_Instance[m_Name] = value;
            }

            public override object GetValue(object component)
            {
                return m_Instance[m_Name];
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type ComponentType
            {
                get { return null; }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override void ResetValue(object component)
            {
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override string Category
            {
                get { return string.Empty; }
            }

            public override string Description
            {
                get { return string.Empty; }
            }
        }
    }
}
